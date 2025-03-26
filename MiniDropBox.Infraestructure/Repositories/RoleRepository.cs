using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly List<Role> _roles = new();

        public Task<Role> AddAsync(Role role)
        {
            _roles.Add(role);
            return Task.FromResult(role);
        }

        public Task<Role?> DeleteAsync(int roleId)
        {
            var role = _roles.FirstOrDefault(f => f.Id == roleId);
            if (role != null)
            {
                _roles.Remove(role);
                return Task.FromResult<Role?>(role);
            }

            return Task.FromResult<Role?>(null);
        }

        public Task<IEnumerable<Role>> GetAllAsync()
        {
            return Task.FromResult(_roles.AsEnumerable());
        }

        public Task<Role?> GetByIdAsync(int roleId)
        {
            var role = _roles.FirstOrDefault(f => f.Id == roleId);
            return Task.FromResult(role);
        }

        public Task<Role?> UpdateAsync(Role role)
        {
            var existingRole = _roles.FirstOrDefault(f => f.Id == role.Id);
            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                return Task.FromResult<Role?>(existingRole);
            }

            return Task.FromResult<Role?>(null);
        }
    }
}
