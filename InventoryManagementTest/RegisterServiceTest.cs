using FluentAssertions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Interfaces.Repositories;
using InventoryManagement.Services;


namespace InventoryManagementTest
{
    public class RegisterServiceTest
    {
        private Mock<IProductRepository> _mockProductRepository;
        private Mock<IBatchRepository> _mockBatchRepository;
        private RegisterService _sut;


        public RegisterServiceTest()
        {
            // Configuração global dos mocks e da classe RegisterService
            _mockProductRepository = new Mock<IProductRepository>();
            _mockBatchRepository = new Mock<IBatchRepository>();
            _sut = new RegisterService(_mockProductRepository.Object, _mockBatchRepository.Object);
        }

        //RegisterNewProduct
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void RegisterNewProduct_WithNullOrEmptyName_ShouldThrowArgumentException(string productName)
        {
            // Act
            var result = () => _sut.RegisterNewProduct(productName);

            //Assert
            result.Should().ThrowExactly<ArgumentException>("Name cannot be null or empty.");
        }

        [Fact]
        public void RegisterNewProduct_WithExistingProductName_ShouldThrowInvalidOperationException()
        {
            // Arrange
            string existingProductName = "Coca-Cola 5 litros";
            _mockProductRepository.Setup(r => r.GetByName(existingProductName)).Returns(new Product { Name = existingProductName });

            // Act
            var result = () => _sut.RegisterNewProduct(existingProductName);

            //Assert
            result.Should().Throw<InvalidOperationException>().WithMessage("A product with the same name already exists.");
        }

        [Fact]
        public void RegisterNewProduct_AddsNewProductToRepository()
        {
            // Arrange
            string productName = "Coca-Cola 5 litros";
            int expectedProductId = 0010;

            _mockProductRepository
                .Setup(r => r.Add(It.IsAny<Product>()))
                .Callback<Product>(p => p.Id = expectedProductId);

            // Act
            int productId = _sut.RegisterNewProduct(productName);

            // Assert
            _mockProductRepository.Verify(r => r.Add(It.IsAny<Product>()), Times.Once);
            productId.Should().Be(expectedProductId);
            productName.Should().Be(productName);
        }
    

        //RecordEntry
        [Fact]
        public void RecordEntry_AddsBatchToRepositorySuccessfully()
        {
            // Arrange
            int code = 102030;
            int productId = 1;
            string productionDate = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
            string expirationDate = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd");
            int productAmount = 10;

            var product = new Product { Id = productId, TotalProductAmount = 5 };
            _mockProductRepository.Setup(r => r.GetById(productId)).Returns(product);

            // Act
            _sut.RecordEntry(code, productId, productionDate, expirationDate, productAmount);

            // Assert
            _mockBatchRepository.Verify(r => r.Add(It.IsAny<Batch>()), Times.Once);
        }

        [Fact]
        public void RecordEntry_IncrementsProductTotalAmount()
        {
            // Arrange
            int code = 102030;
            int productId = 1;
            string productionDate = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd");
            string expirationDate = DateTime.Today.AddDays(5).ToString("yyyy-MM-dd");
            int productAmount = 10;

            var product = new Product { Id = productId, TotalProductAmount = 5 };
            _mockProductRepository.Setup(r => r.GetById(productId)).Returns(product);

            // Act
            _sut.RecordEntry(code, productId, productionDate, expirationDate, productAmount);

            // Assert
            product.TotalProductAmount.Should().Be(15); // Total product amount should be incremented by productAmount
        }


