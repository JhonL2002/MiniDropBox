using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MiniDropBox.Core.Models;

namespace MiniDropBox.Infraestructure.Data.EntityConfiguration
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Description).IsRequired();
        }
    }
}
