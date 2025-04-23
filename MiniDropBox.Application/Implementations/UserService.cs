using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.Helpers;
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
        private readonly IFolderRepository _folderRepository;
        private readonly IPasswordService _passwordService;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository, IUnitOfWork unitOfWork, IPasswordService passwordService, IFolderRepository folderRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _folderRepository = folderRepository;
        }

        public async Task<Result<UserDTO>> CreateUserAsync(UserDTO userDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var defaultRole = await _roleRepository.GetByNameAsync("User");
                if (defaultRole == null)
                {
                    return Result<UserDTO>.Failure("Default role not found");
                }

                var existingUser = await _userRepository.GetByEmailOrUsernameAsync(userDTO.Email, userDTO.UserName);
                if (existingUser != null)
                {
                    return Result<UserDTO>.Failure("User with this email or username already exists");
                }

                var hashedPassword = _passwordService.HashPassword(userDTO.Password);

                var user = new User
                {
                    Username = userDTO.UserName,
                    Password = hashedPassword,
                    Email = userDTO.Email,
                    CreatedAt = DateTime.UtcNow,
                };

                await _userRepository.AddAsync(user);

                // Create the root folder for the user
                var rootFolder = new Folder
                {
                    Name = $"{user.Username}",
                    User = user,
                    CreatedAt = DateTime.UtcNow,
                    Path = $"{user.Username}",
                    ParentFolderId = null
                };

                await _folderRepository.AddAsync(rootFolder);

                var userRole = new UserRole
                {
                    User = user,
                    RoleId = defaultRole.Id
                };

                await _userRoleRepository.AddAsync(userRole);
                await _unitOfWork.CommitAsync();

                return Result<UserDTO>.Success(new UserDTO(
                    user.Username,
                    user.Email,
                    user.Password
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
