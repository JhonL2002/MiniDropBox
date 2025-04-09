using Microsoft.EntityFrameworkCore;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.Data;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;

        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<UserRole> AddAsync(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
            return Task.FromResult(userRole);
        }

        public async Task<UserRole?> DeleteAsync(int userId, int roleId)
        {
            var existing = await _context.UserRoles
                .FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (existing != null)
            {
                _context.UserRoles.Remove(existing);
            }
            return existing;
        }

        public async Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return await _context.UserRoles.ToListAsync();
        }

        public async Task<UserRole?> GetByUserIdAsync(int userId)
        {
            return await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId);
        }

        public async Task<UserRole?> UpdateAsync(UserRole updatedUserRole)
        {
            var existingUserRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == updatedUserRole.UserId);
            if (existingUserRole != null)
            {
                existingUserRole.RoleId = updatedUserRole.RoleId;
                return existingUserRole;
            }
            return null;
        }
    }
}
