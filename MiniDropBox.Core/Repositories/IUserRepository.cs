using MiniDropBox.Core.Models;

namespace MiniDropBox.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> UpdateAsync(User user);
        Task<User?> DeleteAsync(int userId);
    }
}
