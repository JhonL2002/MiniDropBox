namespace MiniDropBox.Core.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime LastLogin { get; set; }

        //Relationship with file
        public ICollection<File> Files { get; set; }

        //Relationship with folder
        public ICollection<Folder> Folders { get; set; }
    }
}
