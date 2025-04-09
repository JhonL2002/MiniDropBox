using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.DTOs.Nodes;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.Trees;
using MiniDropBox.Application.Interfaces.UnitOfWork;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFolderTreeBuilderService _treeBuilder;
        private readonly IUnitOfWork _unitOfWork;

        public FolderService(IFolderRepository folderRepository, IFolderTreeBuilderService treeBuilder, IUnitOfWork unitOfWork)
        {
            _folderRepository = folderRepository;
            _treeBuilder = treeBuilder;
            _unitOfWork = unitOfWork;
        }

        public async Task<FolderDTO> CreateFolderAsync(FolderDTO folderDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var parentFolder = await _folderRepository.GetByIdAsync(folderDTO.ParentFolderId!.Value);
                bool isSubFolder = parentFolder != null;

                var folder = new Folder
                {
                    Name = folderDTO.Name,
                    ParentFolderId = isSubFolder ? folderDTO.ParentFolderId : null,
                    UserId = folderDTO.UserId,
                    CreatedAt = DateTime.UtcNow,
                    Path = isSubFolder
                        ? Path.Combine($"{parentFolder!.Path}", $"{folderDTO.Name}")
                        : Path.Combine($"{folderDTO.Name}",$"{folderDTO.UserId}")
                };

                if (isSubFolder)
                {
                    folder.ParentFolder = parentFolder;
                }

                var createdFolder = await _folderRepository.AddAsync(folder);
                await _unitOfWork.CommitAsync();

                return new FolderDTO(
                    createdFolder.Name,
                    createdFolder.ParentFolderId,
                    createdFolder.UserId
                );
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteFolderAsync(int folderId)
        {
            var deletedFolder = await _folderRepository.DeleteAsync(folderId);
            return deletedFolder != null;
        }

        public async Task<Folder?> GetFolderByIdAsync(int folderId)
        {
            return await _folderRepository.GetByIdAsync(folderId);
        }

        public async Task<Folder?> GetFolderByNameAsync(string folderName)
        {
            return await _folderRepository.GetByNameAsync(folderName);
        }

        public async Task<List<FolderTreeNodeDTO>> GetTreeForUserAsync(int userId)
        {
            var folders = await _folderRepository.GetFoldersByUserIdAsync(userId);
            return _treeBuilder.BuildTree(folders);
        }

        public async Task<bool> UpdateFolderAsync(Folder folder)
        {
            var updatedFolder = await _folderRepository.UpdateAsync(folder);
            return updatedFolder != null;
        }
    }
}
