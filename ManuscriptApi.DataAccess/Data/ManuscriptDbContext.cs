
using Microsoft.EntityFrameworkCore;

public  class ManuscriptDbContext : DbContext
{
    public ManuscriptDbContext()
    {
    }

    public ManuscriptDbContext(DbContextOptions<ManuscriptDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<Image> Images { get; set; }

    public virtual DbSet<Location> Locations { get; set; }

    public virtual DbSet<Manuscript> Manuscripts { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Countrie__3214EC077A1088B5");

            entity.HasIndex(e => e.Name, "IX_Countries_Name");

            entity.HasIndex(e => e.Name, "UQ__Countrie__737584F6A0D87EA9").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IconUrl).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Images__3214EC07068643E5");

            entity.HasIndex(e => e.ManuscriptId, "IX_Images_Manuscript");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.Url).HasMaxLength(255);

            entity.HasOne(d => d.Manuscript).WithMany(p => p.Images)
                .HasForeignKey(d => d.ManuscriptId)
                .HasConstraintName("FK__Images__Manuscri__74794A92");

            entity.HasMany(d => d.Tags).WithMany(p => p.Images)
                .UsingEntity<Dictionary<string, object>>(
                    "ImageTag",
                    r => r.HasOne<Tag>().WithMany()
                        .HasForeignKey("TagId")
                        .HasConstraintName("FK__ImageTag__TagId__719CDDE7"),
                    l => l.HasOne<Image>().WithMany()
                        .HasForeignKey("ImageId")
                        .HasConstraintName("FK__ImageTag__ImageI__70A8B9AE"),
                    j =>
                    {
                        j.HasKey("ImageId", "TagId").HasName("PK__ImageTag__A3413896F673B914");
                        j.ToTable("ImageTag");
                    });
        });

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Location__3214EC0791E58024");

            entity.HasIndex(e => e.CountryId, "IX_Locations_Country");

            entity.HasIndex(e => e.Name, "IX_Locations_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Country).WithMany(p => p.Locations)
                .HasForeignKey(d => d.CountryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Locations__Count__73852659");
        });

        modelBuilder.Entity<Manuscript>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Manuscri__3214EC07001B68D8");

            entity.HasIndex(e => e.AuthorId, "IX_Manuscripts_Author");

            entity.HasIndex(e => e.LocationId, "IX_Manuscripts_Location");

            entity.HasIndex(e => e.YearWrittenEnd, "IX_Manuscripts_YearEnd");

            entity.HasIndex(e => new { e.YearWrittenStart, e.YearWrittenEnd }, "IX_Manuscripts_YearRange");

            entity.HasIndex(e => e.YearWrittenStart, "IX_Manuscripts_YearStart");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.SourceUrl).HasMaxLength(255);
            entity.Property(e => e.Title).HasMaxLength(255);

            entity.HasOne(d => d.Author).WithMany(p => p.Manuscripts)
                .HasForeignKey(d => d.AuthorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserManuscripts");

            entity.HasOne(d => d.Location).WithMany(p => p.Manuscripts)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Manuscrip__Locat__72910220");

            entity.HasMany(d => d.Tags).WithMany(p => p.Manuscripts)
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
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Tags__3214EC0762ADDE44");

            entity.HasIndex(e => e.Name, "IX_Tags_Name");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC070DE18CDA");

            entity.HasIndex(e => e.Email, "IX_User_Email");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E470BC64CE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105342AD655AF").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.Username).HasMaxLength(255);
        });
    }
}