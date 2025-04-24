using Microsoft.EntityFrameworkCore;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.Data;

namespace MiniDropBox.Infraestructure.Repositories
{
    public class FolderRepository : IFolderRepository
    {
        private readonly AppDbContext _context;

        public FolderRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Folder> AddAsync(Folder folder)
        {
            _context.Folders.Add(folder);
            return Task.FromResult(folder);
        }

        public async Task<Folder?> DeleteAsync(int folderId)
        {
            var folder = await _context.Folders.FindAsync(folderId);
            if (folder != null)
            {
                _context.Folders.Remove(folder);
            }

            return folder;
        }

        public async Task<Folder?> GetByIdAsync(int folderId)
        {
            var folder = await _context.Folders
                .Include(f => f.ParentFolder)
                .FirstOrDefaultAsync(f => f.Id == folderId);
            return folder;
        }

        public async Task<Folder?> GetByIdWithFilesRecursivelyAsync(int folderId)
        {
            var root = await _context.Folders
                .Include(f => f.Files)
                .Include(f => f.SubFolders)
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (root != null)
            {
                await LoadSubfoldersAndFilesRecursively(root);
            }

            return root;
        }

        public async Task<Folder?> GetByIdWithSubfoldersRecursivelyAsync(int folderId)
        {
            var root = await _context.Folders
                .FirstOrDefaultAsync(f => f.Id == folderId);

            if (root != null)
            {
                await LoadSubfoldersRecursively(root);
            }

            return root;
        }

        public Task<Folder?> GetByNameAsync(string name)
        {
            var folder = _context.Folders.FirstOrDefault(f => f.Name == name);
            return Task.FromResult(folder);
        }

        public async Task<List<Folder>> GetFoldersByUserIdAsync(int userId)
        {
            return await _context.Folders
                .Where(f => f.UserId == userId)
                .ToListAsync();
        }

        public Task<Folder?> UpdateAsync(Folder folder)
        {
            var existingFolder = _context.Folders.FirstOrDefault(f => f.Id == folder.Id);
            if (existingFolder != null)
            {
                existingFolder.Name = folder.Name;
                return Task.FromResult<Folder?>(existingFolder);
            }

            return Task.FromResult<Folder?>(null);
        }

        // Load subfolders recursively
        private async Task LoadSubfoldersRecursively(Folder folder)
        {
            await _context.Entry(folder)
                .Collection(f => f.SubFolders)
                .LoadAsync();

            foreach (var sub in folder.SubFolders)
            {
                await LoadSubfoldersRecursively(sub);
            }
        }

        // Load files recursively
        private async Task LoadSubfoldersAndFilesRecursively(Folder folder)
        {
            await _context.Entry(folder)
                .Collection(f => f.SubFolders)
                .LoadAsync();

            await _context.Entry(folder)
                .Collection(f => f.Files)
                .LoadAsync();

            foreach(var sub in folder.SubFolders)
            {
                await LoadSubfoldersAndFilesRecursively(sub);
            }
        }

    }
}
