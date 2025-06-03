
using System.Reflection.Metadata;
using ManuscriptApi.DataAccess.Data.Configuration;
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
        new CountryConfiguration().Configure(modelBuilder.Entity<Country>());

        new ImageConfiguration().Configure(modelBuilder.Entity<Image>());

        new LocationConfiguration().Configure(modelBuilder.Entity<Location>());

        new ManuscriptConfiguration().Configure(modelBuilder.Entity<Manuscript>());

        new TagConfiguration().Configure(modelBuilder.Entity<Tag>());

        new UserConfiguration().Configure(modelBuilder.Entity<User>());
    }
}