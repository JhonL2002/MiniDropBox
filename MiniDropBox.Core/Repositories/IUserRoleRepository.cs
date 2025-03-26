using MiniDropBox.Core.Models;

namespace MiniDropBox.Core.Repositories
{
    public interface IUserRoleRepository
    {
        Task<UserRole> AddAsync(UserRole userRole);
        Task<UserRole?> GetByIdAsync(int userRoleId);
        Task<UserRole?> GetByUserIdAsync(int userId);
        Task<IEnumerable<UserRole>> GetAllAsync();
        Task<UserRole?> UpdateAsync(UserRole userRole);
        Task<UserRole?> DeleteAsync(int userRoleId);
    }
}
