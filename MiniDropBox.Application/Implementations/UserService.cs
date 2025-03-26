using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UserDTO>> CreateUserAsync(UserDTO userDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var defaultRole = await _roleRepository.GetByIdAsync(1);
                if (defaultRole == null)
                {
                    return Result<UserDTO>.Failure("Default role not found");
                }

                //To add (verify if user exists by email or username)
                var user = new User
                {
                    Id = userDTO.Id,
                    Username = userDTO.UserName,
                    Password = userDTO.Password, //Change by password hash later
                    Email = userDTO.Email,
                    CreatedAt = DateTime.UtcNow,
                };
                var createdUser = await _userRepository.AddAsync(user);

                var userRole = new UserRole
                {
                    UserId = createdUser.Id,
                    RoleId = defaultRole.Id
                };

                await _userRoleRepository.AddAsync(userRole);

                await _unitOfWork.CommitAsync();

                return Result<UserDTO>.Success(new UserDTO(
                    createdUser.Id,
                    createdUser.Username,
                    createdUser.Email,
                    createdUser.Password
                ));
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
