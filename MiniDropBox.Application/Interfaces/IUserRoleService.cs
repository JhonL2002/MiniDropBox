using MiniDropBox.Application.DTOs;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<UserRoleDTO> CreateUserRoleAsync(UserRoleDTO userRoleDTO);
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
    }
}
