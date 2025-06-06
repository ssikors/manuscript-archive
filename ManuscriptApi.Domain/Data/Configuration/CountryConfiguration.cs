using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ManuscriptApi.DataAccess.Data.Configuration
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Countrie__3214EC077A1088B5");

            builder.HasIndex(e => e.Name).HasDatabaseName("IX_Countries_Name");

            builder.HasIndex(e => e.Name)
                  .IsUnique()
                  .HasDatabaseName("UQ__Countrie__737584F6A0D87EA9");

            builder.Property(e => e.Id).ValueGeneratedNever();

            builder.Property(e => e.Description).HasColumnType("text");

            builder.Property(e => e.IconUrl).HasMaxLength(255);

            builder.Property(e => e.Name).HasMaxLength(255);
        }
    }
}
