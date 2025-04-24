using MiniDropBox.Application.DTOs;
using MiniDropBox.Application.DTOs.Nodes;
using MiniDropBox.Application.Interfaces;
using MiniDropBox.Application.Interfaces.FileServices;
using MiniDropBox.Application.Interfaces.Trees;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;

namespace MiniDropBox.Application.Implementations
{
    public class FolderService : IFolderService
    {
        private readonly IFolderRepository _folderRepository;
        private readonly IFileRepository _fileRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ICurrentUserService _currentUser;
        private readonly IFolderTreeBuilderService _treeBuilder;

        public FolderService(IFolderRepository folderRepository, IFolderTreeBuilderService treeBuilder, IFileRepository fileRepository, ICurrentUserService currentUser, IFileStorageService fileStorageService)
        {
            _folderRepository = folderRepository;
            _treeBuilder = treeBuilder;
            _fileRepository = fileRepository;
            _currentUser = currentUser;
            _fileStorageService = fileStorageService;
        }

        public async Task<Result<FolderDTO>> CreateFolderAsync(FolderDTO folderDTO, int userId)
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

            var resultDTO = new FolderDTO
            (
                createdFolder.Name,
                createdFolder.ParentFolderId
            );

            return Result<FolderDTO>.Success(resultDTO);
        }

        public async Task<Result<string>> DeleteFolderAsync(int folderId, int userId, bool deleteContents)
        {
            var folder = await _folderRepository.GetByIdWithFilesRecursivelyAsync(folderId);
            if (folder == null || folder.UserId != userId)
            {
                return Result<string>.Failure("Folder not found or you don't have permission");
            }

            var hasSubfolders = folder.SubFolders.Any() == true;
            var hasFiles = folder.Files.Any() || folder.SubFolders.Any(sf => sf.Files.Any());

            if ((hasSubfolders || hasFiles) && !deleteContents)
            {
                return Result<string>.Failure("Folder is not empty. Confirm deletion with contents");
            }

            // Delete subfolders and files recursively
            await DeleteFolderAndContentsRecursively(folder);

            return Result<string>.Success("Folder and its contents deleted succesfully!");
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

            return Result<string>.Success($"Folder moved to: {folder.Path}");
        }

        public async Task<Result<string>> UpdateFolderAsync(UpdateFolderDTO updateFolderDTO)
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

            return Result<string>.Success($"Uploaded folder, new path: {updatedFolder!.Path} .");
        }

        // Auxiliary method to delete folder and content recursively
        private async Task DeleteFolderAndContentsRecursively(Folder folder)
        {
            // First delete all files
            foreach(var file in folder.Files)
            {
                await _fileStorageService.DeleteStreamAsync(file.Path);
                await _fileRepository.DeleteAsync(file.Id);
            }

            // Delete all subfolders
            foreach(var subFolder in folder.SubFolders)
            {
                await DeleteFolderAndContentsRecursively(subFolder);
            }

            // Finally delete the folder itself
            await _folderRepository.DeleteAsync(folder.Id);
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
