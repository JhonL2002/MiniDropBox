using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniDropBox.Application.DTOs;
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
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IUserRepository userRepository, IConfiguration configuration, IUserRoleRepository userRoleRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _userRoleRepository = userRoleRepository;
            _roleRepository = roleRepository;
        }

        public async Task<Result<string>> Authenticate(string username, string password)
        {
            var secretKey = _configuration["JwtSettings:JwtKey"];

            var user = (await _userRepository.GetByUsernameAsync(username));

            if (user == null)
            {
                return Result<string>.Failure("User does not exist");
            }

            if (user.Password != password || user.Username != username)
            {
                return Result<string>.Failure("Invalid login attempt, verify credentials");
            }

            var userRole = await _userRoleRepository.GetByUserIdAsync(user.Id);

            if (userRole == null)
            {
                return Result<string>.Failure("No role assigned to this user.");
            }

            var role = await _roleRepository.GetByIdAsync(userRole.RoleId);

            if (role == null)
            {
                return Result<string>.Failure("Assigned role does not exist.");
            }

            //Create JWT
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: "MiniDropBoxApi",
                audience: "MiniDropBoxClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return Result<string>.Success(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}
