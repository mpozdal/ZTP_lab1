using Microsoft.EntityFrameworkCore;
using ProductApp.Domain.Entities;

namespace ProductApp.Infrastructure.Context;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }
    
    public DbSet<Product> Products { get; set; }
    public DbSet<ForbiddenPhrase> ForbiddenPhrase { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<ProductChangeHistory> ProductsHistory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Product>().Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Product>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Product>().Property(p => p.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<ForbiddenPhrase>().Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()"); 
        modelBuilder.Entity<ForbiddenPhrase>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<ForbiddenPhrase>().Property(p => p.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<Category>().Property(c => c.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<Category>().Property(p => p.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        modelBuilder.Entity<Category>().Property(p => p.UpdatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<ProductChangeHistory>().Property(p => p.Id).HasDefaultValueSql("gen_random_uuid()");
        modelBuilder.Entity<ProductChangeHistory>().Property(p => p.ChangeDate).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}