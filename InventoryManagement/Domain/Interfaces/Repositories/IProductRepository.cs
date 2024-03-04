using InventoryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        public void Add(Product product);
        public void Update(Product product);
        //public void DeleteProduct(Product product);
        public Product GetById(int? id);
        public Product GetByName(string name);
        public void SaveChanges();

    }
}
