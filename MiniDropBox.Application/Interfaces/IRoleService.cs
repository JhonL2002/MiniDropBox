using MiniDropBox.Application.DTOs;

namespace MiniDropBox.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDTO> CreateRoleAsync(RoleDTO roleDTO);
    }
}
