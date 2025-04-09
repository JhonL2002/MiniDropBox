using Microsoft.EntityFrameworkCore;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.Data;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            return Task.FromResult(user);
        }

        public async Task<User?> DeleteAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetByEmailOrUsernameAsync(string email, string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email || u.Username == username);
        }

        public async Task<User?> GetByIdAsync(int userId)
        {
            return await _context.Users.FindAsync(userId);
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<User?> UpdateAsync(User updatedUser)
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == updatedUser.Id);
            if (existingUser != null)
            {
                existingUser.Username = updatedUser.Username;
                existingUser.Email = updatedUser.Email;
                existingUser.Password = updatedUser.Password;
                _context.Users.Update(existingUser);
                return existingUser;
            }
            return null;
        }
    }
}
