using MiniDropBox.Application.DTOs;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Application.Interfaces
{
    public interface IFolderService
    {
        Task<Folder> CreateFolderAsync(FolderDTO folderDTO);
        Task<Folder?> GetFolderByIdAsync(int folderId);
        Task<IEnumerable<Folder>> GetAllFoldersAsync();
        Task<bool> UpdateFolderAsync(Folder folder);
        Task<bool> DeleteFolderAsync(int folderId);
    }
}
