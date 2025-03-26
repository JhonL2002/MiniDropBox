using MiniDropBox.Core.Models;
using MiniDropBox.Core.Repositories;
using File = MiniDropBox.Core.Models.File;


namespace MiniDropBox.Infraestructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly List<File> _files = new();

        public Task<File> AddAsync(File file)
        {
            file.Id = _files.Count + 1;
            _files.Add(file);
            return Task.FromResult(file);
        }

        public Task<File?> DeleteAsync(int fileId)
        {
            var file = _files.FirstOrDefault(f => f.Id == fileId);
            if (file != null)
            {
                _files.Remove(file);
                return Task.FromResult<File?>(file);
            }

            return Task.FromResult<File?>(null);
        }

        public Task<IEnumerable<File>> GetAllAsync()
        {
            return Task.FromResult(_files.AsEnumerable());
        }

        public Task<File?> GetByIdAsync(int fileId)
        {
            var file = _files.FirstOrDefault(f => f.Id == fileId);
            return Task.FromResult(file);
        }

        public Task<File?> UpdateAsync(File file)
        {
            var existingFile = _files.FirstOrDefault(f => f.Id == file.Id);
            if (existingFile != null)
            {
                existingFile.Name = file.Name;
                return Task.FromResult<File?>(existingFile);
            }
            return Task.FromResult<File?>(null);
        }
    }
}
