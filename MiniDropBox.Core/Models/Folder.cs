namespace MiniDropBox.Core.Models
{
    public class Folder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentFolderId { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        //Relationship with file
        public ICollection<File> Files { get; set; } = new List<File>();

        //Relationship with folders
        public ICollection<Folder> SubFolders { get; set; } = new List<Folder>();

        //Relationship with parent folder
        public Folder? ParentFolder { get; set; }
    }
}
