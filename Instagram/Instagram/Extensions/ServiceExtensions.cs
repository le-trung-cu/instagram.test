using Entities.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Repository;
using System.Text;
using Instagram.AuthorizationRequirement;
using Microsoft.AspNetCore.Authorization;
using Instagram.AuthorizationHandler;
using Microsoft.EntityFrameworkCore;
using Repository.Triggers;
using StackExchange.Redis;

namespace Instagram.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.WithOrigins("http://instagram.test");
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
                builder.AllowCredentials();
                builder.WithExposedHeaders("x-pagination");
            });

            //options.AddPolicy("CorsPolicy", builder =>
            //{
            //    builder.AllowAnyOrigin();
            //    builder.AllowAnyMethod();
            //    builder.AllowAnyHeader();
            //    builder.WithExposedHeaders("x-pagination");
            //});

            //options.AddPolicy("SignalRPolicy", builder =>
            //{
            //    builder.WithOrigins("http://localhost:4000").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            //});
        });
    }

    public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RepositoryContext>(options =>
       {
           options.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
           options.UseTriggers(triggerOptions =>
           {
               triggerOptions.AddTrigger<AfterSaveCommentTrigger>();
               triggerOptions.AddTrigger<AfterSaveLikeCommentTrigger>();
               triggerOptions.AddTrigger<AfterSaveLikePostTrigger>();
               triggerOptions.AddTrigger<AfterSavePostTrigger>();
               triggerOptions.AddTrigger<AfterSaveFollowTigger>();
           });
       });
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentity<User, IdentityRole>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
            o.User.RequireUniqueEmail = true;
        })
        .AddEntityFrameworkStores<RepositoryContext>()
        .AddDefaultTokenProviders();
    }

    public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Environment.GetEnvironmentVariable("SECRET")!;
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["validIssuer"],
                ValidAudience = jwtSettings["validAudience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/notify")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        });
    }

    public static void ConfigureAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("CanDeletePost", policyBuilder =>
            {
                policyBuilder.AddRequirements(new CanDeletePostRequirement());
            });

            options.AddPolicy("CanDeleteComment", policyBuilder =>
            {
                policyBuilder.AddRequirements(new CanDeleteCommentRequirement());
            });
        });

        services.AddScoped<IAuthorizationHandler, CanDeletePostHandler>();
        services.AddScoped<IAuthorizationHandler, CanDeleteCommentHandler>();
    }

    public static void ConfigureRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IConnectionMultiplexer>(options =>
        {
            return ConnectionMultiplexer.Connect(configuration.GetConnectionString("redis"));
        });
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration.GetConnectionString("redis");
            options.InstanceName = "Instagram";
        });
    }

    public static void ConfigureResponseCaching(this IServiceCollection services) => services.AddResponseCaching();
    public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
    {
        //services.AddHttpCacheHeaders();
        //services.AddHttpCacheHeaders(expirarionOptions =>
        //{
        //    expirarionOptions.MaxAge = 60;
        //    expirarionOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
        //}, validationOptions =>
        //{
        //    validationOptions.MustRevalidate = true;
        //});
    }
}

