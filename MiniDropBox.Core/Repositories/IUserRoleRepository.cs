using MiniDropBox.Core.Models;

namespace MiniDropBox.Core.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddAsync(UserRole userRole);
        Task<UserRole?> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<UserRole?> UpdateAsync(UserRole updatedUserRole);
        Task<UserRole?> DeleteAsync(int userId, int roleId);
    }
}
