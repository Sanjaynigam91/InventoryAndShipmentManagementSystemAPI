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
        public readonly static string NoRecordFound = "No product record found";
        public readonly static string NoShipmetFound = "No shipment found";
        public readonly static string Success = "Success";
        public readonly static string InvaidUser = "Invalid User";
        public readonly static string InValidShipmentRequest = "Invalid shipment request";
        #endregion

    }
}
