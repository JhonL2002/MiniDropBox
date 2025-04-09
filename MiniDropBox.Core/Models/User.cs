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

        //Relationship with File
        public ICollection<File> Files { get; set; }

        //Relationship with Folder
        public ICollection<Folder> Folders { get; set; }

        //Relationship with UserRole
        public ICollection<UserRole> UserRoles { get; set; }
    }
}
