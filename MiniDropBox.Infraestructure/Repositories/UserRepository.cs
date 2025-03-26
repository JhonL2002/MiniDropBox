using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        public List<User> Users { get; set; } = new();

        public Task<User> AddAsync(User user)
        {
            Users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User?> DeleteAsync(int userId)
        {
            var user = Users.FirstOrDefault(u => u.Id == userId);
            if (user != null)
            {
                Users.Remove(user);
                return Task.FromResult<User?>(user);
            }

            return Task.FromResult<User?>(null);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.Run(() => Users.AsEnumerable());
        }

        public Task<User?> GetByIdAsync(int userId)
        {
            return Task.Run(() => Users.FirstOrDefault(u => u.Id == userId));
        }

        public Task<User?> GetByUsernameAsync(string username)
        {
            var user = Users.FirstOrDefault(u => u.Username == username);
            return Task.FromResult(user);
        }

        public Task<User?> UpdateAsync(User user)
        {
            var existingUser = Users.FirstOrDefault(u => u.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                return Task.FromResult<User?>(existingUser);
            }

            return Task.FromResult<User?>(null);
        }
    }
}
