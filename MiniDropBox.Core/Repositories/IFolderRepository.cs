using MiniDropBox.Core.Models;

namespace MiniDropBox.Core.Repositories
{
    public interface IFolderRepository
    {
        Task<Folder> AddAsync(Folder folder);
        Task<Folder?> GetByIdAsync(int folderId);
        Task<Folder?> GetByNameAsync(string name);
        Task<IEnumerable<Folder>> GetAllAsync();
        Task<Folder?> UpdateAsync(Folder folder);
        Task<Folder?> DeleteAsync(int folderId);
    }
}
