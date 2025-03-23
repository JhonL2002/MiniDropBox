using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users = new();

        public Task<User> AddAsync(User user)
        {
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User?> DeleteAsync(int userId)
        {
            var user = _users.FirstOrDefault(f => f.Id == userId);
            if (user != null)
            {
                _users.Remove(user);
                return Task.FromResult<User?>(user);
            }

            return Task.FromResult<User?>(null);
        }

        public Task<IEnumerable<User>> GetAllAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<User?> GetByIdAsync(int userId)
        {
            var user = _users.FirstOrDefault(f => f.Id == userId);
            return Task.FromResult(user);
        }

        public Task<User?> UpdateAsync(User user)
        {
            var existingUser = _users.FirstOrDefault(f => f.Id == user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                return Task.FromResult<User?>(existingUser);
            }

            return Task.FromResult<User?>(existingUser);
        }
    }
}
