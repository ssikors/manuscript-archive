
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.EFCoreDAL.Configuration
{
    public class ManuscriptConfiguration : IEntityTypeConfiguration<Manuscript>
    {
        public void Configure(EntityTypeBuilder<Manuscript> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Manuscri__3214EC07001B68D8");

            builder.HasIndex(e => e.AuthorId).HasDatabaseName("IX_Manuscripts_Author");
            builder.HasIndex(e => e.LocationId).HasDatabaseName("IX_Manuscripts_Location");
            builder.HasIndex(e => e.YearWrittenEnd).HasDatabaseName("IX_Manuscripts_YearEnd");
            builder.HasIndex(e => new { e.YearWrittenStart, e.YearWrittenEnd }).HasDatabaseName("IX_Manuscripts_YearRange");
            builder.HasIndex(e => e.YearWrittenStart).HasDatabaseName("IX_Manuscripts_YearStart");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.CreatedAt).HasColumnType("datetime");
            builder.Property(e => e.Description).HasColumnType("text");
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);
            builder.Property(e => e.SourceUrl).HasMaxLength(255);
            builder.Property(e => e.Title).HasMaxLength(255);

            builder.HasOne(d => d.Author)
                  .WithMany(p => p.Manuscripts)
                  .HasForeignKey(d => d.AuthorId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("UserManuscripts");

            builder.HasOne(d => d.Location)
                  .WithMany(p => p.Manuscripts)
                  .HasForeignKey(d => d.LocationId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Manuscrip__Locat__72910220");

            builder.HasMany(d => d.Tags)
                  .WithMany(p => p.Manuscripts)
                  .UsingEntity<Dictionary<string, object>>(
                      "ManuscriptTag",
                      r => r.HasOne<Tag>().WithMany()
                            .HasForeignKey("TagId")
                            .HasConstraintName("FK__Manuscrip__TagId__6FB49575"),
                      l => l.HasOne<Manuscript>().WithMany()
                            .HasForeignKey("ManuscriptId")
                            .HasConstraintName("FK__Manuscrip__Manus__6EC0713C"),
                      j =>
                      {
                          j.HasKey("ManuscriptId", "TagId").HasName("PK__Manuscri__C066713F04B16665");
                          j.ToTable("ManuscriptTag");
                      });
        }
    }
}
