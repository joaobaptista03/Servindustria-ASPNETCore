using Servindustria.Models;
using Microsoft.EntityFrameworkCore;

namespace Servindustria.Data;

public class ServindustriaDBContext : DbContext {
    public ServindustriaDBContext(DbContextOptions<ServindustriaDBContext> options) : base(options) {}
    
    public DbSet<User> Users { get; set; }
    public DbSet<AdminCallRequest> AdminCallRequests { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<TechnicalTable> TechnicalTables { get; set; }
    public DbSet<ProductCategory> ProductCategories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<ProductCategory>()
            .HasMany(pc => pc.Products)
            .WithOne(p => p.Category)
            .HasForeignKey(p => p.CategoryId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}