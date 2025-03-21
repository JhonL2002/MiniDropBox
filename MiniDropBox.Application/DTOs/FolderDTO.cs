namespace MiniDropBox.Application.DTOs
{
    public record FolderDTO(int Id, string Name, int ParentFolderId, int UserId);
}
