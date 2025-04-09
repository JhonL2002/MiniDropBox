using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }


        [HttpPost]
        public async Task<IActionResult> CreateUserRole([FromBody] UserRoleDTO userRoleDTO)
        {
            if (userRoleDTO.UserId < 1 || userRoleDTO.RoleId < 1)
            {
                return BadRequest("Invalid user id or role id");
            }

            var userRole = await _userRoleService.CreateUserRoleAsync(userRoleDTO);
            return Ok(userRole);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUserRoles()
        {
            var result = await _userRoleService.GetAllUserRolesAsync();
            if (result == null)
            {
                return NoContent();
            }
            return Ok(result);
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUserRole(int userId, [FromBody] UserRoleDTO userRoleDTO)
        {
            if (userId != userRoleDTO.UserId)
            {
                return BadRequest("User ID in route and body must match");
            }
            
            var result = await _userRoleService.UpdateAsync(userRoleDTO);
            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            return NotFound(result.Error);
        }
    }
}
