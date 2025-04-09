using Microsoft.EntityFrameworkCore;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly AppDbContext _context;

        public RoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Role> AddAsync(Role role)
        {
            _context.Roles.Add(role);
            return Task.FromResult(role);
        }

        public async Task<Role?> DeleteAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            if (role != null)
            {
                _context.Roles.Remove(role);
            }
            return role;
        }

        public async Task<IEnumerable<Role>> GetAllAsync()
        {
            return await _context.Roles.ToListAsync();
        }

        public async Task<Role?> GetByIdAsync(int roleId)
        {
            var role = await _context.Roles.FindAsync(roleId);
            return role;
        }

        public Task<Role?> GetByNameAsync(string roleName)
        {
            var role = _context.Roles.FirstOrDefaultAsync(r => r.Name == roleName);
            return role;
        }

        public async Task<Role?> UpdateAsync(Role role)
        {
            var existingRole = await _context.Roles.FindAsync(role.Id);
            if (existingRole != null)
            {
                existingRole.Name = role.Name;
                existingRole.Description = role.Description;
            }
            return null;
        }
    }
}
