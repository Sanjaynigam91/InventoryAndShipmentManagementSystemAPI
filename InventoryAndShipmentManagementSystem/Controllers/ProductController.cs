using InventoryBAL.Interface;
using InventoryDTO;
using InventoryUtility;
using Microsoft.AspNetCore.Mvc;


namespace InventoryAndShipmentManagementSystem.Controllers
{
    [Route(ConstantResources.APIRoute)]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProduct products;
        private readonly ProductLogger productLogger;
        public ProductController(IProduct product)
        {
            products = product;
            productLogger = new ProductLogger();
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
            productLogger.LogInformation("AddNewProduct, API execution process started at {'" + DateTime.Now + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.SaveProductDetails(productRequest);
            if (result.Status && result.StatusCode == 200)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = result.ResponseMessage;
                responseModel.Data = string.Empty;
                productLogger.LogInformation("AddNewProduct, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("AddNewProduct, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'}");
                return NotFound(responseModel);
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
            productLogger.LogInformation("UpdateProduct, API execution process started at {'" + DateTime.Now + "'} of the product Id {'" + productRequest.ProductId + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.UpdateProductDetails(productRequest);
            if (result.Status && result.StatusCode == 200)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = result.ResponseMessage;
                responseModel.Data = string.Empty;
                productLogger.LogInformation("UpdateProduct, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("UpdateProduct, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'}");
                return NotFound(responseModel);
            }

        }
        /// <summary>
        /// Used to get Product Details By Id
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetProductById)]
        public IActionResult GetProductById(int productId)
        {
            productLogger.LogInformation("GetProductById, API execution process started at {'" + DateTime.Now + "'} for the product Id {'" + productId + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.GetProductById(productId);
            if (result != null)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = ConstantResources.Success;
                responseModel.Data = result;
                productLogger.LogInformation("GetProductById, API execution process completed at {'" + DateTime.Now + "'} " +
                    "with status {'" + responseModel + "'} of product Id {'" + productId + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("GetProductById, API execution process completed at {'" + DateTime.Now + "'} " +
                    "with status {'" + responseModel + "'} of product Id {'" + productId + "'}");
                return NotFound(responseModel);
            }

        }
        /// <summary>
        /// Used to get All Product Details
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetAllProducts)]
        public IActionResult GetAllProducts()
        {

            productLogger.LogInformation("GetAllProducts, API execution process started at {'" + DateTime.Now + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.GetAllProducts();
            if (result.Count > 0)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = ConstantResources.Success;
                responseModel.Data = result;
                productLogger.LogInformation("GetAllProducts, API execution process completed at {'" + DateTime.Now + "'} " +
                   "with status {'" + responseModel.Status + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("GetAllProducts, API execution process completed at {'" + DateTime.Now + "'} " +
                   "with status {'" + responseModel.Status + "'}");
                return NotFound(responseModel);
            }

        }
        /// <summary>
        /// API for to create new product
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route(ConstantResources.DeleteProduct)]
        public IActionResult DeleteProduct(int productId)
        {
            productLogger.LogInformation("DeleteProduct, API execution process started at {'" + DateTime.Now + "'} of the product Id {'" + productId + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.DeleteProductDetails(productId);
            if (result.Status && result.StatusCode == 200)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = result.ResponseMessage;
                responseModel.Data = string.Empty;
                productLogger.LogInformation("DeleteProduct, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'} for product Id {'"+ productId + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("DeleteProduct, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'} for product Id {'"+ productId + "'}");
                return NotFound(responseModel);
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
            productLogger.LogInformation("AssignToShipment, API execution process started at {'" + DateTime.Now + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.ProductAssignToShipment(shipmentRequest);
            if (result.Status && result.StatusCode == 200)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = result.ResponseMessage;
                responseModel.Data = string.Empty;
                productLogger.LogInformation("AssignToShipment, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("AssignToShipment, API execution process completed at {'" + DateTime.Now + "'} with status {'" + responseModel.Status + "'}");
                return NotFound(responseModel);
            }

        }

        /// <summary>
        /// Used to get All Product Details
        /// </summary>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route(ConstantResources.GetAllShipments)]
        public IActionResult GetAllShipments()
        {

            productLogger.LogInformation("GetAllShipments, API execution process started at {'" + DateTime.Now + "'}");
            APIResponseModel<object> responseModel = new APIResponseModel<object>();
            var result = products.GetAllShipmentDetails();
            if (result.Count > 0)
            {
                responseModel.Status = true;
                responseModel.StatusCode = 200;
                responseModel.ResponseMessage = ConstantResources.Success;
                responseModel.Data = result;
                productLogger.LogInformation("GetAllShipments, API execution process completed at {'" + DateTime.Now + "'} " +
                   "with status {'" + responseModel.Status + "'}");
                return Ok(responseModel);
            }
            else
            {
                responseModel.Status = false;
                responseModel.StatusCode = 400;
                responseModel.ResponseMessage = "No Record found!";
                responseModel.Data = result;
                productLogger.LogInformation("GetAllShipments, API execution process completed at {'" + DateTime.Now + "'} " +
                   "with status {'" + responseModel.Status + "'}");
                return NotFound(responseModel);
            }

        }
    }
}
