using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryManagementContext _inventoryManagementContext;

        public ProductRepository(InventoryManagementContext inventoryManagementContext)
        {
            _inventoryManagementContext = inventoryManagementContext;
        }

        public void Add(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product cannot be null.");

            _inventoryManagementContext.Products.Add(product);
            SaveChanges();
        }

        //public void DeleteProduct(int id)
        //{
        //    var product = GetProductById(id);
            
        //    if (product.TotalProductAmount == 0)
        //    {
        //        _inventoryManagementContext.Products.Remove(GetProductById(id));
        //        SaveChanges();
        //    }
            
        //}

        public Product GetById(int? id)
        {
            var product = _inventoryManagementContext.Products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                throw new ArgumentException("Product not found.");

            return product;
        }

        public Product GetByName(string name)
        {
            var product = _inventoryManagementContext.Products.FirstOrDefault(p => p.Name == name);

            if (product == null)
                throw new ArgumentException("Product not found.");

            return product;
        }


        public void SaveChanges()
        {
            _inventoryManagementContext.SaveChanges();
        }

        public void Update(Product product)
        {
            if (product == null)
                throw new ArgumentNullException("Product cannot be null.");

            _inventoryManagementContext.Update(product);
            SaveChanges();
        }
    }
}
