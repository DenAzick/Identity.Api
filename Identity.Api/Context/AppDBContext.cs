using Identity.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Api.Context;

public class AppDBContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {
        
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");

            entity.HasKey(key => key.Id);

            entity.Property(username => username.Username)
            .IsRequired();

            entity.HasIndex(index => index.Username)
            .IsUnique();

            entity.Property(name => name.Name)
            .IsRequired();
            
        });

    }
}
