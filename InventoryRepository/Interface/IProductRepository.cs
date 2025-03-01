using InventoryDTO;


namespace InventoryRepository.Interface
{
    public interface IProductRepository
    {

        /// <summary>
        ///  Used for Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<APIResponseModel<string>> SaveProductDetails(ProductRequest productRequest);
        /// <summary>
        ///  Used for upadte Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<APIResponseModel<string>> UpdateProductDetails(ProductRequest productRequest);
        /// <summary>
        /// Used for Get All Products
        /// </summary>
        /// <returns></returns>
        Task<ProductDataResponse> GetAllProducts();
        /// <summary>
        /// Used to Get Product By Product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        Task<ProductModel> GetProductById(int productId);
        /// <summary>
        ///  Used for delete Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        Task<APIResponseModel<string>> DeleteProductDetails(int productId);
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        Task<APIResponseModel<string>> ProductAssignToShipment(ShipmentRequest shipmentRequest);
        /// <summary>
        /// Used for Get All Product shipment history 
        /// </summary>
        /// <returns></returns>
        Task<ProductShipmentResponse> GetAllShipmentDetails();
    }
}
