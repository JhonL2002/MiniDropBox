using MiniDropBox.Application.Interfaces.FileServices;

namespace MiniDropBox.Application.DTOs
{
    public class UploadFileDTO<T> where T : IFileUpload
    {
        public T File { get; }
        public int FolderId { get; }
        public string FolderPath { get; }

        public UploadFileDTO(T file, int folderId, string folderPath)
        {
            File = file;
            FolderId = folderId;
            FolderPath = folderPath;
        }
    }
}
