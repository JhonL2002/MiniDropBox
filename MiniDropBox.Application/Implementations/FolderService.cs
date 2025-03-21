using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;

        public FolderService(IFolderRepository folderRepository)
        {
            _folderRepository = folderRepository;
        }

        public async Task<Folder> CreateFolderAsync(FolderDTO folderDTO)
        {
            var parentFolder = await _folderRepository.GetByIdAsync(folderDTO.ParentFolderId);
            bool isSubFolder = parentFolder != null;

            var folder = new Folder
            {
                Id = folderDTO.Id,
                Name = folderDTO.Name,
                ParentFolderId = isSubFolder ? folderDTO.ParentFolderId : null,
                UserId = folderDTO.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _folderRepository.AddAsync(folder);
            return folder;
        }

        public async Task<bool> DeleteFolderAsync(int folderId)
        {
            var deletedFolder = await _folderRepository.DeleteAsync(folderId);
            return deletedFolder != null;
        }

        public async Task<IEnumerable<Folder>> GetAllFoldersAsync()
        {
            return await _folderRepository.GetAllAsync();
        }

        public async Task<Folder?> GetFolderByIdAsync(int folderId)
        {
            return await _folderRepository.GetByIdAsync(folderId);
        }

        public async Task<bool> UpdateFolderAsync(Folder folder)
        {
            var updatedFolder = await _folderRepository.UpdateAsync(folder);
            return updatedFolder != null;
        }
    }
}
