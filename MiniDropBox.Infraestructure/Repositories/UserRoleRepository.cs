using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        public List<UserRole> UsersRoles { get; set; } = new();

        public Task<UserRole> AddAsync(UserRole userRole)
        {
            userRole.Id = UsersRoles.Count + 1;
            UsersRoles.Add(userRole);
            return Task.FromResult(userRole);
        }

        public Task<UserRole?> DeleteAsync(int userRoleId)
        {
            var userRole = UsersRoles.FirstOrDefault(f => f.Id == userRoleId);
            if (userRole != null)
            {
                UsersRoles.Remove(userRole);
                return Task.FromResult<UserRole?>(userRole);
            }

            return Task.FromResult<UserRole?>(null);
        }

        public Task<IEnumerable<UserRole>> GetAllAsync()
        {
            return Task.FromResult(UsersRoles.AsEnumerable());
        }

        public Task<UserRole?> GetByIdAsync(int userRoleId)
        {
            var userRole = UsersRoles.FirstOrDefault(ur => ur.Id == userRoleId);
            return Task.FromResult(userRole);

        }

        public Task<UserRole?> GetByUserIdAsync(int userId)
        {
            var userRole = UsersRoles.FirstOrDefault(ur => ur.UserId == userId);
            return Task.FromResult(userRole);
        }

        public Task<UserRole?> UpdateAsync(UserRole userRole)
        {
            var existingUserRole = UsersRoles.FirstOrDefault(f => f.Id == userRole.Id);
            if (existingUserRole != null)
            {
                existingUserRole.RoleId = userRole.RoleId;
                return Task.FromResult<UserRole?>(existingUserRole);
            }

            return Task.FromResult<UserRole?>(null);
        }
    }
}
