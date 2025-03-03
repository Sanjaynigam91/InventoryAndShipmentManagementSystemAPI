using InventoryDTO;
using InventoryRepository.Implementation;
using InventoryRepository.Interface;
using InventoryUtility;
using LISCareDataAccess.IInventoryDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;


namespace InventoryAndShipmentManagementTest
{
    [TestFixture]
    public class ProductRepositoryTest
    {
        private Mock<IProductRepository> mockProductRepository;
        private InventoryDbContext invDbContext;
        private ProductRepository productRepo;
        private IServiceProvider serviceProvider;
        private Mock<ProductLoggers> mockProductLoggers;

        [SetUp]
        public void Setup()
        {
            // Create a mock of IProductRepository
            mockProductRepository = new Mock<IProductRepository>();
            invDbContext = new InventoryDbContext();
            mockProductLoggers = new Mock<ProductLoggers>();

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

            productRepo = new ProductRepository(invDbContext, mockProductLoggers.Object);

        }

        #region Unit Test Case for Save Product Details
        /// <summary>
        /// Save Product Details Should Return BadRequest When Product Request is Invalid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SaveProductDetailsShouldReturnBadRequestWhenProductRequestIsInvalid()
        {
            // Arrange
            var productRequest = new ProductRequest { ProductName = "", Price = 0, Quantity = 0 };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.SaveProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });

