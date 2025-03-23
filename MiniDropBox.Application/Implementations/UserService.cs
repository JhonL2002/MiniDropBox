using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDTO> CreateUserAsync(UserDTO userDTO)
        {
            var user = new User
            {
                Id = userDTO.Id,
                Username = userDTO.UserName,
                Password = userDTO.Password,
                Email = userDTO.Email,
                CreatedAt = DateTime.UtcNow,
            };

            var createdUser = await _userRepository.AddAsync(user);
            return new UserDTO(
                createdUser.Id,
                createdUser.Username,
                createdUser.Email,
                createdUser.Password
            );
        }
    }
}
