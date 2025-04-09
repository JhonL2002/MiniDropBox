using MiniDropBox.Application.DTOs.Nodes;

namespace MiniDropBox.Application.DTOs.Trees
{
    public class FolderTreeDTO
    {
        public List<FolderTreeNodeDTO> RootFolders { get; set; } = new();
    }
}
