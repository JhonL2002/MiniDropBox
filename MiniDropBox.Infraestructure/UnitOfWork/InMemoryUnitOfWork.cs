using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Models;
using MiniDropBox.Infraestructure.Repositories;

namespace MiniDropBox.Infraestructure.UnitOfWork
{
    public class InMemoryUnitOfWork : IUnitOfWork
    {
        private List<User> _usersBackup = new();
        private List<UserRole> _userRolesBackup = new();

        private readonly UserRepository _userRepository;
        private readonly UserRoleRepository _userRoleRepository;

        public InMemoryUnitOfWork(UserRepository userRepository, UserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
        }

        public Task BeginTransactionAsync()
        {
            _usersBackup = _userRepository.Users.ToList();
            _userRolesBackup = _userRoleRepository.UsersRoles.ToList();
            return Task.CompletedTask;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public Task RollbackAsync()
        {
            _userRepository.Users = _usersBackup.ToList();
            _userRoleRepository.UsersRoles = _userRolesBackup.ToList();
            return Task.CompletedTask;
        }
    }
}
