using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
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
                Id = userRoleDTO.Id,
                UserId = userRoleDTO.UserId,
                RoleId = userRoleDTO.RoleId
            };

            var createdUserRole = await _userRoleRepository.AddAsync(userRole);

            return new UserRoleDTO(
                createdUserRole.Id,
                createdUserRole.UserId,
                createdUserRole.RoleId
            );
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await _userRoleRepository.GetAllAsync();
        }
    }
}
