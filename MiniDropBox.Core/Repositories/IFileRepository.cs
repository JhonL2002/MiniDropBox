using File = MiniDropBox.Core.Models.File;

namespace MiniDropBox.Core.Repositories
{
    public interface IFileRepository
    {
        Task<File> AddAsync(File file);
        Task<File?> GetByIdAsync(int fileId);
        Task<IEnumerable<File>> GetAllAsync();
        Task<File?> UpdateAsync(File file);
        Task<File?> DeleteAsync(int fileId);
    }
}
