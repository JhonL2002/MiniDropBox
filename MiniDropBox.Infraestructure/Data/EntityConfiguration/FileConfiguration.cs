using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MiniDropBox.Infraestructure.Data.EntityConfiguration
{
    public class FileConfiguration : IEntityTypeConfiguration<Core.Models.File>
    {
        public void Configure(EntityTypeBuilder<Core.Models.File> builder)
        {
            builder.Property(f => f.Name).IsRequired();
            builder.Property(f => f.Extension).IsRequired();
            builder.Property(f => f.Path).IsRequired();
            builder.Property(f => f.UserId).IsRequired();
            builder.Property(f => f.FolderId).IsRequired();

            //Relationships
            builder.HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(f => f.Folder)
                .WithMany(f => f.Files)
                .HasForeignKey(f => f.FolderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
