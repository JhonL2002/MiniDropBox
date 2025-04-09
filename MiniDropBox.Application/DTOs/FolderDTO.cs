namespace MiniDropBox.Application.DTOs
{
    public record FolderDTO(string Name, int? ParentFolderId, int UserId);
}
