using MascotBusiness.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MascotBusiness.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Mascot> Mascots => Set<Mascot>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Mascot>(entity =>
        {
            entity.Property(mascot => mascot.Name)
                .HasMaxLength(100);

            entity.Property(mascot => mascot.Description)
                .HasMaxLength(1000);

            entity.Property(mascot => mascot.ImageUrl)
                .HasMaxLength(500);

            entity.Property(mascot => mascot.RentalPrice)
                .HasPrecision(18, 2);

            entity.Property(mascot => mascot.SalePrice)
                .HasPrecision(18, 2);
        });
    }
}
