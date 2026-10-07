using Kt5RepositoryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace Kt5RepositoryApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().Property(category => category.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(product => product.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Product>().Property(product => product.Description).HasMaxLength(500);
        modelBuilder.Entity<Product>().Property(product => product.Price).HasPrecision(12, 2);
        modelBuilder.Entity<Product>()
            .HasOne(product => product.Category)
            .WithMany()
            .HasForeignKey(product => product.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Электроника" },
            new Category { Id = 2, Name = "Канцелярия" });

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Клавиатура", Price = 2500, CategoryId = 1 },
            new Product { Id = 2, Name = "Мышь", Price = 1200, CategoryId = 1 },
            new Product { Id = 3, Name = "Тетрадь", Price = 80, CategoryId = 2 });
    }
}
