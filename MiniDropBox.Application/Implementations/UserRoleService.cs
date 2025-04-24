using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }

        public async Task<UserRoleDTO> CreateUserRoleAsync(UserRoleDTO userRoleDTO)
        {
            var userRole = new UserRole
            {
                UserId = userRoleDTO.UserId,
                RoleId = userRoleDTO.RoleId
            };

            var createdUserRole = await _userRoleRepository.AddAsync(userRole);

            return new UserRoleDTO(
                createdUserRole.UserId,
                createdUserRole.RoleId
            );
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await _userRoleRepository.GetAllAsync();
        }

        public async Task<Result<string>> UpdateAsync(UserRoleDTO userRoleDTO)
        {
            var existingUserRole = await _userRoleRepository.GetByUserIdAsync(userRoleDTO.UserId);
            if (existingUserRole == null)
            {
                return Result<string>.Failure("User role not found");
            }

            await _userRoleRepository.DeleteAsync(existingUserRole.UserId, existingUserRole.RoleId);

            var updatedUserRole = new UserRole
            {
                UserId = userRoleDTO.UserId,
                RoleId = userRoleDTO.RoleId
            };

            await _userRoleRepository.AddAsync(updatedUserRole);

            return Result<string>.Success("User role updated successfully");
        }
    }
}
