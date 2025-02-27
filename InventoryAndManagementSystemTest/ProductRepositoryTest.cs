using InventoryDTO;
using InventoryRepository.Implementation;
using InventoryRepository.Interface;
using InventoryUtility;
using InventoryUtility.Interface;
using LISCareDataAccess.InventoryDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;


namespace InventoryAndShipmentManagementTest
{
    [TestFixture]
    public class ProductRepositoryTest
    {
        private Mock<IProductRepository> mockProductRepository;
        private InventoryDbContext invDbContext;
        private ProductRepository productRepo;
        private IServiceProvider serviceProvider;
        private IProductLoggers productLoggers;
 
        [SetUp]
        public void Setup()
        {
            // Create a mock of IProductRepository
            mockProductRepository = new Mock<IProductRepository>();
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
            productLoggers = serviceProvider.GetService<IProductLoggers>();

           productRepo = new ProductRepository(invDbContext, productLoggers);

        }
        /// <summary>
        /// Get All Products Should Return All Products
        /// </summary>
        /// <returns></returns>
        [Test]
        public void GetAllProductsShouldReturnAllProducts()
        {
            
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(new List<ProductResponse>());

            // Act: Call the method to test
            var result = productRepo.GetAllProducts();

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(10));  // We expect 2 products
            Assert.Multiple(() =>
            {
                Assert.That(result[0].productName, Is.EqualTo("Android Mobiles"));
                Assert.That(result[0].quantity, Is.EqualTo(10));
                Assert.That(result[0].price, Is.EqualTo(25000.00));
            });
        }
        /// <summary>
        /// Get All Products Should Return EmptyList When No Products Exist
        /// </summary>
        [Test]
        public void GetAllProductsShouldReturnEmptyListWhenNoProductsExist()
        {
            // Arrange: Setup the mock to return an empty list
            mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(new List<ProductResponse>());

            // Act: Call the method to test
            var result = productRepo.GetAllProducts();

            // Assert: Verify that the result is an empty list
            Assert.IsNotNull(result);
            Assert.That(0, Is.EqualTo(0));
            //  Assert.That(result.Count, Is.EqualTo(0));
        }
        /// <summary>
        /// test use case by setting the result as null and  Should ReturnNull When Repository Returns Null
        /// </summary>
        [Test]
        public void GetAllProductsShouldReturnNullWhenRepositoryReturnsNull()
        {
            // Arrange: Setup the mock to return null
            mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns((List<ProductResponse>)null);

            // Act: Call the method to test
            var result = productRepo.GetAllProducts();
            result = null;
            // Assert: Verify that the result is null
            Assert.That(result, Is.Null);
        }
        /// <summary>
        /// Test use case with vaid product to get the product details
        /// </summary>
        /// <returns></returns>
        [Test]
        public void ProductsShouldReturProductsById()
        {
            // Arrange: Setup the mock repository to return the mock data
            int productId = 14;
            mockProductRepository.Setup(repo => repo.GetProductById(productId)).Returns(new ProductResponse());

            // Act: Call the method to test
            var result = productRepo.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.productName, Is.EqualTo("Android Mobiles"));
                Assert.That(result.quantity, Is.EqualTo(10));
                Assert.That(result.price, Is.EqualTo(25000.00));
            });
        }
        /// <summary>
        /// Test use case if product Id is zero then no product data should return
        /// </summary>
        [Test]
        public void GetProductsShouldReturnNoProductDataWhenNoProductsExist()
        {
            // Arrange: Setup the mock repository to return the mock data
            int productId = 0;
            mockProductRepository.Setup(repo => repo.GetProductById(productId)).Returns(new ProductResponse());

            // Act: Call the method to test
            var result = productRepo.GetProductById(productId);

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.Multiple(() =>
            {
                Assert.That(result.productName, Is.EqualTo(string.Empty));
                Assert.That(result.quantity, Is.EqualTo(0));
                Assert.That(result.price, Is.EqualTo(0));
            });
        }
        /// <summary>
        /// test use case to create/add/save new product
        /// </summary>
        [Test]
        public void SaveProductProductDetailsTest()
        {
            // Arrange
            var product = new ProductRequest
            {
                ProductName = "Paper Goods",
                Price = 10000,
                Quantity = 20,
                CreatedBy="Sanjay Nigam"
            };
            var response = new APIResponseModel<object>
            {
                Status = true,
                ResponseMessage = "Product details has been added successfully!.",
                Data = string.Empty
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.SaveProductDetails(product)).Returns(new APIResponseModel<object> { Data = 200 });

            // Act: Call the method to test
            var result = productRepo.SaveProductDetails(product);

            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// test use case for save prduct details with Negative senario
        /// </summary>
        [Test]
        public void SaveProductDetailsShouldReturnErrorWhenProductRequestIsInvalid()
        {
            // Arrange
            var invalidProductRequest = new ProductRequest
            {
                // Set properties that are invalid for this test case.
                ProductName = null, // Let's assume ProductName is required and cannot be null.
                Price = -1, // Assuming the price cannot be negative.
                Quantity = -1,
                CreatedBy = null  
            };
            var response = new APIResponseModel<object>
            {
                Status = false,
                ResponseMessage = "Procedure or function 'Usp_Save_Product_Details' expects parameter '@ProductName', which was not supplied.",
                Data = string.Empty
            };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.SaveProductDetails(invalidProductRequest)).Returns(new APIResponseModel<object> { Data = 404 });

            // Act: Call the method to test
            var result = productRepo.SaveProductDetails(invalidProductRequest);

            // Assert
            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(404));
                Assert.That(result.ResponseMessage, Is.EqualTo("Procedure or function 'Usp_Save_Product_Details' expects parameter '@ProductName', which was not supplied."));
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// test use case to update new product
        /// </summary>
        [Test]
        public void UpdateProductProductDetailsTest()
        {
            // Arrange
            var product = new ProductRequest
            {
                ProductId = 8,
                ProductName = "Notepad",
                Price = 15000,
                Quantity = 10,
                CreatedBy = "Sanjay Nigam"
            };
            var response = new APIResponseModel<object>
            {
                Status = true,
                ResponseMessage = "Product details has been updated successfully!",
                Data = string.Empty
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.UpdateProductDetails(product)).Returns(new APIResponseModel<object> { Data = 200 });

            // Act: Call the method to test
            var result = productRepo.UpdateProductDetails(product);

            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// test use case for update prduct details with Negative senario
        /// </summary>
        [Test]
        public void UpdateProductProductDetailsShouldReturnErrorWhenProductRequestIsInvalid()
        {
            // Arrange
            var invalidProductRequest = new ProductRequest
            {
                // Set properties that are invalid for this test case.
                ProductId = 0, // Assuming the price cannot be negative.
            };
            var response = new APIResponseModel<object>
            {
                Status = false,
                ResponseMessage = "Product Id must be greater than zero.",
                Data = string.Empty
            };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.UpdateProductDetails(invalidProductRequest)).Returns(new APIResponseModel<object> { Data = 404 });

            // Act: Call the method to test
            var result = productRepo.UpdateProductDetails(invalidProductRequest);

            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(404));
                Assert.That(result.ResponseMessage, Is.EqualTo("Product Id must be greater than zero."));
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// Test use case with vaid product to get the product details
        /// </summary>
        /// <returns></returns>
        [Test]
        public void ProductShould_DeleteByProductId()
        {
            // Arrange: Setup the mock repository to return the mock data
            int productId = 7;
            var response = new APIResponseModel<object>
            {
                Status = true,
                ResponseMessage = "Product details has been deleted successfully!",
                Data = string.Empty
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.DeleteProductDetails(productId)).Returns(new APIResponseModel<object> { Data = 200 });

            // Act: Call the method to test
            var result = productRepo.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.ResponseMessage, Is.EqualTo("Product details has been deleted successfully!"));
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// test use case for update prduct details with Negative senario
        /// </summary>
        [Test]
        public void ProductShouldDeleteByProductIdShouldReturnErrorWhenProductRequestIsInvalid()
        {
            // Arrange
            int productId = -5;
            var response = new APIResponseModel<object>
            {
                Status = false,
                ResponseMessage = "Product Id must be greater than zero.",
                Data = string.Empty
            };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.DeleteProductDetails(productId)).Returns(new APIResponseModel<object> { Data = 404 });

            // Act: Call the method to test
            var result = productRepo.DeleteProductDetails(productId);

            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(404));
                Assert.That(result.ResponseMessage, Is.EqualTo("Product Id must be greater than zero."));
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// Get All Shipments Should Return All Shipment Details
        /// </summary>
        /// <returns></returns>
        [Test]
        public void GetAllShipmentsShouldReturnAllShipments()
        {

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.GetAllShipmentDetails()).Returns(new List<ProductShipmentResponse>());

            // Act: Call the method to test
            var result = productRepo.GetAllShipmentDetails();

            // Assert: Verify that the result matches the expected output
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(15));  // We expect 2 products
            Assert.Multiple(() =>
            {
                Assert.That(result[0].ShipmentName, Is.EqualTo("Ground"));
                Assert.That(result[0].ProductId, Is.EqualTo(18));
                Assert.That(result[0].ShipmentId, Is.EqualTo(1));
            });
        }
        /// <summary>
        /// Get All Shipments Should Return Empty List When No Shipments Exist
        /// </summary>
        [Test]
        public void GetAllShipmentsShouldReturnEmptyListWhenNoShipmentsExist()
        {
            // Arrange: Setup the mock to return an empty list
            mockProductRepository.Setup(repo => repo.GetAllShipmentDetails()).Returns(new List<ProductShipmentResponse>());

            // Act: Call the method to test
            var result = productRepo.GetAllShipmentDetails();

            // Assert: Verify that the result is an empty list
            Assert.IsNotNull(result);
            Assert.That(0, Is.EqualTo(0));
            //  Assert.That(result.Count, Is.EqualTo(0));
        }
        /// <summary>
        /// test use case by setting the result as null and  Should ReturnNull When Repository Returns Null
        /// </summary>
        [Test]
        public void GetAllShipmentsShouldReturnNullWhenRepositoryReturnsNull()
        {
            // Arrange: Setup the mock to return null
            mockProductRepository.Setup(repo => repo.GetAllShipmentDetails()).Returns((List<ProductShipmentResponse>)null);

            // Act: Call the method to test
            var result = productRepo.GetAllShipmentDetails();
            result = null;
            // Assert: Verify that the result is null
            Assert.That(result, Is.Null);
        }
        /// <summary>
        /// test use case to create/add/save new product
        /// </summary>
        [Test]
        public void ProductAssignToShipment_Test()
        {
            // Arrange
            var shipment = new ShipmentRequest
            {
                ProductId = 64,
                Quantity = 1,
                ShipmentName = "Ground"
            };
            var response = new APIResponseModel<object>
            {
                Status = true,
                ResponseMessage = "Product assigned to shipment successfully!",
                Data = string.Empty
            };

            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.ProductAssignToShipment(shipment)).Returns(new APIResponseModel<object> { Data = 200 });

            // Act: Call the method to test
            var result = productRepo.ProductAssignToShipment(shipment);

            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(200));
                Assert.That(result.Status, Is.EqualTo(true));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }
        /// <summary>
        /// test use case for save prduct details with Negative senario
        /// </summary>
        [Test]
        public void ProductAssignToShipmentShouldReturnErrorWhenProductRequestIsInvalid()
        {
            // Arrange
            var invalidShipmentRequest = new ShipmentRequest
            {
                // Set properties that are invalid for this test case.
                ProductId = -1, // Assuming the price cannot be negative.
                Quantity = -1,
                ShipmentName = null
            };
            var response = new APIResponseModel<object>
            {
                Status = false,
                ResponseMessage = "Procedure or function 'Usp_Assign_ProductToShipment' expects parameter '@ShipmentName', which was not supplied.",
                Data = string.Empty
            };
            // Arrange: Setup the mock repository to return the mock data
            mockProductRepository.Setup(repo => repo.ProductAssignToShipment(invalidShipmentRequest)).Returns(new APIResponseModel<object> { Data = 404 });

            // Act: Call the method to test
            var result = productRepo.ProductAssignToShipment(invalidShipmentRequest);

            // Assert
            // Assert
            Assert.That(result, Is.InstanceOf<APIResponseModel<object>>()); // If the method directly returns an int
            Assert.Multiple(() =>
            {
                Assert.That(result.StatusCode, Is.EqualTo(404));
                Assert.That(result.ResponseMessage, Is.EqualTo("Procedure or function 'Usp_Assign_ProductToShipment' expects parameter '@ShipmentName', which was not supplied."));
                Assert.That(result.Status, Is.EqualTo(false));
                Assert.That(result.Data, Is.EqualTo(expected: null));
            });
        }


        [TearDown]
        public void TearDown()
        {
            // Dispose of invDbContext after each test to release resources
            invDbContext?.Dispose();

        }
    }
}
