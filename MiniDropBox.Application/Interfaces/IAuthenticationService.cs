using MiniDropBox.Application.DTOs;

namespace MiniDropBox.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<string>> Authenticate(string username, string password);
    }
}
