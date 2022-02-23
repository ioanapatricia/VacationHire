using Microsoft.EntityFrameworkCore;
using VacationHire.VehicleRentals.Entities;

namespace VacationHire.VehicleRentals.Persistence;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options)
           : base(options)
    {
    }

    public DbSet<Rental> Rentals { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<VehicleType> VehicleTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Rental>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<VehicleType>()
          .Property(p => p.PricePerDay)
          .HasColumnType("decimal(18,2)");
    }
}