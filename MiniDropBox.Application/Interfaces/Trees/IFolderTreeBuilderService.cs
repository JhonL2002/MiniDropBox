using MiniDropBox.Application.DTOs.Nodes;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Application.Interfaces.Trees
{
    public interface IFolderTreeBuilderService
    {
        List<FolderTreeNodeDTO> BuildTree(List<Folder> folders);
    }
}
