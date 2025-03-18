namespace MiniDropBox.Core.Models
{
    public class File
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Size { get; set; }
        public string Extension { get; set; }
        public string Path { get; set; }
        public int UserId { get; set; }
        public int FolderId { get; set; }
        public DateTime CreatedAt { get; set; }

        //Relationship with user
        public User User { get; set; }

        //Relationship with folder
        public Folder Folder { get; set; }
    }
}
