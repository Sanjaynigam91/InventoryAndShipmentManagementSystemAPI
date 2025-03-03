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
using Moq;
using System.Net;


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
            mockProductRepo.Setup(repo => repo.SaveProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });
            var productSaveResponse = await productRepository.SaveProductDetails(productRequest);
            mockProductRepo.Setup(repo => repo.SaveProductDetails(productRequest)).ReturnsAsync(productSaveResponse);
            // Act
            var result = await productBAL.SaveProductDetails(productRequest);

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
            var productRequest = new ProductRequest { ProductName = "Bat", Price = 1000, Quantity = 100, CreatedDate = DateTime.Now, CreatedBy = "Sanjay Nigam" };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepo.Setup(repo => repo.SaveProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });
            var productSaveResponse = await productRepository.SaveProductDetails(productRequest);
            mockProductRepo.Setup(repo => repo.SaveProductDetails(productRequest)).ReturnsAsync(productSaveResponse);
            // Act
            var result = await productBAL.SaveProductDetails(productRequest);

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
            mockProductRepo.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.NotFound });
            var productUpdateResponse = await productRepository.UpdateProductDetails(productRequest);
            mockProductRepo.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(productUpdateResponse);

            // Act
            var result = await productBAL.UpdateProductDetails(productRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductFoundResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
            mockProductRepo.Verify(repo => repo.UpdateProductDetails(productRequest), Times.Once);
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
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepo.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });
            var productUpdateResponse = await productRepository.UpdateProductDetails(productRequest);
            mockProductRepo.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(productUpdateResponse);

            // Act
            var result = await productBAL.UpdateProductDetails(productRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.InValidProductId));
            });
            mockProductRepo.Verify(repo => repo.UpdateProductDetails(productRequest), Times.Once);
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
            mockProductRepo.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });
            var productUpdateResponse = await productRepository.UpdateProductDetails(productRequest);
            mockProductRepo.Setup(repo => repo.UpdateProductDetails(productRequest)).ReturnsAsync(productUpdateResponse);

            // Act
            var result = await productBAL.UpdateProductDetails(productRequest);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductUpdateResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
            mockProductRepo.Verify(repo => repo.UpdateProductDetails(productRequest), Times.Once);
        }
        #endregion

        #region Unit Test Case for Delete Product Details
        /// <summary>
        /// Delete Product Details No Product Found Returns Not Found
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteProductDetailsNoProductFoundReturnsNotFound()
        {
            // Arrange: Setup the mock repository to return the mock data
            int productId = 200;
            mockProductRepo.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.NotFound });
            var productDeleteResult = await productRepository.DeleteProductDetails(productId);
            mockProductRepo.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(productDeleteResult);

            // Act
            var result = await productBAL.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductFoundResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
            mockProductRepo.Verify(repo => repo.DeleteProductDetails(productId), Times.Once);
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
            mockProductRepo.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });
            var productDeleteResult = await productRepository.DeleteProductDetails(productId);
            mockProductRepo.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(productDeleteResult);

            // Act
            var result = await productBAL.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductIdGreaterThanZero + productId));
            });
            mockProductRepo.Verify(repo => repo.DeleteProductDetails(productId), Times.Once);
        }
        /// <summary>
        /// Delete Product Details Valid ProductId Returns Success Response
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteProductDetailsValidProductIdReturnsSuccessResponse()
        {
            // Arrange
            int productId = 22;
            mockProductRepo.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });
            var productDeleteResult = await productRepository.DeleteProductDetails(productId);
            mockProductRepo.Setup(repo => repo.DeleteProductDetails(productId)).ReturnsAsync(productDeleteResult);

            // Act
            var result = await productBAL.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.True);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductDeleteResponseMsg));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
            mockProductRepo.Verify(repo => repo.DeleteProductDetails(productId), Times.Once);
        }
        #endregion

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
            mockProductRepo.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(new ProductModel());
            var productResult = await productRepository.GetProductById(productId);
            mockProductRepo.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(productResult);

            // Act: Call the method to test
            var result = await productBAL.GetProductById(productId);

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
            mockProductRepo.Verify(repo => repo.GetProductById(productId), Times.Once);
        }
        /// <summary>
        /// Get Product By Id Invalid ProductId Returns NotFound
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetProductByIdInvalidProductIdReturnsNotFound()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            int productId = -1;
            mockProductRepo.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(new ProductModel());
            var productResult = await productRepository.GetProductById(productId);
            mockProductRepo.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(productResult);

            // Act: Call the method to test
            var result = await productBAL.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductIdGreaterThanZero + productId));
                Assert.That(result.Data, Is.Null);
            });
            mockProductRepo.Verify(repo => repo.GetProductById(productId), Times.Once);
        }
        /// <summary>
        /// Get Product By Id Product Not Found Returns Not Found
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetProductByIdProductNotFoundReturnsNotFound()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            int productId = 1000;
            mockProductRepo.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(new ProductModel());
            var productResult = await productRepository.GetProductById(productId);
            mockProductRepo.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(productResult);

            // Act: Call the method to test
            var result = await productBAL.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoProductFound + " which is " + productId));
                Assert.That(result.Data, Is.Null);
            });
            mockProductRepo.Verify(repo => repo.GetProductById(productId), Times.Once);
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
            mockProductRepo.Setup(repo => repo.GetAllShipmentDetails()).ReturnsAsync(new ProductShipmentResponse());
            var productShipments = await productRepository.GetAllShipmentDetails();
            mockProductRepo.Setup(repo => repo.GetAllShipmentDetails()).ReturnsAsync(productShipments);

            // Act: Call the method to test
            var result = await productBAL.GetAllShipmentDetails();

            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(0));
                Assert.That(result.Status, Is.False);
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NotFound));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.NoShipmetFound));
            });

            mockProductRepo.Verify(repo => repo.GetAllShipmentDetails(), Times.Once);
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
            mockProductRepo.Setup(repo => repo.ProductAssignToShipment(shipmentRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.BadRequest });
            var productShipments = await productRepository.ProductAssignToShipment(shipmentRequest);
            mockProductRepo.Setup(repo => repo.ProductAssignToShipment(shipmentRequest)).ReturnsAsync(productShipments);

            // Act
            var result = await productBAL.ProductAssignToShipment(shipmentRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.InValidShipmentRequest));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
            mockProductRepo.Verify(repo => repo.ProductAssignToShipment(shipmentRequest), Times.Once);
        }
        /// <summary>
        /// Product Assign To Shipment Should Return Success When Shipment Request Is Valid
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task ProductAssignToShipmentShouldReturnSuccessWhenShipmentRequestIsValid()
        {
            // Arrange
            var shipmentRequest = new ShipmentRequest { ShipmentName = "Air", ProductId = 4, Quantity = 10 };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepo.Setup(repo => repo.ProductAssignToShipment(shipmentRequest)).ReturnsAsync(new APIResponseModel<string> { StatusCode = (int)HttpStatusCode.OK });
            var productShipments = await productRepository.ProductAssignToShipment(shipmentRequest);
            mockProductRepo.Setup(repo => repo.ProductAssignToShipment(shipmentRequest)).ReturnsAsync(productShipments);

            // Act
            var result = await productBAL.ProductAssignToShipment(shipmentRequest);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.OK));
                Assert.That(result.ResponseMessage, Is.EqualTo(ConstantResources.ProductShipedResponse));
                Assert.That(result.Data, Is.EqualTo(string.Empty));
            });
            mockProductRepo.Verify(repo => repo.ProductAssignToShipment(shipmentRequest), Times.Once);
        }
        /// <summary>
        /// Get All Shipment Details Returns Shipments When Shipments Exist
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllShipmentDetailsReturnsShipmentsWhenShipmentsExist()
        {
            // Arrange: Setup the mock logger and repository to return the mock data
            mockProductRepo.Setup(repo => repo.GetAllShipmentDetails()).ReturnsAsync(new ProductShipmentResponse());
            var productShipments = await productRepository.GetAllShipmentDetails();
            mockProductRepo.Setup(repo => repo.GetAllShipmentDetails()).ReturnsAsync(productShipments);


            // Act: Call the method to test
            var result = await productBAL.GetAllShipmentDetails();

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
            mockProductRepo.Verify(repo => repo.GetAllShipmentDetails(), Times.Once);
        }
        #endregion
    }
}
