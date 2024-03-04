using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InventoryManagement.Domain.Entities
{
    public interface IProduct
    {
        public int Id { get; }

        public string Name { get; }

        public List<Batch> Batches { get; }

        public int TotalProductAmount { get; }
    }


    public class Product : IProduct
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public List<Batch> Batches { get; set; }

        public int TotalProductAmount { get; set; }

        public Product()
        {
            Batches = new List<Batch>();
        }
    }
}


