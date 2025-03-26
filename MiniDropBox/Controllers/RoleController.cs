using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Models;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] RoleDTO roleDTO)
        {
            if (roleDTO == null || string.IsNullOrEmpty(roleDTO.Name) || string.IsNullOrEmpty(roleDTO.Description))
            {
                return BadRequest("Name and Description of role cannot be null");
            }

            var role = await _roleService.CreateRoleAsync(roleDTO);

            return CreatedAtAction(nameof(CreateRole), new { id = role.Id }, role);
        }
    }
}
