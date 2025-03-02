using InventoryBAL.Implementation;
using InventoryDTO;
using InventoryRepository.Implementation;
using InventoryRepository.Interface;
using InventoryUtility;
using InventoryUtility.Interface;
using LISCareDataAccess.IInventoryDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace InventoryAndManagementSystemTest
{
    [TestFixture]
    public class ProductBALTests
    {

        private Mock<IProductRepository> mockProductRepo;
        private Mock<IProductLoggers> mockLogger;
        private ProductBAL productBAL;
        private ProductRepository productRepository;
        private InventoryDbContext invDbContext;
        private IServiceProvider serviceProvider;

        [SetUp]
        public void Setup()
        {
            mockProductRepo = new Mock<IProductRepository>();
            mockLogger = new Mock<IProductLoggers>();
            invDbContext = new InventoryDbContext();

            // Setting up mock configuration
            var inMemorySettings = new Dictionary<string, string>
             {
                {ConstantResources.ConnectionStrings, ConstantResources.DataSource}
             };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Set up services and DbContext in the test environment
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString(ConstantResources.InventoryDbConnection)));

            serviceProvider = services.BuildServiceProvider();
            invDbContext = serviceProvider.GetService<InventoryDbContext>();


            productRepository = new ProductRepository(invDbContext, mockLogger.Object);
            productBAL = new ProductBAL(mockProductRepo.Object, mockLogger.Object);
        }


        #region Unit Test Case GetAllProducts
        /// <summary>
        /// Get All Products Should Return Product Data Response When Called
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllProductsShouldReturnProductDataResponseWhenCalled()
        {
            // Arrange
            mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(new ProductDataResponse());
            var productResponse = await productRepository.GetAllProducts();
            mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(productResponse);

            // Act
            var result = await productBAL.GetAllProducts();

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.Success));
                Assert.That(result.Data.Count, Is.GreaterThan(0));
                Assert.That(result.Data[0].ProductId, Is.EqualTo(1));
                Assert.That(result.Data[0].ProductName, Is.EqualTo("IPhones"));
                Assert.That(result.Data[0].Quantity, Is.EqualTo(50));
                Assert.That(result.Data[0].Price, Is.EqualTo(150000.00));
                Assert.That(result.Data[0].CreatedBy, Is.EqualTo("Sanjay Nigam"));
            });

            mockProductRepo.Verify(repo => repo.GetAllProducts(), Times.Once);
        }
        /// <summary>
        /// Get All Products Returns Not Found When No Products Exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllProductsReturnsNotFoundWhenNoProductsExist()
        {
            // Arrange
            mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(new ProductDataResponse());
            var productResponse = await productRepository.GetAllProducts();
            mockProductRepo.Setup(repo => repo.GetAllProducts()).ReturnsAsync(productResponse);

            // Act: Call the method to test
            var result = await productBAL.GetAllProducts();

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(0));
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductsFound));
            });
            mockProductRepo.Verify(repo => repo.GetAllProducts(), Times.Once);
        }
        #endregion
    }
}
