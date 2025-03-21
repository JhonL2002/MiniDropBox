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

        public async Task<FolderDTO> CreateFolderAsync(FolderDTO folderDTO)
        {
            var parentFolder = await _folderRepository.GetByIdAsync(folderDTO.ParentFolderId!.Value);
            bool isSubFolder = parentFolder != null;

            var folder = new Folder
            {
                Id = folderDTO.Id,
                Name = folderDTO.Name,
                ParentFolderId = isSubFolder ? folderDTO.ParentFolderId : null,
                UserId = folderDTO.UserId,
                CreatedAt = DateTime.UtcNow,
            };

            if (isSubFolder)
            {
                folder.ParentFolder = new Folder
                {
                    Id = parentFolder!.Id,
                    Name = parentFolder.Name,
                    ParentFolderId = parentFolder.ParentFolderId,
                    UserId = parentFolder.UserId,
                    CreatedAt = parentFolder.CreatedAt,
                };

                parentFolder!.SubFolders.Add(folder);
                await _folderRepository.UpdateAsync(parentFolder!);
            }

            var createdFolder = await _folderRepository.AddAsync(folder);
            
            return new FolderDTO(
                createdFolder.Id,
                createdFolder.Name,
                createdFolder.ParentFolderId,
                createdFolder.UserId
            );
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
