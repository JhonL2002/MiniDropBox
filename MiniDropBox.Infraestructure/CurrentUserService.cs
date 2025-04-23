using Microsoft.AspNetCore.Http;
using MiniDropBox.Application.Interfaces;
using System.Security.Claims;

namespace MiniDropBox.Infraestructure
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserId =>
            _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public bool IsInRole(string role) =>
            _httpContextAccessor.HttpContext?.User.IsInRole(role) ?? false;
    }
}