        //ValidadeCode
        [Fact]
        public void ValidateCode_WithNullInput_ShouldThrowArgumentNullException()
        {
            // Arrange
            int? code = null;

            // Act
            var result = () => _sut.ValidateCode(code);

            // Assert
            result.Should().ThrowExactly<ArgumentNullException>("The batch code cannot be null");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidateCode_WithNegativeOrZeroInput_ShouldThrowException(int code)
        {
            // Act
            var result = () => _sut.ValidateCode(code);

            // Assert
            result.Should().ThrowExactly<ArgumentException>("The batch code cannot be less than or equal to zero.");
        }

        [Fact]
        public void ValidateCode_WithPositiveInput_ShouldNotThrowException()
        {
            // Arrange
            int code = 101010;

            // Act
            var result = () => _sut.ValidateCode(code);

            // Assert
            result.Should().NotThrow();
        }


        //ValidateProduct
        [Fact]
        public void ValidateProduct_WithNullInput_ShouldThrowArgumentNullException()
        {
            // Arrange
            int? productId = null;

            // Act
            var result = () => _sut.ValidateProductAmount(productId);

            // Assert
            result.Should().ThrowExactly<ArgumentNullException>("Product ID cannot be null.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidateProduct_WithIdZeroOrNegative_ShouldThrowArgumentException(int productId)
        {

            // Act
            var result = () => _sut.ValidateProduct(productId);

            // Assert
            result.Should().ThrowExactly<ArgumentException>("Product ID must be greater than zero.");
        }

        [Fact]
        public void ValidateProduct_WithNonExistentId_ShouldReturnFalseAndNullProduct()
        {
            // Arrange
            int productId = 100;
            _mockProductRepository.Setup(r => r.GetById(productId)).Returns((Product)null);

            // Act
            var (productIsValid, product) = _sut.ValidateProduct(productId);

            // Assert
            productIsValid.Should().BeFalse();
            product.Should().BeNull();
        }


        [Fact]
        public void ValidateProduct_WithExistentId_ShouldReturnTrueAndValidProduct()
        {
            // Arrange
            int productId = 1;
            var expectedProduct = new Product { Id = productId, Name = "Product 1" };
            _mockProductRepository.Setup(r => r.GetById(productId)).Returns(expectedProduct);

            // Act
            var (productIsValid, product) = _sut.ValidateProduct(productId);

            // Assert
            productIsValid.Should().BeTrue();
            product.Should().BeEquivalentTo(expectedProduct);
        }


        //ValidateProductionDate
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ValidateProductionDate_NullOrEmptyString_ThrowsArgumentException(string productionDateString)
        {
            // Act
            var result = () => _sut.ValidateProductionDate(productionDateString);

            // Assert
            result.Should().ThrowExactly<ArgumentException>("Production date string cannot be null or empty.");
        }

        [Theory]
        [InlineData("2024/03/03")]
        [InlineData("03/03/2024")]
        [InlineData("invalid_date")]
        public void ValidateProductionDate_InvalidDateFormat_ThrowsArgumentException(string productionDateString)
        {
            // Arrange & Act & Assert
            _sut.Invoking(rs => rs.ValidateProductionDate(productionDateString))
                .Should().Throw<ArgumentException>().WithMessage("Invalid production date format. Must be in the format 'yyyy-MM-dd'.");
        }

        [Fact]
        public void ValidateProductionDate_IsNOTValidDateAndReturnsFalse_IfDateIsAfterToday()
        {
            // Arrange
            string expirationDateString = DateTime.Today.AddDays(2).ToString("yyyy-MM-dd");
            DateOnly expirationDate = DateOnly.Parse(expirationDateString);

            // Act
            var (result, validatedDate) = _sut.ValidateProductionDate(expirationDateString);

            // Assert
            result.Should().BeFalse();
            validatedDate.Should().Be(expirationDate);
        }

        [Theory]
        [MemberData(nameof(GetProductionDates))]
        public void ValidateProductionDate_IsValidDateAndReturnsTrue_IfDateIsTodayOrBefore(string productionDateString)
        {
            // Arrange
            DateOnly productionDate = DateOnly.Parse(productionDateString);

            // Act
            var (result, validatedDate) = _sut.ValidateProductionDate(productionDateString);

            // Assert
            result.Should().BeTrue();
            validatedDate.Should().Be(productionDate);
        }

        public static IEnumerable<object[]> GetProductionDates()
        {
            yield return new object[] { DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd") };// production date is in the past
            yield return new object[] { "2024-03-02" }; // production date is in the past
            yield return new object[] { DateTime.Today.ToString("yyyy-MM-dd") }; // production date is today
            //yield return new object[] { DateTime.Today.AddDays(1).ToString("yyyy-MM-dd") }; // production date is in the future
        }


        //ValidateExpirationDate
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void ValidateExpirationDate_NullOrEmptyString_ThrowsArgumentException(string expirationDateString)
        {
            // Act
            var result = () => _sut.ValidateExpirationDate(expirationDateString);

            // Assert
            result.Should().ThrowExactly<ArgumentException>("Expiration date string cannot be null or empty.");
        }

        [Theory]
        [InlineData("2024/03/03")]
        [InlineData("03/03/2024")]
        [InlineData("invalid_date")]
        public void ValidateExpirationDate_InvalidDateFormat_ThrowsArgumentException(string expirationDateString)
        {
            // Arrange & Act & Assert
            _sut.Invoking(rs => rs.ValidateExpirationDate(expirationDateString))
                .Should().Throw<ArgumentException>().WithMessage("Invalid expiration date format. Must be in the format 'yyyy-MM-dd'.");
        }

        [Theory]
        [InlineData("2024-03-02")]
        public void ValidateExpirationDate_IsNOTValidDateAndReturnsFalse_IfDateIsPastToday(string expirationDateString)
        {
            // Arrange
            DateOnly expirationDate = DateOnly.Parse(expirationDateString);

            // Act
            var (result, validatedDate) = _sut.ValidateExpirationDate(expirationDateString);

            // Assert
            result.Should().BeFalse();
            validatedDate.Should().Be(expirationDate);
        }

        [Theory]
        [MemberData(nameof(GetExpirationDates))]
        public void ValidateExpirationDate_IsValidDateAndReturnsTrue_IfDateIsTodayOrAfter(string expirationDateString)
        {
            // Arrange
            DateOnly expirationDate = DateOnly.Parse(expirationDateString);

            // Act
            var (result, validatedDate) = _sut.ValidateExpirationDate(expirationDateString);

            // Assert
            result.Should().BeTrue();
            validatedDate.Should().Be(expirationDate);
        }

        public static IEnumerable<object[]> GetExpirationDates()
        {
            //yield return new object[] { "2024-03-02" }; // expiration date is in the past
            yield return new object[] { DateTime.Today.ToString("yyyy-MM-dd") }; // expiration date is today
            yield return new object[] { DateTime.Today.AddDays(1).ToString("yyyy-MM-dd") }; // expiration date is in the future
        }


        //ValidataProductAmount
        [Fact]
        public void ValidateProductAmount_WithNullInput_ShouldThrowArgumentNullException()
        {
            // Arrange
            int? productAmount = null;

            // Act
            var result = () => _sut.ValidateProductAmount(productAmount);

            // Assert
            result.Should().ThrowExactly<ArgumentNullException>("The amount of products in the batch cannot be null.");
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void ValidateProductAmount_WithNegativeOrZeroAmount_ShouldThrowException(int productAmount)
        {
            // Act
            var result = () => _sut.ValidateProductAmount(productAmount);

            // Assert
            result.Should().ThrowExactly<ArgumentException>("The amount of products in the batch cannot be less than or equal to zero.");
        }

        [Fact]
        public void ValidateProductAmount_WithPositiveInput_ShouldNotThrowException()
        {
            // Arrange
            int productAmount = 10;

            // Act
            var result = () => _sut.ValidateProductAmount(productAmount);

            // Assert
            result.Should().NotThrow();
        }
    }
}