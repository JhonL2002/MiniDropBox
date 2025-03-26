using MiniDropBox.Application.DTOs;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Application.Interfaces
{
    public interface IFolderService
    {
        Task<FolderDTO> CreateFolderAsync(FolderDTO folderDTO);
        Task<Folder?> GetFolderByIdAsync(int folderId);
        Task<Folder?> GetFolderByNameAsync(string folderName);
        Task<IEnumerable<Folder>> GetAllFoldersAsync();
        Task<bool> UpdateFolderAsync(Folder folder);
        Task<bool> DeleteFolderAsync(int folderId);
    }
}
