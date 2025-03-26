using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Id = roleDTO.Id,
                Name = roleDTO.Name,
                Description = roleDTO.Description
            };

            var createdRole = await _roleRepository.AddAsync(role);

            return new RoleDTO(
                createdRole.Id,
                createdRole.Name,
                createdRole.Description
            );
        }
    }
}
