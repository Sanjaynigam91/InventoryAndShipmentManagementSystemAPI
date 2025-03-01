using InventoryBAL.Interface;
using InventoryDTO;
using InventoryRepository.Interface;
using InventoryUtility;
using InventoryUtility.Interface;
using Microsoft.Extensions.Configuration;


namespace InventoryBAL.Implementation
{
    public class ProductBAL : IProduct
    {
        private readonly IProductRepository productRepo;
        private readonly IProductLoggers productLoggers;

        public ProductBAL(IProductRepository productRepository, IProductLoggers productLogger)
        {
            productRepo = productRepository;
            productLoggers = productLogger;
        }
        /// <summary>
        /// Used to Delete Product Details
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public APIResponseModel<object> DeleteProductDetails(int productId)
        {
            productLoggers.LogInformation(ConstantResources.DeleteProductBALStart + productId);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.DeleteProductDetails(productId);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileDeleteProductBAL + productId);
                throw;
            }
            productLoggers.LogInformation(ConstantResources.DeleteProductBALComplete + productId);
            return response;
        }
        /// <summary>
        /// Used to Get All Products
        /// </summary>
        /// <returns></returns>
        public APIResponseModel<object> GetAllProducts()
        {
            productLoggers.LogInformation(ConstantResources.GetAllProductsBALStart);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.GetAllProducts();
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileGettingAllProductsBAL);
                throw;
            }
            productLoggers.LogInformation(ConstantResources.GetAllProductsBALComplete);
            return response;
        }
        /// <summary>
        /// Used to Get All Shipments 
        /// </summary>
        /// <returns></returns>
        public APIResponseModel<object> GetAllShipmentDetails()
        {
            productLoggers.LogInformation(ConstantResources.GetAllShipmentsBALStart);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.GetAllShipmentDetails();
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionGetAllShipmentsBAL);
                throw;
            }
            productLoggers.LogInformation(ConstantResources.GetAllShipmentsBAComplete);
            return response;
        }

        /// <summary>
        /// Used to get Product Details By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public APIResponseModel<object> GetProductById(int productId)
        {
            productLoggers.LogInformation(ConstantResources.GetProductByIdBALStart + ConstantResources.ForProductId + productId);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.GetProductById(productId);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileGettingProductById + productId + " at {'" + ConstantResources.timeStamp + "'}");
                throw;
            }
            productLoggers.LogInformation(ConstantResources.GetProductByIdBALComplete + productId);
            return response;
        }
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            productLoggers.LogInformation(ConstantResources.AssignToShipmentBALStart);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.ProductAssignToShipment(shipmentRequest);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionAssignToShipmentBAL + shipmentRequest);
                throw;
            }
            productLoggers.LogInformation(ConstantResources.AssignToShipmentBALComplete);
            return response;
        }

        /// <summary>
        /// Used to Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> SaveProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation(ConstantResources.SaveProductDetailsBALStart);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.SaveProductDetails(productRequest);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.SaveProductDetailsBALException + productRequest);
                throw;
            }
            productLoggers.LogInformation(ConstantResources.SaveProductDetailsBALComplete);
            return response;
        }
        /// <summary>
        /// Used to Update Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> UpdateProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation(ConstantResources.UpdateProductDetailsBALStart);
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.UpdateProductDetails(productRequest);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.UpdateProductDetailsBALException + productRequest.ProductId);
                throw;
            }
            productLoggers.LogInformation(ConstantResources.UpdateProductDetailsBALComplete);
            return response;
        }
    }
}
