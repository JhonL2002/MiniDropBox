using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.UnitOfWork;
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
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(IRoleRepository roleRepository, IUnitOfWork unitOfWork)
        {
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<RoleDTO> CreateRoleAsync(RoleDTO roleDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var role = new Role
                {
                    Id = roleDTO.Id,
                    Name = roleDTO.Name,
                    Description = roleDTO.Description
                };

                var createdRole = await _roleRepository.AddAsync(role);

                await _unitOfWork.CommitAsync();

                return new RoleDTO(
                    createdRole.Id,
                    createdRole.Name,
                    createdRole.Description
                );
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
