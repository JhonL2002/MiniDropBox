namespace MiniDropBox.Application.Interfaces.Helpers
{
    public interface IPasswordService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
    }
}
