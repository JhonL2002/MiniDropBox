using Microsoft.EntityFrameworkCore;
using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using MiniDropBox.Infraestructure.Data;
using File = MiniDropBox.Core.Models.File;


namespace MiniDropBox.Infraestructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly AppDbContext _context;

        public FileRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<File> AddAsync(File file)
        {
            _context.Files.Add(file);
            return Task.FromResult(file);
        }

        public async Task<File?> DeleteAsync(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            if (file != null)
            {
                _context.Files.Remove(file);
            }

            return file;
        }

        public async Task<IEnumerable<File>> GetAllAsync()
        {
            return await _context.Files.ToListAsync();
        }

        public async Task<File?> GetByIdAsync(int fileId)
        {
            var file = await _context.Files.FindAsync(fileId);
            return file;
        }

        public async Task<IEnumerable<File>> GetFilesByFolderIdAsync(int folderId)
        {
            var files = await _context.Files
                .Where(f => f.FolderId == folderId)
                .ToListAsync();

            return files;
        }

        public async Task<File?> UpdateAsync(File file)
        {
            var existingFile = await _context.Files.FindAsync(file.Id);
            if (existingFile != null)
            {
                existingFile.Name = file.Name;
            }
            return null;
        }
    }
}
