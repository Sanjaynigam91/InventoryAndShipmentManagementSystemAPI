using InventoryDTO;
using InventoryRepository.Implementation;
using LISCareDataAccess.InventoryDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;


namespace InventoryAndShipmentManagementTest
{
    [TestFixture]
    public class ProductRepositoryTest
    {
        private Mock<IConfiguration> _mockConfiguration;
        private InventoryDbContext _dbContext;
        private ProductRepository _productRepository;
        private IServiceProvider _serviceProvider;

        [SetUp]
        public void Setup()
        {
            // Mock IConfiguration
            _mockConfiguration = new Mock<IConfiguration>();
            _dbContext = new InventoryDbContext();
            // Setting up mock configuration
            var inMemorySettings = new Dictionary<string, string>
             {
                {"ConnectionStrings:InventoryDbConnection", "Data Source=SANJAY-NIGAM\\SQLEXPRESS;Initial Catalog=IASMS;Integrated Security=true;TrustServerCertificate=True;"}
             };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            // Set up services and DbContext in the test environment
            var services = new ServiceCollection();
            services.AddSingleton<IConfiguration>(configuration);
            services.AddDbContext<InventoryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("InventoryDbConnection")));

            _serviceProvider = services.BuildServiceProvider();
            _dbContext = _serviceProvider.GetService<InventoryDbContext>();

            _productRepository = new ProductRepository(_mockConfiguration.Object, _dbContext);
           
        }

        [Test]
        public async Task GetAllProductsShouldReturnAllProducts()
        {
            // Arrange
            List<ProductResponse> response = new List<ProductResponse>();
            var product1 = new ProductResponse { productId = 1, productName = "Laptop", price = 60000, quantity = 10, createdBy = "Sanjay Nigam", createdDate = DateTime.Now };
            var product2 = new ProductResponse { productId = 2, productName = "Desktop", price = 50000, quantity = 15, createdBy = "Sanjay Nigam", createdDate = DateTime.Now };
            var product3 = new ProductResponse { productId = 3, productName = "Mobile", price = 30000, quantity = 20, createdBy = "Sanjay Nigam", createdDate = DateTime.Now };
            var product4 = new ProductResponse { productId = 4, productName = "Tata Harier", price = 3000000, quantity = 20, createdBy = "Sanjay Nigam", createdDate = DateTime.Now };
            response.Add(product1);
            response.Add(product2);
            response.Add(product3);
            response.Add(product4);

            // Act
            var products = _productRepository.GetAllProducts();

            // Assert
            Assert.Equals(response.Count(), products.Count());
            Assert.Pass();
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of _dbContext after each test to release resources
            _dbContext?.Dispose();
           
        }
    }
}
