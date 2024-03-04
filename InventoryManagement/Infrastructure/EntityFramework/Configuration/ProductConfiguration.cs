using InventoryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;



namespace InventoryManagement.Infrastructure.EntityFramework.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(b => b.Name).IsRequired();
            builder.Property(b => b.TotalProductAmount).IsRequired();
        }
    }
}
