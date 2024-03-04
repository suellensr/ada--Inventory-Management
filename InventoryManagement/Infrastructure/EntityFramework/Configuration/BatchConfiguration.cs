using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.Infrastructure.EntityFramework.Configuration
{
    public class BatchConfiguration : IEntityTypeConfiguration<Batch>
    {
        public void Configure(EntityTypeBuilder<Batch> builder)
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).ValueGeneratedOnAdd().UseIdentityColumn();
            builder.Property(b => b.Code).IsRequired();
            builder.Property(b => b.ProductionDate).IsRequired();
            builder.Property(b => b.ExpirationDate).IsRequired();
            builder.Property(b => b.ProductAmountBatch).IsRequired();

            builder.HasOne(b => b.Product).WithMany(p => p.Batches).HasForeignKey(b => b.ProductId);
        }
    }
}
