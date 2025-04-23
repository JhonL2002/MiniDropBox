namespace MiniDropBox.API.ApiDTOs
{
    public class UploadFileFormDTO
    {
        public IFormFile File { get; set; } = default!;
        public int FolderId { get; set; }
    }
}
