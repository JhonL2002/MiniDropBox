namespace MiniDropBox.Application.DTOs
{
    public record UploadFileDTO<T>(T File, int FolderId, string FolderPath);
}
