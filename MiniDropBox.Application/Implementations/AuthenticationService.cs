using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MiniDropBox.Application.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<string> Authenticate(string username, string password)
        {
            var user = (await _userRepository.GetAllAsync()).FirstOrDefault(u => u.Username == username);

            if (user == null || user.Password != password)
            {
                return null!;
            }

            var secretKey = _configuration["JwtSettings:JwtKey"];

            //Create JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "MiniDropBoxApi",
                audience: "MiniDropBoxClient",
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
