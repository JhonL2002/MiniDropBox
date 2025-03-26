namespace MiniDropBox.Application.DTOs
{
    public record FileDTO(string Name, long Size, string Extension, string Path, int UserId, int FolderId);
}
