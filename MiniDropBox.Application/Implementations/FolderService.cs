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
        private readonly IFileRepository _fileRepository;
        private readonly ICurrentUserService _currentUser;
        private readonly IFolderTreeBuilderService _treeBuilder;
        private readonly IUnitOfWork _unitOfWork;

        public FolderService(IFolderRepository folderRepository, IFolderTreeBuilderService treeBuilder, IUnitOfWork unitOfWork, IFileRepository fileRepository, ICurrentUserService currentUser)
        {
            _folderRepository = folderRepository;
            _treeBuilder = treeBuilder;
            _unitOfWork = unitOfWork;
            _fileRepository = fileRepository;
            _currentUser = currentUser;
        }

        public async Task<Result<FolderDTO>> CreateFolderAsync(FolderDTO folderDTO, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                Folder parentFolder = null!;

                // Validate if is trying to create a root folder
                bool isRootFolder = !folderDTO.ParentFolderId.HasValue;

                if (isRootFolder)
                {
                    bool isAdmin = _currentUser.IsInRole("Admin");
                    if (!isAdmin)
                        return Result<FolderDTO>.Failure("You cannot create root folders");
                }

                if (folderDTO.ParentFolderId.HasValue)
                {
                    parentFolder = (await _folderRepository.GetByIdAsync(folderDTO.ParentFolderId.Value))!;
                    if (parentFolder == null || parentFolder.UserId != userId)
                        return Result<FolderDTO>.Failure("Invalid parent folder");
                }

                var newPath = parentFolder != null
                    ? Path.Combine(parentFolder.Path, folderDTO.Name)
                    : folderDTO.Name;

                var folder = new Folder
                {
                    Name = folderDTO.Name,
                    ParentFolderId = folderDTO.ParentFolderId,
                    ParentFolder = parentFolder,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    Path = newPath
                };

                var createdFolder = await _folderRepository.AddAsync(folder);
                await _unitOfWork.CommitAsync();

                var resultDTO = new FolderDTO
                (
                    createdFolder.Name,
                    createdFolder.ParentFolderId
                );

                return Result<FolderDTO>.Success(resultDTO);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteFolderAsync(int folderId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var folder = await _folderRepository.GetByIdAsync(folderId);
                if (folder == null)
                {
                    return false;
                }

                var hasChildren = folder.SubFolders != null && folder.SubFolders.Count != 0;
                if (hasChildren)
                    return false;

                var deletedFolder = await _folderRepository.DeleteAsync(folderId);
                await _unitOfWork.CommitAsync();
                return deletedFolder != null;
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
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

        public async Task<Result<string>> MoveFolderAsync(MoveFolderDTO moveFolderDTO, int userId)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var folder = await _folderRepository.GetByIdWithSubfoldersRecursivelyAsync(moveFolderDTO.Id);
                if (folder == null || folder.UserId != userId)
                    return Result<string>.Failure("Folder not found or you don't have permission");

                if (folder.Id == moveFolderDTO.NewParentFolderId)
                    return Result<string>.Failure("Cannot ove a folder into itslef");

                var newParent = await _folderRepository.GetByIdAsync(moveFolderDTO.NewParentFolderId);
                if (newParent == null || newParent.UserId != userId)
                    return Result<string>.Failure("New parent folder not found");

                // Avoid circular reference
                var temp = newParent;
                while (temp != null)
                {
                    if (temp.Id == folder.Id)
                        return Result<string>.Failure("Cannot move folder into its descendant (circular reference)");

                    temp = temp.ParentFolderId != null
                        ? await _folderRepository.GetByIdAsync(temp.ParentFolderId.Value)
                        : null;
                }

                // Update the folder's parent
                folder.ParentFolderId = newParent.Id;

                // Update the folder's and subfolders path 
                folder.Path = Path.Combine(newParent.Path, folder.Name);
                UpdateSubfolderPathsRecursively(folder);
                await UpdateFilePathsRecursively(folder);

                await _folderRepository.UpdateAsync(folder);
                await _unitOfWork.CommitAsync();

                return Result<string>.Success($"Folder moved to: {folder.Path}");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        public async Task<Result<string>> UpdateFolderAsync(UpdateFolderDTO updateFolderDTO)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var existing = await _folderRepository.GetByIdAsync(updateFolderDTO.Id);
                if (existing == null)
                    return Result<string>.Failure("Folder not found");

                if (updateFolderDTO.ParentFolderId == updateFolderDTO.Id)
                    return Result<string>.Failure("Folder cannot be its own parent");

                Folder? parentFolder = null;

                if (updateFolderDTO.ParentFolderId != null)
                {
                    parentFolder = await _folderRepository.GetByIdAsync(updateFolderDTO.ParentFolderId.Value);
                    if (parentFolder == null)
                        return Result<string>.Failure("Parent folder not found");

                    // Avoid circular reference
                    var temp = parentFolder;
                    while (temp != null)
                    {
                        if (temp.Id == existing.Id)
                            return Result<string>.Failure("Cannot assign a child folder as parent (circular reference)");

                        temp = temp.ParentFolderId !=null
                            ? await _folderRepository.GetByIdAsync(temp.ParentFolderId.Value)
                            : null;
                    }
                }

                existing.Name = updateFolderDTO.Name;
                existing.ParentFolderId = updateFolderDTO.ParentFolderId;

                existing.Path = parentFolder != null
                    ? Path.Combine(parentFolder.Path, updateFolderDTO.Name)
                    : Path.Combine(updateFolderDTO.Name, existing.UserId.ToString());

                var updatedFolder = await _folderRepository.UpdateAsync(existing);
                await _unitOfWork.CommitAsync();

                return Result<string>.Success($"Uploaded folder, new path: {updatedFolder!.Path} .");
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();
                throw;
            }
        }

        // Auxiliary method to update folder path recursively
        private void UpdateSubfolderPathsRecursively(Folder folder)
        {
            foreach (var sub in folder.SubFolders)
            {
                sub.Path = Path.Combine(folder.Path, sub.Name);
                UpdateSubfolderPathsRecursively(sub);
            }
        }

        // Auxiliary method to update files inside folders path recursively
        private async Task UpdateFilePathsRecursively(Folder folder)
        {
            var filesInFolder = await _fileRepository.GetFilesByFolderIdAsync(folder.Id);

            foreach(var file in filesInFolder)
            {
                file.Path = Path.Combine(folder.Path, file.Name);
                await _fileRepository.UpdateAsync(file);
            }

            foreach (var sub in folder.SubFolders)
            {
                await UpdateFilePathsRecursively(sub);
            }
        }
    }

}
