using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InventoryManagement.Domain.Entities
{
    public interface IBatch
    {
        public int Id { get; }
        public int Code { get; }
        public int ProductId { get; }
        public Product Product { get; }
        public DateOnly ProductionDate { get; }
        public DateOnly ExpirationDate { get; }
        public int ProductAmountBatch { get; }
    }
    
    
    public class Batch : IBatch
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
        public DateOnly ProductionDate { get; set; }
        public DateOnly ExpirationDate { get; set; }
        public int ProductAmountBatch { get; set; }
    }
}
