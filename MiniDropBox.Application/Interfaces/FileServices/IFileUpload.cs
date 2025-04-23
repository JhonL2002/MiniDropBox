namespace MiniDropBox.Application.Interfaces.FileServices
{
    public interface IFileUpload
    {
        string FileName { get; }
        long Lenght { get; }
        Stream OpenReadStream();
    }
}
