using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
        {
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserRoleDTO> CreateUserRoleAsync(UserRoleDTO userRoleDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var userRole = new UserRole
                {
                    UserId = userRoleDTO.UserId,
                    RoleId = userRoleDTO.RoleId
                };

                var createdUserRole = await _userRoleRepository.AddAsync(userRole);
                await _unitOfWork.CommitAsync();

                return new UserRoleDTO(
                    createdUserRole.UserId,
                    createdUserRole.RoleId
                );
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await _userRoleRepository.GetAllAsync();
        }

        public async Task<Result<string>> UpdateAsync(UserRoleDTO userRoleDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existingUserRole = await _userRoleRepository.GetByUserIdAsync(userRoleDTO.UserId);
                if (existingUserRole == null)
                {
                    return Result<string>.Failure("User role not found");
                }

                await _userRoleRepository.DeleteAsync(existingUserRole.UserId, existingUserRole.RoleId);

                var updatedUserRole = new UserRole
                {
                    UserId = userRoleDTO.UserId,
                    RoleId = userRoleDTO.RoleId
                };

                await _userRoleRepository.AddAsync(updatedUserRole);
                await _unitOfWork.CommitAsync();

                return Result<string>.Success("User role updated successfully");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}
