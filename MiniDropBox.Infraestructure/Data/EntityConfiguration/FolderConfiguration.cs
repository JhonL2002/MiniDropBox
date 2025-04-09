using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Infraestructure.Data.EntityConfiguration
{
    public class FolderConfiguration : IEntityTypeConfiguration<Folder>
    {
        public void Configure(EntityTypeBuilder<Folder> builder)
        {
            builder.Property(f => f.Name).IsRequired();
            builder.Property(f => f.UserId).IsRequired();

            //Relationships
            builder.HasOne(f => f.ParentFolder)
                .WithMany(f => f.SubFolders)
                .HasForeignKey(f => f.ParentFolderId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
