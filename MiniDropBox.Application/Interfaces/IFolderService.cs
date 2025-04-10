using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.DTOs.Nodes;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Application.Interfaces
{
    public interface IFolderService
    {
        Task<FolderDTO> CreateFolderAsync(FolderDTO folderDTO);
        Task<Folder?> GetFolderByIdAsync(int folderId);
        Task<Folder?> GetFolderByNameAsync(string folderName);
        Task<List<FolderTreeNodeDTO>> GetTreeForUserAsync(int userId);
        Task<Result<string>> MoveFolderAsync(MoveFolderDTO moveFolderDTO, int userId);
        Task<Result<string>> UpdateFolderAsync(UpdateFolderDTO updateFolderDTO);
        Task<bool> DeleteFolderAsync(int folderId);
    }
}
