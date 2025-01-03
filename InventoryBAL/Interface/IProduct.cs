﻿
using InventoryDTO;

namespace InventoryBAL.Interface
{
    public interface IProduct
    {
        /// <summary>
        ///  Used for Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        APIResponseModel<object> SaveProductDetails(ProductRequest productRequest);
        /// <summary>
        ///  Used for upadte Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        APIResponseModel<object> UpdateProductDetails(ProductRequest productRequest);
        /// <summary>
        /// Used for Get All Products
        /// </summary>
        /// <returns></returns>
        List<ProductResponse> GetAllProducts();
        /// <summary>
        /// Used to Get Product By Product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        ProductResponse GetProductById(int productId);
        /// <summary>
        ///  Used for Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        APIResponseModel<object> DeleteProductDetails(int productId);
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        APIResponseModel<object> ProductAssignToShipment(ShipmentRequest shipmentRequest);
        /// <summary>
        /// Used for Get All Product shipment history 
        /// </summary>
        /// <returns></returns>
        List<ProductShipmentResponse> GetAllShipmentDetails();
    }
}
