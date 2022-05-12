using AutoMapper;
using Contracts;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Shared.DataTransferObjects;
using Service.Contracts;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using Entities.Exceptions;

namespace Service;

public sealed class AuthenticationService : IAuthenticationService
{
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private User? _user;

    public AuthenticationService(ILoggerManager logger, IMapper mapper,
        UserManager<User> userManager, IConfiguration configuration)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<TokenDto> CreateToken(bool populateExp)
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

        var refreshToken = GenerateRefreshToken();
        _user!.RefreshToken = refreshToken;
        if (populateExp)
            _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(7);
        await _userManager.UpdateAsync(_user);
        var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        return new TokenDto(accessToken, refreshToken);
    }

    public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);
        if (result.Succeeded && userForRegistration.Roles != null && userForRegistration.Roles.Any())
        {
            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);
        }
        return result;
    }

    public async Task<bool> VerifyUserAsync(UserForAuthenticationDto userForAuthentication)
    {
        _user = await _userManager.FindByNameAsync(userForAuthentication.UserName);
        var result = _user != null && await _userManager.CheckPasswordAsync(_user, userForAuthentication.Password);
        if (!result)
        {
            _logger.LogWarn($"{nameof(VerifyUserAsync)}: Authentication failed. Wrong username or password");
        }
        return result;
    }
    public async Task<TokenDto> RefreshToken(TokenDto token)
    {
        if(token.RefreshToken == null)
        {
            throw new RefreshTokenBadRequest();
        }
        var principal = GetPrincipalFromExpiredToken(token.AccessToken!);
        var user = await _userManager.FindByNameAsync(principal.Identity?.Name);
        if (user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
        {
            throw new RefreshTokenBadRequest();
        }
        _user = user;

        return await CreateToken(populateExp: false);
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, IEnumerable<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken
        (
            issuer: jwtSettings["validIssuer"],
            audience: jwtSettings["validAudience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["expires"])),
            signingCredentials: signingCredentials
        );
        return tokenOptions;
    }

    private async Task<IEnumerable<Claim>> GetClaims()
    {
        var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, _user!.UserName),
                new Claim(ClaimTypes.NameIdentifier, _user.Id),
                new Claim(ClaimTypes.Email, _user!.Email),
        };
        var roles = await _userManager.GetRolesAsync(_user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private static SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);

    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET")!)),
            ValidateLifetime = true,
            ValidIssuer = jwtSettings["validIssuer"],
            ValidAudience = jwtSettings["validAudience"]
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

        if (securityToken is JwtSecurityToken jwtSecurityToken)
        {
            if (!jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
        }
        return principal;
    }
}
