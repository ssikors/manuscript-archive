using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.EFCoreDAL.Configuration
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Images__3214EC07068643E5");

            builder.HasIndex(e => e.ManuscriptId).HasDatabaseName("IX_Images_Manuscript");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);
            builder.Property(e => e.Title).HasMaxLength(255);
            builder.Property(e => e.Url).HasMaxLength(255);

            builder.HasOne(d => d.Manuscript)
                  .WithMany(p => p.Images)
                  .HasForeignKey(d => d.ManuscriptId)
                  .HasConstraintName("FK__Images__Manuscri__74794A92");

            builder.HasMany(d => d.Tags)
                  .WithMany(p => p.Images)
                  .UsingEntity<Dictionary<string, object>>(
                      "ImageTag",
                      r => r.HasOne<Tag>()
                            .WithMany()
                            .HasForeignKey("TagId")
                            .HasConstraintName("FK__ImageTag__TagId__719CDDE7"),
                      l => l.HasOne<Image>()
                            .WithMany()
                            .HasForeignKey("ImageId")
                            .HasConstraintName("FK__ImageTag__ImageI__70A8B9AE"),
                      j =>
                      {
                          j.HasKey("ImageId", "TagId").HasName("PK__ImageTag__A3413896F673B914");
                          j.ToTable("ImageTag");
                      });
        }
    }
}
