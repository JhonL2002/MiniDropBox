using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using File = MiniDropBox.Core.Models.File;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly List<Folder> _folders = new(); 

        public Task<Folder> AddAsync(Folder folder)
        {
            _folders.Add(folder);
            return Task.FromResult(folder);
        }

        public Task<Folder?> DeleteAsync(int folderId)
        {
            var folder = _folders.FirstOrDefault(f => f.Id == folderId);
            if (folder != null)
            {
                _folders.Remove(folder);
                return Task.FromResult<Folder?>(folder);
            }

            return Task.FromResult<Folder?>(null);
        }

        public Task<IEnumerable<Folder>> GetAllAsync()
        {
            return Task.FromResult(_folders.AsEnumerable());
        }

        public Task<Folder?> GetByIdAsync(int folderId)
        {
            var folder = _folders
                .Where(f => f.Id == folderId)
                .Select(f => new Folder
                {
                    Id = f.Id,
                    ParentFolderId = f.ParentFolderId,
                    ParentFolder = _folders.FirstOrDefault(p => p.Id == f.ParentFolderId),
                })
                .FirstOrDefault();
            return Task.FromResult(folder);
        }

        public Task<Folder?> GetByNameAsync(string name)
        {
            var folder = _folders.FirstOrDefault(f => f.Name == name);
            return Task.FromResult(folder);
        }

        public Task<Folder?> UpdateAsync(Folder folder)
        {
            var existingFolder = _folders.FirstOrDefault(f => f.Id == folder.Id);
            if (existingFolder != null)
            {
                existingFolder.Name = folder.Name;
                return Task.FromResult<Folder?>(existingFolder);
            }

            return Task.FromResult<Folder?>(null);
        }
    }
}
