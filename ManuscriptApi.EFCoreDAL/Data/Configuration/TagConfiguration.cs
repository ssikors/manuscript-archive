
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.EFCoreDAL.Configuration
{
    public class TagConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Tags__3214EC0762ADDE44");

            builder.HasIndex(e => e.Name).HasDatabaseName("IX_Tags_Name");

            builder .Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Description).HasColumnType("text");
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);
            builder.Property(e => e.Name).HasMaxLength(255);
        }
    }
}
