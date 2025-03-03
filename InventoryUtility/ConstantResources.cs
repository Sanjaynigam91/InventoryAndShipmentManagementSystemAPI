using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace InventoryUtility
{
    public class ConstantResources
    {
        public ConstantResources() { }

        #region Route Files and API Name
        public const string APIRoute = "api";
        public const string AddNewProduct = "AddNewProduct";
        public const string UpdateProduct = "UpdateProduct";
        public const string GetProductById = "GetProductById";
        public const string GetAllProducts = "GetAllProducts";
        public const string DeleteProduct = "DeleteProduct";
        public const string AssignToShipment = "AssignToShipment";
        public const string GetAllShipments = "GetAllShipments";


        #endregion

        #region Constant Values
        public const string InventoryDbConnection = "InventoryDbConnection";
        public const string ConnectionStrings = "ConnectionStrings:InventoryDbConnection";
        public const string DataSource = "Data Source=SANJAY-NIGAM\\SQLEXPRESS;Initial Catalog=IASMS;Integrated Security=true;TrustServerCertificate=True;";
        public const string ProductId = "ProductId";
        public const string ProductName = "ProductName";
        public const string Quantity = "Quantity";
        public const string Price = "Price";
        public const string CreatedDate = "CreatedDate";
        public const string CreatedBy = "CreatedBy";
        public const string ShipmentId = "ShipmentId";
        public const string ShipmentName = "ShipmentName";
        public const string ShipmentDate = "ShipmentDate";

        #endregion

        #region Sql Parameter
        public const string ParamProductId = "@ProductId";
        public const string ParamProductName = "@ProductName";
        public const string ParamQuantity = "@Quantity";
        public const string ParamPrice = "@Price";
        public const string ParamCreatedBy = "@CreatedBy";
        public const string ParamIsSuccess = "@IsSuccess";
        public const string ParamIsError = "@IsError";
        public const string ParamErrorMsg = "@ErrorMsg";
        public const string ParamShipmentName = "@ShipmentName";


        #endregion

        #region Store Procedure
        public const string UspSaveProductDetails = "Usp_Save_Product_Details";
        public const string UspUpdateProductDetails = "Usp_Update_Product_Details";
        public const string UspGetAllProducts = "Usp_GetAllProducts";
        public const string UspDeleteProduct = "Usp_Delete_Product";
        public const string UspGetProductDetailsById = "Usp_Get_Product_DetailsById";
        public const string UspAssignProductToShipment = "Usp_Assign_ProductToShipment";
        public const string UspGetProductShipmentDetails = "Usp_GetProduct_ShipmentDetails";



        #endregion

        #region API Response
        public readonly static string InValidRequest = "Invalid product request";
        public readonly static string InValidProductId = "Invalid productId, ProductId must be greater than zero";
        public readonly static string NoProductFound = "No product record found for requested productId";
        public readonly static string NoProductsFound = "No product record found!";
        public readonly static string NoShipmetFound = "No shipment found";
        public readonly static string Success = "Success";
        public readonly static string InvaidUser = "Invalid User";
        public readonly static string InValidShipmentRequest = "Invalid shipment request";
        public readonly static string ProductSaveResponseMsg = "Product details has been added successfully!";
        public readonly static string ProductUpdateResponseMsg = "Product details has been updated successfully!";
        public readonly static string ProductDeleteResponseMsg = "Product details has been deleted successfully!";
        public readonly static string NoProductFoundResponseMsg = "This product is not belong to our records!";
        public readonly static string ProductShipedResponse = "Product assigned to shipment successfully!";
        #endregion

        #region Logging Information
        public readonly static string timeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        public readonly static string AddNewProductStart = $"AddNewProduct, API execution process started at  {timeStamp}";
        public readonly static string AddNewProductComplete = $"AddNewProduct, API execution process completed at {timeStamp} with status" + " ";
        public readonly static string SaveProductDetailsBALStart = $"SaveProductDetails, Business operation execution process started at {timeStamp}";
        public readonly static string SaveProductDetailsBALException = $"An error occurred in business operation of SaveProductDetails while saving the product details like" + " ";
        public readonly static string SaveProductDetailsBALComplete = $"SaveProductDetails, Business operation execution process completed at {timeStamp}";
        public readonly static string SaveProductDetailsRepoStart = $"SaveProductDetails, Repository operation execution process started at {timeStamp}";
        public readonly static string DBConnectionForSaveProductDetails = $"Data base connection open at {timeStamp} for SaveProductDetails repository logic.";
        public readonly static string GetDBConnection = $"Getting data base connection at {timeStamp}";
        public readonly static string ExecptionOnSavingProduct = $"An error occurred while saving product details";
        public readonly static string DBConnectionClosedForSaveProduct = $"Data base connection closed at {timeStamp} for SaveProductDetails repository logic.";
        public readonly static string SaveProductDetailsRepoComplete = $"SaveProductDetails, Repository operation execution process completed at {timeStamp} with status code" + " ";
        public readonly static string UpdateProductAPIStart = $"UpdateProduct, API execution process started at {timeStamp} of the product Id" + " ";
        public readonly static string UpdateProductAPIComplete = $"UpdateProduct, API execution process completed at {timeStamp} with status " + " ";
        public readonly static string UpdateProductDetailsBALStart = $"UpdateProductDetails, Business operation execution process started at {timeStamp}";
        public readonly static string UpdateProductDetailsBALException = $"An error occurred in business operation of UpdateProductDetails while updateing the product details of the product Id" + " ";
        public readonly static string UpdateProductDetailsBALComplete = $"UpdateProductDetails, Business operation execution process completed at {timeStamp}";
        public readonly static string UpdateProductDetailsRepoStart = $"UpdateProductDetails, Repository operation execution process started at {timeStamp}";
        public readonly static string CheckingProductId = $"Checking Product Id, Product Id must be greater than zero at {timeStamp}";
        public readonly static string DBConnectionForUpdateProductDetails = $"Data base connection open at {timeStamp} for UpdateProductDetails repository logic.";
        public readonly static string ProductIdGreaterThanZero = $"Product Id must be greater than zero at {timeStamp}" + " and requested productId is ";
        public readonly static string ExceptionWhileUpdatingProductDetails = $"An error occurred while updating product details at {timeStamp}";
        public readonly static string DBConnectionClosedForUpdateProduct = $"Data base connection closed at {timeStamp} for UpdateProductDetails repository logic.";
        public readonly static string UpdateProductDetailsRepoComplete = $"UpdateProductDetails, Repository operation execution process completed at {timeStamp} with status code ";
        public readonly static string GetProductByIdAPIStart = $"GetProductById, API execution process started at {timeStamp} for the product Id is ";
        public readonly static string GetProductByIdAPIComplete = $"GetProductById, API execution process completed at {timeStamp}" + " ";
        public readonly static string WithStatus = "with status" + " ";
        public readonly static string OfProductId = " of product Id" + " ";
        public readonly static string GetProductByIdBALStart = $"GetProductById, Business operation execution process started at {timeStamp}" + " ";
        public readonly static string ForProductId = " for product Id" + " ";
        public readonly static string ExceptionWhileGettingProductById = $"An error occurred in business operation of GetProductById while fetching product details by product Id" + " ";
        public readonly static string GetProductByIdBALComplete = $"GetProductById, Business operation execution process completed at {timeStamp} for product Id" + " ";
        public readonly static string GetProductByIdRepoStart = $"GetProductById, Repository operation execution process started at {timeStamp} for the product Id" + " ";
        public readonly static string DBConnectionForGetProductById = $"Data base connection open at {timeStamp} for GetProductById repository logic.";
        public readonly static string ExceptionWhileGettingProductByIdInRepo = $"An error occurred products while fetching product details by product Id" + " ";
        public readonly static string GetProductByIdMethodAt = $"in Product Repository under GetProductById method at {timeStamp}";
        public readonly static string DBConnectionClosedForGetProductById = $"Data base connection closed at {timeStamp} for GetProductById repository logic.";
        public readonly static string GetProductByIdRepoComplete = $"GetProductById, Repository operation execution process completed at {timeStamp} for the product Id" + " ";
        public readonly static string GetAllProductsAPIStart = $"GetAllProducts, API execution process started at {timeStamp}";
        public readonly static string GetAllProductsAPIComplete = $"GetAllProducts, API execution process completed at {timeStamp}" + " ";
        public readonly static string GetAllProductsBALStart = $"GetAllProducts,Business operation execution process started at {timeStamp}";
        public readonly static string ExceptionWhileGettingAllProductsBAL = $"An error occurred in business operation of GetAllProducts while fetching all products at {timeStamp}";
        public readonly static string GetAllProductsBALComplete = $"GetAllProducts,Business operation execution process completed at {timeStamp}";
        public readonly static string GetAllProductsByIdRepoStart = $"GetAllProducts, Repository operation execution process started at {timeStamp}";
        public readonly static string DBConnectionForGetAllProducts = $"Data base connection open at {timeStamp} for GetAllProducts repository logic.";
        public readonly static string ExceptionWhileGettingAllProductsInRepo = $"An error occurred products while fetching all productd details in Product Repository under GetAllProducts method at {timeStamp} ";
        public readonly static string DBConnectionClosedForGetAllProducts = $"Data base connection closed at {timeStamp} for GetAllProducts repository logic.";
        public readonly static string GetAllProductsByIdRepoComplete = $"GetAllProducts, Repository operation execution process completed at {timeStamp}" + " ";
        public readonly static string WithTotalProductCount = "with total product count";
        public readonly static string DeleteProductAPIStart = $"DeleteProduct, API execution process started at {timeStamp} of the product Id" + " ";
        public readonly static string DeleteProductAPIComplete = $"DeleteProduct, API execution process completed at {timeStamp} with status" + " ";
        public readonly static string DeleteProductBALStart = $"DeleteProduct, Business operation execution process started at {timeStamp} for product Id" + " ";
        public readonly static string ExceptionWhileDeleteProductBAL = $"An error occurred in business operation of DeleteProductDetails while deleting product with product Id" + " ";
        public readonly static string DeleteProductBALComplete = $"DeleteProduct, Business operation execution process completed at {timeStamp} for product Id" + " ";
        public readonly static string DeleteProductRepoStart = $"DeleteProduct, Repository operation execution process started at {timeStamp} for product Id" + " ";
        public readonly static string DBConnectionForDeleteProduct = $"Data base connection open at {timeStamp} for DeleteProduct repository logic.";
        public readonly static string ExceptionWhileDeleteProductRepo = $"An error occurred while deleting product with Product Id" + " ";
        public readonly static string DBConnectionClosedForDeleteProduct = $"Data base connection closed at {timeStamp} for DeleteProduct repository logic.";
        public readonly static string DeleteProductRepoComplete = $"DeleteProduct, Repository operation execution process completed at {timeStamp} for product Id" + " ";
        public readonly static string AssignToShipmentAPIStart = $"AssignToShipment, API execution process started at {timeStamp}";
        public readonly static string AssignToShipmentAPIComplete = $"AssignToShipment, API execution process completed at {timeStamp} with status" + " ";
        public readonly static string AssignToShipmentBALStart = $"ProductAssignToShipment, Business operation execution process started at {timeStamp}";
        public readonly static string ExceptionAssignToShipmentBAL = $"An error occurred in business operation of ProductAssignToShipment while saving the product details like" + " ";
        public readonly static string AssignToShipmentBALComplete = $"ProductAssignToShipment, Business operation execution process completed at {timeStamp}";
        public readonly static string AssignToShipmentRepoStart = $"ProductAssignToShipment, Repository operation execution process started at {timeStamp}";
        public readonly static string DBConnectionForAssignToShipment = $"Data base connection open at {timeStamp} for ProductAssignToShipment repository logic.";
        public readonly static string ExceptionAssignToShipmentRepo = $"An error occurred while assign product for shipment and shipment product Id is" + " ";
        public readonly static string DBConnectionClosedForAssignToShipment = $"Data base connection closed at {timeStamp} for ProductAssignToShipment repository logic.";
        public readonly static string AssignToShipmentRepoComplete = $"ProductAssignToShipment, Repository operation execution process completed at {timeStamp} with status code" + " ";
        public readonly static string GetAllShipmentsAPIStart = $"GetAllShipments, API execution process started at {timeStamp}";
        public readonly static string GetAllShipmentsAPIComplete = $"GetAllShipments, API execution process completed at {timeStamp}" + " ";
        public readonly static string GetAllShipmentsBALStart = $"GetAllShipmentDetails,Business operation execution process started at {timeStamp}";
        public readonly static string ExceptionGetAllShipmentsBAL = $"An error occurred in business operation of GetAllShipmentDetails while fetching all shipment details at {timeStamp}" + " ";
        public readonly static string GetAllShipmentsBAComplete = $"GetAllShipmentDetails,Business operation execution process completed at {timeStamp}";
        public readonly static string GetAllShipmentsRepoStart = $"GetAllShipmentDetails, Repository operation execution process started at {timeStamp}";
        public readonly static string DBConnectionForGetAllShipments = $"Data base connection open at {timeStamp} for GetAllShipmentDetails repository logic.";
        public readonly static string ExceptionGetAllShipmentsRepo = $"An error occurred products while fetching all productd details in Product Repository under GetAllShipmentDetails method at {timeStamp}";
        public readonly static string DBConnectionClosedForGetAllShipments = $"Data base connection closed at {timeStamp} for GetAllShipmentDetails repository logic.";
        public readonly static string GetAllShipmentsRepoComplete = $"GetAllShipmentDetails, Repository operation execution process completed at {timeStamp}  with total product count" + " ";
        #endregion


    }
}
