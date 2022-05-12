using AutoMapper;
using Entities.Exceptions;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserDto> GetUserByUserNameAsync(string userName)
        {
            var user = await _userManager.Users
                .Include(t => t.ActivityUserDataCount)
                .Where(t => t.UserName == userName)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                throw new UserNotFoundException(userName);
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
