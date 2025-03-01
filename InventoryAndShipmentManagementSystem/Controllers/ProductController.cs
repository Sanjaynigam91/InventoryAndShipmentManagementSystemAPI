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
        public IActionResult CreateNewProduct(ProductRequest productRequest)
        {
            productLoggers.LogInformation(ConstantResources.AddNewProductStart);
            var result = products.SaveProductDetails(productRequest);
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
        /// <summary>
        /// API for to update product details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>

        [HttpPut]
        [Route(ConstantResources.UpdateProduct)]
        public IActionResult UpdateProduct(ProductRequest productRequest)
        {
            productLoggers.LogInformation(ConstantResources.UpdateProductAPIStart + productRequest.ProductId);
            var result = products.UpdateProductDetails(productRequest);
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
        /// <summary>
        /// Used to get Product Details By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetProductById)]
        public IActionResult GetProductById(int productId)
        {
            productLoggers.LogInformation(ConstantResources.GetProductByIdAPIStart + productId);
            var result = products.GetProductById(productId);
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
        /// <summary>
        /// Used to get All Product Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetAllProducts)]
        public IActionResult GetAllProducts()
        {
            productLoggers.LogInformation(ConstantResources.GetAllProductsAPIStart);
            var result = products.GetAllProducts();
            if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
            {
                productLoggers.LogInformation(ConstantResources.GetAllProductsAPIComplete + ConstantResources.WithStatus + result.Status);
                return Ok(result);
            }
            else
            {
                productLoggers.LogInformation(ConstantResources.GetAllProductsAPIComplete + ConstantResources.WithStatus + result.Status);
                return NotFound(result);
            }

        }
        /// <summary>
        /// API for to Delete the product details
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ConstantResources.DeleteProduct)]
        public IActionResult DeleteProduct(int productId)
        {
            productLoggers.LogInformation(ConstantResources.DeleteProductAPIStart + productId);
            var result = products.DeleteProductDetails(productId);
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

        /// <summary>
        /// API for to product getting assign to shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>

        [HttpPost]
        [Route(ConstantResources.AssignToShipment)]
        public IActionResult ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            productLoggers.LogInformation(ConstantResources.AssignToShipmentAPIStart);
            var result = products.ProductAssignToShipment(shipmentRequest);
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
        /// <summary>
        /// Used to get All Product Details
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetAllShipments)]
        public IActionResult GetAllShipments()
        {
            productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIStart);
            var result = products.GetAllShipmentDetails();
            if (result.Status && result.StatusCode == (int)HttpStatusCode.OK)
            {
                productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIComplete + ConstantResources.WithStatus + result.Status);
                return Ok(result);
            }
            else
            {
                productLoggers.LogInformation(ConstantResources.GetAllShipmentsAPIComplete + ConstantResources.WithStatus + result.Status);
                return NotFound(result);
            }

        }
    }
}
