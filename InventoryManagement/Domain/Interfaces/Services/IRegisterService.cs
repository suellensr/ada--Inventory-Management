using InventoryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Domain.Interfaces.Services
{
    public interface IRegisterService
    {
        public int RegisterNewProduct(string Name);
        public void RecordEntry(int? code, int? productId, string productionDateString, string expirationDateString, int? productAmount);
        public bool ValidateCode(int? code);
        public (bool, Product) ValidateProduct(int? idProduct);
        public (bool, DateOnly) ValidateProductionDate(string productionDateString);
        public (bool, DateOnly) ValidateExpirationDate(string expirationDateString);
        public bool ValidateProductAmount(int? productAmount);
    }

    



}
