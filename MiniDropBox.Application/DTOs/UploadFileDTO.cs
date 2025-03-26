namespace MiniDropBox.Application.DTOs
{
    public record UploadFileDTO<T>(T File, string FolderPath);
}
