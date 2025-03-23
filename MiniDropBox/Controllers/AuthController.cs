using Microsoft.AspNetCore.Mvc;
using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;

namespace MiniDropBox.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthController(IAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
        {
            var token = await _authService.Authenticate(loginDTO.Username, loginDTO.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(new {token});
        }
    }
}
