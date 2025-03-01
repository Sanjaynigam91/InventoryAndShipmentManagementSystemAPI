using InventoryBAL.Interface;
using InventoryDTO;
using InventoryUtility;
using InventoryUtility.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace InventoryAndShipmentManagementSystem.Controllers
{
    [Route(ConstantResources.APIRoute)]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct products;
        private readonly IProductLoggers productLoggers;
        public ProductController(IProduct product, IProductLoggers productLogger)
        {
            products = product;
            productLoggers = productLogger;
        }
        /// <summary>
        /// API for to create new product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>

        [HttpPost]
        [Route(ConstantResources.AddNewProduct)]
        public async Task<IActionResult> CreateNewProduct(ProductRequest productRequest)
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.AddNewProductStart);
                var result = await products.SaveProductDetails(productRequest);
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.AddNewProductComplete + result.Status);
                    return Ok(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.AddNewProductComplete + result.Status);
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// API for to update product details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>

        [HttpPut]
        [Route(ConstantResources.UpdateProduct)]
        public async Task<IActionResult> UpdateProduct(ProductRequest productRequest)
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.UpdateProductAPIStart + productRequest.ProductId);
                var result = await products.UpdateProductDetails(productRequest);
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.UpdateProductAPIComplete + result.Status);
                    return Ok(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.UpdateProductAPIComplete + result.Status);
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Used to get Product Details By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetProductById)]
        public async Task<IActionResult> GetProductById(int productId)
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.GetProductByIdAPIStart + productId);
                var result = await products.GetProductById(productId);
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.GetProductByIdAPIComplete + ConstantResources.WithStatus +
                        "{'" + result.Status + "'}" + ConstantResources.OfProductId + productId);
                    return Ok(result);
                }
                else if (!result.Status && result.StatusCode == (int)HttpStatusCode.BadRequest)
                {
                    productLoggers.LogInformation(ConstantResources.GetProductByIdAPIComplete + ConstantResources.WithStatus +
                         "{'" + result.Status + "'}" + ConstantResources.OfProductId + productId);
                    return BadRequest(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.GetProductByIdAPIComplete + ConstantResources.WithStatus +
                         "{'" + result.Status + "'}" + ConstantResources.OfProductId + productId);
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Used to get All Product Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetAllProducts)]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.GetAllProductsAPIStart);
                var result = await products.GetAllProducts();
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.GetAllProductsAPIComplete + ConstantResources.WithStatus + result.Status);
                    return Ok(result);
                }
                else if (!result.Status && result.StatusCode == (int)HttpStatusCode.BadRequest)
                {
                    productLoggers.LogInformation(ConstantResources.GetAllProductsAPIComplete + ConstantResources.WithStatus + result.Status);
                    return BadRequest(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.GetAllProductsAPIComplete + ConstantResources.WithStatus + result.Status);
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// API for to Delete the product details
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ConstantResources.DeleteProduct)]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.DeleteProductAPIStart + productId);
                var result =await products.DeleteProductDetails(productId);
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.DeleteProductAPIComplete + result.Status + ConstantResources.ForProductId + productId);
                    return Ok(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.DeleteProductAPIComplete + result.Status + ConstantResources.ForProductId + productId);
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// API for to product getting assign to shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>

        [HttpPost]
        [Route(ConstantResources.AssignToShipment)]
        public async Task<IActionResult> ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.AssignToShipmentAPIStart);
                var result =await products.ProductAssignToShipment(shipmentRequest);
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.AssignToShipmentAPIComplete + result.Status);
                    return Ok(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.AssignToShipmentAPIComplete + result.Status);
                    return BadRequest(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }

        }
        /// <summary>
        /// Used to get All Product Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetAllShipments)]
        public async Task<IActionResult> GetAllShipments()
        {
            try
            {
                productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIStart);
                var result = await products.GetAllShipmentDetails();
                if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
                {
                    productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIComplete + ConstantResources.WithStatus + result.Status);
                    return Ok(result);
                }
                else if (!result.Status && result.StatusCode == (int)HttpStatusCode.BadRequest)
                {
                    productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIComplete + ConstantResources.WithStatus + result.Status);
                    return BadRequest(result);
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIComplete + ConstantResources.WithStatus + result.Status);
                    return NotFound(result);
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation(ex.Message + "," + " at {'" + ConstantResources.timeStamp + "'}");
                return BadRequest(ex.Message);
            }

        }
    }
}
