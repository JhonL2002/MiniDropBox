using MiniDropBox.Core.Models;

namespace MiniDropBox.Core.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> AddAsync(Role role);
        Task<Role?> GetByIdAsync(int roleId);
        Task<Role?> GetByNameAsync(string roleName);
        Task<IEnumerable<Role>> GetAllAsync();
        Task<Role?> UpdateAsync(Role role);
        Task<Role?> DeleteAsync(int roleId);
    }
}
