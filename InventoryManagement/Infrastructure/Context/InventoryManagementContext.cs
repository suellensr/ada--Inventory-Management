using InventoryManagement.Domain.Entities;
using InventoryManagement.Infrastructure.EntityFramework.Configuration;
using Microsoft.EntityFrameworkCore;


namespace InventoryManagement.Infrastructure
{
    public class InventoryManagementContext : DbContext
    {
        public DbSet<Batch> Batches { get; set;}
        public DbSet<Product> Products { get; set;}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=(localdb\\MSSQLLocalDB;Initial Catalog=InventoryManagement" +
                ";Integrated Security=True;Connected Timeout=30;Encrypt=False;Trust Server Certificate=False" +
                ";Application Intent=ReadWrite;Multi Subnet Failover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BatchConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
        }

    }
}
