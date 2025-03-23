using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO userDTO)
        {
            if (userDTO == null || string.IsNullOrWhiteSpace(userDTO.UserName) || string.IsNullOrEmpty(userDTO.Password) || string.IsNullOrEmpty(userDTO.Email))
            {
                return BadRequest("User name, email and password cannot be empty");
            }

            var createdUser = await _userService.CreateUserAsync(userDTO);

            return CreatedAtAction(nameof(CreateUser), new { id = createdUser.Id }, createdUser);
        }
    }
}
