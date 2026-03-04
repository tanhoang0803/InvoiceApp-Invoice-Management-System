using invoice_database.Models;
using Microsoft.EntityFrameworkCore;

namespace invoice_database.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<InvoiceService> InvoiceServices => Set<InvoiceService>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Soft delete global query filter
        modelBuilder.Entity<Invoice>()
            .HasQueryFilter(i => !i.IsDeleted);

        // Unique index on InvoiceNo
        modelBuilder.Entity<Invoice>()
            .HasIndex(i => i.InvoiceNo)
            .IsUnique();

        // Cascade delete services when invoice is deleted
        modelBuilder.Entity<Invoice>()
            .HasMany(i => i.Services)
            .WithOne(s => s.Invoice)
            .HasForeignKey(s => s.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        // Decimal column types
        modelBuilder.Entity<Invoice>()
            .Property(i => i.TotalAmount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<InvoiceService>()
            .Property(s => s.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<InvoiceService>()
            .Property(s => s.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}
