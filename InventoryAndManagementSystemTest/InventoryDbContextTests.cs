using InventoryUtility;
using LISCareDataAccess.InventoryDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryAndShipmentManagementTest
{
    [TestFixture]
    public class InventoryDbContextTests
    {
        private IServiceProvider serviceProvider;
        private InventoryDbContext? invDbContext;

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
                options.UseSqlServer(configuration.GetConnectionString(ConstantResources.InventoryDbConnection)));

            serviceProvider = services.BuildServiceProvider();

        }

        [Test]
        public void ShouldRetrieveCorrectConnectionString()
        {
            // Retrieve the DbContext from DI container
            invDbContext = serviceProvider.GetService<InventoryDbContext>();

            // Optionally, you can query the database or check connection settings
            var connectionString = invDbContext.Database.GetDbConnection().ConnectionString;
            CollectionAssert.AreEqual("Data Source=SANJAY-NIGAM\\SQLEXPRESS;Initial Catalog=IASMS;Integrated Security=true;TrustServerCertificate=True;", connectionString);
        }

        [Test]
        public void ShouldInitializeDbContextWithCorrectConnectionString()
        {
            // Arrange
            invDbContext = serviceProvider.GetService<InventoryDbContext>();

            // Act
            var options = invDbContext.Database.GetDbConnection().ConnectionString;

            // Assert
            CollectionAssert.AreEqual("Data Source=SANJAY-NIGAM\\SQLEXPRESS;Initial Catalog=IASMS;Integrated Security=true;TrustServerCertificate=True;", options);
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of invDbContext after each test to release resources
            invDbContext?.Dispose();
        }
    }
}
