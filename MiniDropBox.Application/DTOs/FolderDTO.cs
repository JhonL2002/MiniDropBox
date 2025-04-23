namespace MiniDropBox.Application.DTOs
{
    public record FolderDTO(string Name, int? ParentFolderId);
    public record UpdateFolderDTO(int Id, string Name, int? ParentFolderId);
    public record MoveFolderDTO(int Id, int NewParentFolderId);
}
