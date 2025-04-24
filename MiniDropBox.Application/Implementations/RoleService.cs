using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleDTO> CreateRoleAsync(RoleDTO roleDTO)
        {
            var role = new Role
            {
                Name = roleDTO.Name,
                Description = roleDTO.Description
            };

            var createdRole = await _roleRepository.AddAsync(role);

            return new RoleDTO(
                createdRole.Name,
                createdRole.Description
            );
        }
    }
}
