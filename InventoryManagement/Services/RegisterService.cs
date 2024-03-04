using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces.Repositories;
using InventoryManagement.Domain.Interfaces.Services;
using System.Globalization;

namespace InventoryManagement.Services
{
    public class RegisterService : IRegisterService
    {
        private readonly IProductRepository _productRepository;
        private readonly IBatchRepository _batchRepository;

        public RegisterService(IProductRepository productRepository, IBatchRepository batchRepository)
        {
            _productRepository = productRepository;
            _batchRepository = batchRepository;
        }

        public int RegisterNewProduct(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty.");

            // Verificar se já existe um produto com o mesmo nome
            var existingProduct = _productRepository.GetByName(name);
            if (existingProduct != null)
                throw new InvalidOperationException("A product with the same name already exists.");

            var product = new Product { Name = name, TotalProductAmount = 0 };
            _productRepository.Add(product);

            return product.Id;
        }


        public void RecordEntry(int? code, int? productId, string productionDateString, string expirationDateString, int? productAmount)
        {
            if (!ValidateCode(code))
                throw new ArgumentException("Invalid batch code.");

            var (isValidProduct, product) = ValidateProduct(productId);
            if (!isValidProduct)
                throw new ArgumentException("Product not found.");

            var (isValidProductionDate, productionDate) = ValidateProductionDate(productionDateString);
            if (!(isValidProductionDate))
                throw new ArgumentException("Invalid production date.");

            var (isValidExpirationDate, expirationDate) = ValidateExpirationDate(expirationDateString);
            if (!isValidExpirationDate)
                throw new ArgumentException("Invalid expiration date.");

            if (!ValidateProductAmount(productAmount))
                throw new ArgumentNullException("Invalid amount of products.");


            var batch = new Batch
            {
                Code = (int)code,
                ProductId = (int)productId,
                Product = product,
                ProductionDate = productionDate,
                ExpirationDate = expirationDate,
                ProductAmountBatch = (int)productAmount
            };

            //Add the batch to the repository
            _batchRepository.Add(batch);

            //Update the total amount of products in the Product repository
            product.TotalProductAmount += (int)productAmount;
            _productRepository.Update(product);

        }

        public bool ValidateCode(int? code)
        {
            if (code == null)
                throw new ArgumentNullException("The batch code cannot be null");
            if (code <= 0)
                throw new ArgumentException("The batch code cannot be less than or equal to zero.");

            return true;
        }

        public (bool, Product) ValidateProduct(int? productId)
        {
            if (productId == null)
                throw new ArgumentNullException("Product ID cannot be null.");

            if (productId <= 0)
                throw new ArgumentException("Product ID must be greater than zero.");

            var product = _productRepository.GetById(productId);
            if (product == null)
                return (false, null);

            return (true, product);
        }

        public (bool, DateOnly) ValidateProductionDate(string productionDateString)
        {
            if (string.IsNullOrEmpty(productionDateString))
                throw new ArgumentException("Production date string cannot be null or empty.");

            if (!DateOnly.TryParseExact(productionDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly productionDate))
                throw new ArgumentException("Invalid production date format. Must be in the format 'yyyy-MM-dd'.");

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            return (productionDate <= today, productionDate);
        }

        public (bool, DateOnly) ValidateExpirationDate(string expirationDateString)
        {
            if (string.IsNullOrEmpty(expirationDateString))
                throw new ArgumentException("Expiration date string cannot be null or empty.");

            if (!DateOnly.TryParseExact(expirationDateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly expirationDate))
                throw new ArgumentException("Invalid expiration date format. Must be in the format 'yyyy-MM-dd'.");

            DateOnly today = DateOnly.FromDateTime(DateTime.Today);

            return (expirationDate >= today, expirationDate);
        }

        public bool ValidateProductAmount(int? productAmount)
        {
            if (productAmount == null)
                throw new ArgumentNullException("The amount of products in the batch cannot be null.");
            if (productAmount <= 0)
                throw new ArgumentException("The amount of products in the batch cannot be less than or equal to zero.");

            return true;
        }
    }
}
