using LISCareDataAccess.InventoryDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework.Legacy;

namespace InventoryAndShipmentManagementTest
{
    [TestFixture]
    public class InventoryDbContextTests
    {
        private IServiceProvider _serviceProvider;
        private InventoryDbContext? _dbContext;

        // This method is run before each test
        [SetUp]
        public void Setup()
        {
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

        }

        [Test]
        public void ShouldRetrieveCorrectConnectionString()
        {
            // Retrieve the DbContext from DI container
            _dbContext = _serviceProvider.GetService<InventoryDbContext>();

            // Optionally, you can query the database or check connection settings
            var connectionString = _dbContext.Database.GetDbConnection().ConnectionString;
            CollectionAssert.AreEqual("Data Source=SANJAY-NIGAM\\SQLEXPRESS;Initial Catalog=IASMS;Integrated Security=true;TrustServerCertificate=True;", connectionString);
        }

        [Test]
        public void ShouldInitializeDbContextWithCorrectConnectionString()
        {
            // Arrange
            _dbContext = _serviceProvider.GetService<InventoryDbContext>();

            // Act
            var options = _dbContext.Database.GetDbConnection().ConnectionString;

            // Assert
            CollectionAssert.AreEqual("Data Source=SANJAY-NIGAM\\SQLEXPRESS;Initial Catalog=IASMS;Integrated Security=true;TrustServerCertificate=True;", options);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of _dbContext after each test to release resources
            _dbContext?.Dispose();
        }
    }
}
