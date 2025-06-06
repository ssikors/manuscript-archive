
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManuscriptApi.EFCoreDAL.Configuration
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Users__3214EC070DE18CDA");

            builder.HasIndex(e => e.Email).HasDatabaseName("IX_User_Email");

            builder.HasIndex(e => e.Username)
                  .IsUnique()
                  .HasDatabaseName("UQ__Users__536C85E470BC64CE");

            builder.HasIndex(e => e.Email)
                  .IsUnique()
                  .HasDatabaseName("UQ__Users__A9D105342AD655AF");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);
            builder.Property(e => e.Username).HasMaxLength(255);
        }
    }
}
