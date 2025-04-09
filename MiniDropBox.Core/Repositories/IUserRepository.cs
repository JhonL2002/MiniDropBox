using MiniDropBox.Core.Models;

namespace MiniDropBox.Core.Repositories
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User?> GetByIdAsync(int userId);
        Task<User?> GetByUsernameAsync(string username);
        Task<User?> GetByEmailOrUsernameAsync(string email, string username);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> UpdateAsync(User updatedUser);
        Task<User?> DeleteAsync(int userId);
    }
}
