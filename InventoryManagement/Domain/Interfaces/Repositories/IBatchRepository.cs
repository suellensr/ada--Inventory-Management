using InventoryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Interfaces.Repositories
{
    public interface IBatchRepository
    {
        public void Add(Batch batch);
        public void Update(Batch batch);
        public void Delete(Batch batch);
        public void SaveChanges();

    }
}
