namespace MiniDropBox.Application.DTOs.Nodes
{
    public class FolderTreeNodeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public int? ParentFolderId { get; set; }

        public List<FolderTreeNodeDTO> SubFolders { get; set; } = new();
    }
}
