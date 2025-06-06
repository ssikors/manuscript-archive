
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ManuscriptApi.EFCoreDAL.Configuration
{
    public class LocationConfiguration : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> builder)
        {
            builder .HasKey(e => e.Id).HasName("PK__Location__3214EC0791E58024");

            builder.HasIndex(e => e.CountryId).HasDatabaseName("IX_Locations_Country");
            builder.HasIndex(e => e.Name).HasDatabaseName("IX_Locations_Name");

            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.Property(e => e.Name).HasMaxLength(255);

            builder.HasOne(d => d.Country)
                  .WithMany(p => p.Locations)
                  .HasForeignKey(d => d.CountryId)
                  .OnDelete(DeleteBehavior.ClientSetNull)
                  .HasConstraintName("FK__Locations__Count__73852659");
        }
    }
}
