namespace MiniDropBox.Application.Interfaces
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(string username, string password);
    }
}
