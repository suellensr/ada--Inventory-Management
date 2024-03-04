using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Infrastructure.Repositories
{
    public class BatchRepository : IBatchRepository
    {
        private readonly InventoryManagementContext _inventoryManagementContext;

        public BatchRepository(InventoryManagementContext inventoryManagementContext)
        {
            _inventoryManagementContext = inventoryManagementContext;
        }
        public void Add(Batch batch)
        {
            if (batch == null)
                throw new ArgumentNullException("Batch cannot be null.");

            _inventoryManagementContext.Batches.Add(batch);
            SaveChanges();
        }

        public void Delete(Batch batch)
        {
            throw new NotImplementedException();
        }
                public void SaveChanges()
        {
            _inventoryManagementContext.SaveChanges();
        }

        public void Update(Batch batch)
        {
            throw new NotImplementedException();
        }
    }
}
