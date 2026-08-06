using MascotBusiness.Api.Features.Mascots;
using MascotBusiness.Api.Features.Reservations;
using MascotBusiness.Api.Features.Customers;

using Microsoft.EntityFrameworkCore;

namespace MascotBusiness.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public DbSet<Mascot> Mascots => Set<Mascot>();

    public DbSet<Customer> Customers => Set<Customer>();

    public DbSet<Reservation> Reservations => Set<Reservation>();

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

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.Property(customer => customer.FirstName)
                .HasMaxLength(100);

            entity.Property(customer => customer.LastName)
                .HasMaxLength(100);

            entity.Property(customer => customer.Phone)
                .HasMaxLength(30);

            entity.Property(customer => customer.Email)
                .HasMaxLength(100);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.Property(reservation => reservation.PublicNumber)
                .HasMaxLength(30);

            entity.Property(reservation => reservation.EventLocation)
                .HasMaxLength(300);

            entity.Property(reservation => reservation.Note)
                .HasMaxLength(1000);

            entity.HasIndex(reservation => reservation.PublicNumber)
                   .IsUnique();

              entity.Property(reservation => reservation.Status)
            .HasConversion<string>()
            .HasMaxLength(20);


             // veza sa klijentom
             entity.HasOne(reservation => reservation.Customer)
            .WithMany()
            .HasForeignKey(reservation => reservation.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

            // veza sa maskotom
            entity.HasOne(reservation => reservation.Mascot)
                .WithMany()
                .HasForeignKey(reservation => reservation.MascotId)
                .OnDelete(DeleteBehavior.Restrict);

        });
    }
}
