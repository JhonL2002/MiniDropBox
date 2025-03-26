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
        public async Task<IActionResult> CreateUserRoleAsync([FromBody] UserRoleDTO userRoleDTO)
        {
            if (userRoleDTO.UserId < 1 || userRoleDTO.RoleId < 1)
            {
                return BadRequest("Invalid user id or role id");
            }

            var userRole = await _userRoleService.CreateUserRoleAsync(userRoleDTO);
            return CreatedAtAction(nameof(CreateUserRoleAsync), new { id = userRole.Id }, userRole);
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
    }
}
