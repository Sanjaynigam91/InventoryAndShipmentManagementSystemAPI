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
            productLoggers.LogInformation("DeleteProduct, Bussiness operation execution process started at {'" + DateTime.Now + "'} for product Id {'" + productId + "'}");
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.DeleteProductDetails(productId);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of DeleteProductDetails while deleting product with product Id {'" + productId + "'}");
                throw;
            }
            productLoggers.LogInformation("DeleteProduct, Bussiness operation execution process completed at {'" + DateTime.Now + "'} for product Id {'" + productId + "'}");
            return response;
        }
        /// <summary>
        /// Used to Get All Products
        /// </summary>
        /// <returns></returns>
        public APIResponseModel<object> GetAllProducts()
        {
            productLoggers.LogInformation("GetAllProducts,Bussiness operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.GetAllProducts();
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of GetAllProducts while fetching all products at {'" + DateTime.Now + "'}");
                throw;
            }
            productLoggers.LogInformation("GetAllProducts,Bussiness operation execution process completed at {'" + DateTime.Now + "'}");
            return response;
        }
        /// <summary>
        /// Used to Get All Shipments 
        /// </summary>
        /// <returns></returns>
        public List<ProductShipmentResponse> GetAllShipmentDetails()
        {
            productLoggers.LogInformation("GetAllShipmentDetails,Bussiness operation execution process started at {'" + DateTime.Now + "'}");
            var response = new List<ProductShipmentResponse>();
            try
            {
                response = productRepo.GetAllShipmentDetails();
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of GetAllShipmentDetails while fetching all shipment details at {'" + DateTime.Now + "'}");
                throw;
            }
            productLoggers.LogInformation("GetAllShipmentDetails,Bussiness operation execution process completed at {'" + DateTime.Now + "'}");
            return response;
        }

        /// <summary>
        /// Used to get Product Details By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public APIResponseModel<object> GetProductById(int productId)
        {
            productLoggers.LogInformation("GetProductById, Bussiness operation execution process started at {'" + DateTime.Now + "'} for product Id {'" + productId + "'}");
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.GetProductById(productId);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of GetProductById while fetching product details by product Id {'" + productId + "'} at {'" + DateTime.Now + "'}");
                throw;
            }
            productLoggers.LogInformation("GetProductById, Bussiness operation execution process completed at {'" + DateTime.Now + "'} for product Id {'" + productId + "'}");
            return response;
        }
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            productLoggers.LogInformation("ProductAssignToShipment, Bussiness operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.ProductAssignToShipment(shipmentRequest);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of ProductAssignToShipment while saving the product details like {'" + shipmentRequest + "'}");
                throw;
            }
            productLoggers.LogInformation("ProductAssignToShipment, Bussiness operation execution process completed at {'" + DateTime.Now + "'}");
            return response;
        }

        /// <summary>
        /// Used to Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> SaveProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation("SaveProductDetails, Bussiness operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.SaveProductDetails(productRequest);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of SaveProductDetails while saving the product details like {'" + productRequest + "'}");
                throw;
            }
            productLoggers.LogInformation("SaveProductDetails, Bussiness operation execution process completed at {'" + DateTime.Now + "'}");
            return response;
        }
        /// <summary>
        /// Used to Update Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> UpdateProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation("UpdateProductDetails, Bussiness operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>();
            try
            {
                response = productRepo.UpdateProductDetails(productRequest);
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred in business operation of UpdateProductDetails while updateing the product details of the product Id {'" + productRequest.ProductId + "'}");
                throw;
            }
            productLoggers.LogInformation("UpdateProductDetails, Bussiness operation execution process completed at {'" + DateTime.Now + "'}");
            return response;
        }
    }
}