            // Act
            var result = await productRepo.SaveProductDetails(productRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.InValidRequest));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        /// <summary>
        /// Save ProductDetails Should Return Success When Product Request Is Valid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task SaveProductDetailsShouldReturnSuccessWhenProductRequestIsValid()
        {
            // Arrange
            var productRequest = new ProductRequest { ProductName = "Mouse", Price = 1000, Quantity = 150, CreatedDate = DateTime.Now, CreatedBy = "Sanjay Nigam" };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.SaveProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });

            // Act
            var result = await productRepo.SaveProductDetails(productRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductSaveResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        #endregion

        #region Unit Test Case for Update Product Details
        /// <summary>
        /// Update Product Details No Product Found Returns Not Found
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductDetailsNoProductFoundReturnsNotFound()
        {
            // Arrange
            var productRequest = new ProductRequest
            {
                ProductId = 200,
                ProductName = "No Product found test",
                Quantity = 10,
                Price = 5,
                CreatedBy = "TestUser"
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.NotFound });

            // Act
            var result = await productRepo.UpdateProductDetails(productRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductFoundResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        /// <summary>
        /// Update Product Details Invalid ProductId Returns BadRequest
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductDetailsInvalidProductIdReturnsBadRequest()
        {
            // Arrange
            var productRequest = new ProductRequest
            {
                ProductId = 0,
                ProductName = "Invalid Product",
                Quantity = 0,
                Price = 0,
                CreatedBy = "TestUser"
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });

            // Act
            var result = await productRepo.UpdateProductDetails(productRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.InValidProductId));
            });
        }
        /// <summary>
        /// Update Product Details Valid ProductId Returns Success Response
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateProductDetailsValidProductIdReturnsSuccessResponse()
        {
            // Arrange
            var productRequest = new ProductRequest
            {
                ProductId = 11,
                ProductName = "Water Bottle",
                Quantity = 500,
                Price = 40,
                CreatedBy = "Sanjay Nigam"
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });

            // Act
            var result = await productRepo.UpdateProductDetails(productRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductUpdateResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        #endregion

        #region Unit Test Case for Delete Product Details
        /// <summary>
        /// Delete Product Details NoProduct Found Returns Not Found
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteProductDetailsNoProductFoundReturnsNotFound()
        {
            // Arrange: Setup the mock repository to return the mock data
            int productId = 200;
            mockProductRepository.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.NotFound });

            // Act
            var result = await productRepo.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductFoundResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        /// <summary>
        /// Delete Product Details Invalid ProductId Returns BadRequest
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteProductDetailsInvalidProductIdReturnsBadRequest()
        {
            // Arrange: Setup the mock repository to return the mock data
            int productId = -1;
            mockProductRepository.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });

            // Act
            var result = await productRepo.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductIdGreaterThanZero + productId));
            });
        }
        /// <summary>
        /// Delete Product Details Valid ProductId Returns Success Response
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteProductDetailsValidProductIdReturnsSuccessResponse()
        {
            // Arrange
            int productId = 20;
            mockProductRepository.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });

            // Act
            var result = await productRepo.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductDeleteResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        #endregion

        #region Unit Test Case for Get All Products

        /// <summary>
        /// Get All Products Returns Products When Products Exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllProductsReturnsProductsWhenProductsExist()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            mockProductRepository.Setup(repo => repo.GetAllProducts()).ReturnsAsync(new ProductDataResponse());
            // Act: Call the method to test
            var result = await productRepo.GetAllProducts();

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
        }
        /// <summary>
        /// Get All Products Returns Not Found When No Products Exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllProductsReturnsNotFoundWhenNoProductsExist()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            mockProductRepository.Setup(repo => repo.GetAllProducts()).ReturnsAsync(new ProductDataResponse());
            // Act: Call the method to test
            var result = await productRepo.GetAllProducts();

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(0));
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductsFound));
            });
        }

        #endregion

        #region Unit Test Case for Get Product By Id
        /// <summary>
        /// Get Product By Id Valid ProductId Returns Product
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetProductByIdValidProductIdReturnsProduct()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            int productId = 3;
            mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(new ProductModel());

            // Act: Call the method to test
            var result = await productRepo.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.Success));
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Data.ProductId, Is.EqualTo(3));
                Assert.That(result.Data.ProductName, Is.EqualTo("Paper"));
                Assert.That(result.Data.Quantity, Is.EqualTo(100));
                Assert.That(result.Data.Price, Is.EqualTo(10));
                Assert.That(result.Data.CreatedBy, Is.EqualTo("Sanjay Nigam"));
            });
        }
        /// <summary>
        /// Get Product By Id Invalid ProductId Returns NotFound
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetProductByIdInvalidProductIdReturnsNotFound()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            int productId = 0;
            mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(new ProductModel());

            // Act: Call the method to test
            var result = await productRepo.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductIdGreaterThanZero + productId));
                Assert.That(result.Data, Is.Null);
            });
        }
        /// <summary>
        /// Get Product By Id Product Not Found Returns Not Found
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetProductByIdProductNotFoundReturnsNotFound()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            int productId = 100;
            mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(new ProductModel());

            // Act: Call the method to test
            var result = await productRepo.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductFound + " which is " + productId));
                Assert.That(result.Data, Is.Null);
            });
        }
        #endregion

        #region Unit Test Case for Shipment Module
        /// <summary>
        /// Get All Shipment Details Returns Not Found When No Shipments Exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllShipmentDetailsReturnsNotFoundWhenNoShipmentsExist()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            mockProductRepository.Setup(repo => repo.GetAllShipmentDetails()).ReturnsAsync(new ProductShipmentResponse());

            // Act: Call the method to test
            var result = await productRepo.GetAllShipmentDetails();

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(0));
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoShipmetFound));
            });
        }
        /// <summary>
        /// Product Assign To Shipment Should Return BadRequest When Product Request is Invalid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ProductAssignToShipmentShouldReturnBadRequestWhenProductRequestIsInvalid()
        {
            // Arrange
            var shipmentRequest = new ShipmentRequest { ShipmentName = "", ProductId = 0, Quantity = 0 };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.ProductAssignToShipment(shipmentRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });

            // Act
            var result = await productRepo.ProductAssignToShipment(shipmentRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.InValidShipmentRequest));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        /// <summary>
        /// Save ProductDetails Should Return Success When Product Request Is Valid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ProductAssignToShipmentShouldReturnSuccessWhenShipmentRequestIsValid()
        {
            // Arrange
            var shipmentRequest = new ShipmentRequest { ShipmentName = "Air", ProductId = 4, Quantity = 100 };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.ProductAssignToShipment(shipmentRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });

            // Act
            var result = await productRepo.ProductAssignToShipment(shipmentRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductShipedResponse));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
        }
        /// <summary>
        /// Get All Shipment Details Returns Shipments When Shipments Exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllShipmentDetailsReturnsShipmentsWhenShipmentsExist()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            mockProductRepository.Setup(repo => repo.GetAllShipmentDetails()).ReturnsAsync(new ProductShipmentResponse());

            // Act: Call the method to test
            var result = await productRepo.GetAllShipmentDetails();

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.Success));
                Assert.That(result.Data.Count, Is.GreaterThan(0));
                Assert.That(result.Data[0].ProductId, Is.EqualTo(4));
                Assert.That(result.Data[0].ShipmentId, Is.EqualTo(1));
                Assert.That(result.Data[0].ShipmentName, Is.EqualTo("Air"));
                Assert.That(result.Data[0].Quantity, Is.EqualTo(100));
            });

        }
        #endregion

        [TearDown]
        public void TearDown()
        {
            // Dispose of invDbContext after each test to release resources
            invDbContext?.Dispose();

        }
    }
}
