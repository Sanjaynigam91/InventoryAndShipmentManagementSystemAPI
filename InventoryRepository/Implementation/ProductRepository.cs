using InventoryRepository.Interface;
using System.Data.Common;
using System.Data;
using System.Net;
using Microsoft.Extensions.Configuration;
using LISCareDataAccess.InventoryDbContext;
using InventoryDTO;
using InventoryUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using InventoryUtility.Interface;
using Microsoft.AspNetCore.Http;
using System.Reflection.PortableExecutable;




namespace InventoryRepository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly InventoryDbContext invDbContext;
        private readonly IProductLoggers productLoggers;

        public ProductRepository(InventoryDbContext dbContext, IProductLoggers productLogger)
        {
            invDbContext = dbContext;
            productLoggers = productLogger;
        }
        /// <summary>
        /// Used to Delete Product Details By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public APIResponseModel<object> DeleteProductDetails(int productId)
        {
            productLoggers.LogInformation("DeleteProduct, Repository operation execution process started at {'" + DateTime.Now + "'} for product Id {'" + productId + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidProductId
            };
            try
            {
                productLoggers.LogInformation("Checking Product Id, Product Id must be greater than zero {'" + productId + "'}");
                if (productId > 0)
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        productLoggers.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for DeleteProduct repository logic.");
                    invDbContext.Database.OpenConnection();
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    command.CommandText = ConstantResources.UspDeleteProduct;
                    productLoggers.LogInformation("{'" + ConstantResources.UspDeleteProduct + "'} getting called at {'" + DateTime.Now + "'}");
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamProductId, productId));
                    // output parameters
                    SqlParameter outputBitParm = new SqlParameter(ConstantResources.ParamIsSuccess, SqlDbType.Bit)
                    {

                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputErrorParm = new SqlParameter(ConstantResources.ParamIsError, SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputErrorMessageParm = new SqlParameter(ConstantResources.ParamErrorMsg, SqlDbType.NVarChar, 404)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputBitParm);
                    command.Parameters.Add(outputErrorParm);
                    command.Parameters.Add(outputErrorMessageParm);
                    command.ExecuteScalar();
                    OutputParameterModel parameterModel = new OutputParameterModel
                    {
                        ErrorMessage = Convert.ToString(outputErrorMessageParm.Value),
                        IsError = outputErrorParm.Value as bool? ?? default,
                        IsSuccess = outputBitParm.Value as bool? ?? default,
                    };
                    if (parameterModel.IsSuccess)
                    {
                        response.Status = true;
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.ResponseMessage = parameterModel.ErrorMessage;
                        response.Data = string.Empty;
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.Status = false;
                        response.ResponseMessage = parameterModel.ErrorMessage;
                        response.Data = string.Empty;
                    }
                }
                else
                {
                    productLoggers.LogInformation("Product Id must be greater than zero {'" + productId + "'}");
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.ResponseMessage = ("Invalid ProductId, ProductId {'" + productId + "'} must be greater than zero");
                    response.Data = string.Empty;

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = false;
                response.ResponseMessage = "{'" + ex + "'},An error occurred while deleting product with Product Id {'" + productId + "'}";
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'},An error occurred while deleting product with Product Id {'" + productId + "'}");
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for DeleteProduct repository logic");
            }
            productLoggers.LogInformation("DeleteProduct, Repository operation execution process completed at {'" + DateTime.Now + "'} for product Id {'" + productId + "'} with status code {'" + response.StatusCode + "'} for DeleteProduct repository logic");
            return response;
        }
        /// <summary>
        /// Used for Get All Products
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public APIResponseModel<object> GetAllProducts()
        {
            productLoggers.LogInformation("GetAllProducts, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var allProducts = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                ResponseMessage = ConstantResources.NoRecordFound
            };

            List<ProductResponse> response = new List<ProductResponse>();
            try
            {
                if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                    invDbContext.Database.OpenConnection();
                productLoggers.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for GetAllProducts repository logic.");
                var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                productLoggers.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                cmd.CommandText = ConstantResources.UspGetAllProducts;
                productLoggers.LogInformation("{'" + ConstantResources.UspGetAllProducts + "'} getting called at {'" + DateTime.Now + "'}");
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ProductResponse productResponse = new ProductResponse();
                    productResponse.productId = reader[ConstantResources.ProductId] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.ProductId]) : 0;
                    productResponse.productName = reader[ConstantResources.ProductName] != DBNull.Value ? Convert.ToString(reader[ConstantResources.ProductName]) : string.Empty;
                    productResponse.quantity = reader[ConstantResources.Quantity] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.Quantity]) : 0;
                    productResponse.price = reader[ConstantResources.Price] != DBNull.Value ? Convert.ToDecimal(reader[ConstantResources.Price]) : 0;
                    productResponse.createdDate = reader[ConstantResources.CreatedDate] != DBNull.Value ? Convert.ToDateTime(reader[ConstantResources.CreatedDate]) : DateTime.Now;
                    productResponse.createdBy = reader[ConstantResources.CreatedBy] != DBNull.Value ? Convert.ToString(reader[ConstantResources.CreatedBy]) : string.Empty;
                    response.Add(productResponse);
                }
                allProducts.Data = response;
                allProducts.StatusCode = (int)HttpStatusCode.OK;
                allProducts.Status = true;
                allProducts.ResponseMessage = ConstantResources.Success;
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred products while fetching all productd details in Product Repository under GetAllProducts method at {'" + DateTime.Now + "'}");
                allProducts.Data = string.Empty;
                allProducts.StatusCode = (int)HttpStatusCode.NotFound;
                allProducts.Status = false;
                allProducts.ResponseMessage = "{'" + ex + "'},An error occurred products while fetching all productd details in Product Repository under GetAllProducts method at {'" + DateTime.Now + "'}";
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for GetAllProducts repository logic.");
            }
            productLoggers.LogInformation("GetAllProducts, Repository operation execution process completed at {'" + DateTime.Now + "'}  with total product count {'" + response.Count() + "'}");
            return allProducts;
        }
        /// <summary>
        /// Used for Get All Product By Id
        /// </summary>
        /// <returns></returns>
        public APIResponseModel<object> GetProductById(int productId)
        {
            productLoggers.LogInformation("GetProductById, Repository operation execution process started at {'" + DateTime.Now + "'} for the product Id {'" + productId + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidProductId
            };
            ProductResponse productResponse = new ProductResponse();
            try
            {
                productLoggers.LogInformation("Checking Product Id, Product Id must be greater than zero {'" + productId + "'}");
                if (productId > 0)
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        invDbContext.Database.OpenConnection();
                    productLoggers.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for GetProductById repository logic.");
                    var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    cmd.CommandText = ConstantResources.UspGetProductDetailsById;
                    productLoggers.LogInformation("{'" + ConstantResources.UspGetProductDetailsById + "'} getting called at {'" + DateTime.Now + "'}");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(ConstantResources.ParamProductId, productId));
                    DbDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {

                        productResponse.productId = reader[ConstantResources.ProductId] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.ProductId]) : 0;
                        productResponse.productName = reader[ConstantResources.ProductName] != DBNull.Value ? Convert.ToString(reader[ConstantResources.ProductName]) : string.Empty;
                        productResponse.quantity = reader[ConstantResources.Quantity] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.Quantity]) : 0;
                        productResponse.price = reader[ConstantResources.Price] != DBNull.Value ? Convert.ToDecimal(reader[ConstantResources.Price]) : 0;
                        productResponse.createdDate = reader[ConstantResources.CreatedDate] != DBNull.Value ? Convert.ToDateTime(reader[ConstantResources.CreatedDate]) : DateTime.Now;
                        productResponse.createdBy = reader[ConstantResources.CreatedBy] != DBNull.Value ? Convert.ToString(reader[ConstantResources.CreatedBy]) : string.Empty;
                    }
                    response.Status = true;
                    response.StatusCode = (int)HttpStatusCode.OK;
                    response.ResponseMessage = ConstantResources.Success;
                    response.Data = productResponse;
                }
                else
                {
                    productLoggers.LogInformation("Product Id must be greater than zero {'" + productId + "'}");
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Status = false;
                    response.ResponseMessage = "Product Id must be greater than zero {'" + productId + "'}";
                    response.Data = string.Empty;
                }

            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'},An error occurred products while fetching product details by product Id {'" + productId + "'} in Product Repository under GetProductById method at {'" + DateTime.Now + "'}");
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Status = false;
                response.ResponseMessage = "{'" + ex + "'},An error occurred products while fetching product details by product Id {'" + productId + "'} in Product Repository under GetProductById method at {'" + DateTime.Now + "'}";
                response.Data = string.Empty;
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for GetProductById repository logic.");
            }
            productLoggers.LogInformation("GetProductById, Repository operation execution process completed at {'" + DateTime.Now + "'} for the product Id {'" + productId + "'}");

            return response;
        }
        /// <summary>
        /// Used for Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public APIResponseModel<object> SaveProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation(ConstantResources.SaveProductDetailsRepoStart);
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidRequest
            };
            try
            {
                if (string.IsNullOrEmpty(productRequest.ProductName) || productRequest.Price == 0 || productRequest.Quantity == 0)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.ResponseMessage = ConstantResources.InValidRequest;
                    response.Data = string.Empty;
                }
                else
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        invDbContext.Database.GetDbConnection().Open();
                    productLoggers.LogInformation(ConstantResources.DBConnectionForSaveProductDetails);
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation(ConstantResources.GetDBConnection);
                    command.CommandText = ConstantResources.UspSaveProductDetails;
                    productLoggers.LogInformation("{'" + ConstantResources.UspSaveProductDetails + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamProductName, productRequest.ProductName));
                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamQuantity, productRequest.Quantity));
                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamPrice, productRequest.Price));
                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamCreatedBy, productRequest.CreatedBy));

                    // output parameters
                    SqlParameter outputBitParm = new SqlParameter(ConstantResources.ParamIsSuccess, SqlDbType.Bit)
                    {

                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputErrorParm = new SqlParameter(ConstantResources.ParamIsError, SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputErrorMessageParm = new SqlParameter(ConstantResources.ParamErrorMsg, SqlDbType.NVarChar, 404)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputBitParm);
                    command.Parameters.Add(outputErrorParm);
                    command.Parameters.Add(outputErrorMessageParm);
                    command.ExecuteScalar();
                    OutputParameterModel parameterModel = new OutputParameterModel
                    {
                        ErrorMessage = Convert.ToString(outputErrorMessageParm.Value),
                        IsError = outputErrorParm.Value as bool? ?? default,
                        IsSuccess = outputBitParm.Value as bool? ?? default,
                    };

                    if (parameterModel.IsSuccess)
                    {
                        response.Status = true;
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.ResponseMessage = parameterModel.ErrorMessage;
                        response.Data = string.Empty;
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.Status = false;
                        response.ResponseMessage = ConstantResources.InValidRequest;
                        response.Data = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = false;
                response.ResponseMessage = "{'" + ex + "'}" + " " + ConstantResources.ExecptionOnSavingProduct;
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'}" + " " + ConstantResources.ExecptionOnSavingProduct);

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForSaveProduct);
            }
            productLoggers.LogInformation(ConstantResources.SaveProductDetailsRepoComplete + response.StatusCode);
            return response;
        }
        /// <summary>
        /// Used for Update Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public APIResponseModel<object> UpdateProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation("UpdateProductDetails, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidRequest
            };
            try
            {
                productLoggers.LogInformation("Checking Product Id, Product Id must be greater than zero {'" + productRequest.ProductId + "'}");
                if (productRequest.ProductId > 0)
                {
                    if (!string.IsNullOrEmpty(productRequest.ToString()))
                    {
                        if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                            invDbContext.Database.GetDbConnection().Open();
                        productLoggers.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for UpdateProductDetails repository logic.");
                        var command = invDbContext.Database.GetDbConnection().CreateCommand();
                        productLoggers.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                        command.CommandText = ConstantResources.UspUpdateProductDetails;
                        productLoggers.LogInformation("{'" + ConstantResources.UspUpdateProductDetails + "'} getting called at {'" + DateTime.Now + "'}");
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter(ConstantResources.ParamProductId, productRequest.ProductId));
                        command.Parameters.Add(new SqlParameter(ConstantResources.ParamProductName, productRequest.ProductName));
                        command.Parameters.Add(new SqlParameter(ConstantResources.ParamQuantity, productRequest.Quantity));
                        command.Parameters.Add(new SqlParameter(ConstantResources.ParamPrice, productRequest.Price));
                        command.Parameters.Add(new SqlParameter(ConstantResources.ParamCreatedBy, productRequest.CreatedBy));

                        // output parameters
                        SqlParameter outputBitParm = new SqlParameter(ConstantResources.ParamIsSuccess, SqlDbType.Bit)
                        {

                            Direction = ParameterDirection.Output
                        };
                        SqlParameter outputErrorParm = new SqlParameter(ConstantResources.ParamIsError, SqlDbType.Bit)
                        {
                            Direction = ParameterDirection.Output
                        };
                        SqlParameter outputErrorMessageParm = new SqlParameter(ConstantResources.ParamErrorMsg, SqlDbType.NVarChar, 404)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputBitParm);
                        command.Parameters.Add(outputErrorParm);
                        command.Parameters.Add(outputErrorMessageParm);
                        command.ExecuteScalar();
                        OutputParameterModel parameterModel = new OutputParameterModel
                        {
                            ErrorMessage = Convert.ToString(outputErrorMessageParm.Value),
                            IsError = outputErrorParm.Value as bool? ?? default,
                            IsSuccess = outputBitParm.Value as bool? ?? default,
                        };

                        if (parameterModel.IsSuccess)
                        {
                            response.Status = true;
                            response.StatusCode = (int)HttpStatusCode.OK;
                            response.ResponseMessage = parameterModel.ErrorMessage;
                            response.Data = string.Empty;
                        }
                        else
                        {
                            response.StatusCode = (int)HttpStatusCode.BadRequest;
                            response.Status = false;
                            response.ResponseMessage = parameterModel.ErrorMessage;
                            response.Data = string.Empty;
                        }
                    }
                }
                else
                {
                    productLoggers.LogInformation("Product Id must be greater than zero {'" + productRequest.ProductId + "'}");
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.ResponseMessage = ConstantResources.InValidProductId;
                    response.Data = string.Empty;
                }

            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = false;
                response.ResponseMessage = "{'" + ex + "'},An error occurred while updating product details";
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'},An error occurred while updating product details");
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for UpdateProductDetails repository logic");
            }
            productLoggers.LogInformation("UpdateProductDetails, Repository operation execution process completed at {'" + DateTime.Now + "'} with status code {'" + response.StatusCode + "'}");
            return response;
        }
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            productLoggers.LogInformation("ProductAssignToShipment, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidShipmentRequest,
                Data = string.Empty
            };
            try
            {
                if (string.IsNullOrEmpty(shipmentRequest.ShipmentName) || shipmentRequest.ProductId <= 0 || shipmentRequest.Quantity <= 0)
                {
                    response.Status = false;
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ResponseMessage = ConstantResources.InValidShipmentRequest;
                    response.Data = string.Empty;

                }
                else
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        invDbContext.Database.GetDbConnection().Open();
                    productLoggers.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for ProductAssignToShipment repository logic.");
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    command.CommandText = ConstantResources.UspAssignProductToShipment;
                    productLoggers.LogInformation("{'" + ConstantResources.UspSaveProductDetails + "'} getting called at {'" + DateTime.Now + "'}");
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamProductId, shipmentRequest.ProductId));
                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamQuantity, shipmentRequest.Quantity));
                    command.Parameters.Add(new SqlParameter(ConstantResources.ParamShipmentName, shipmentRequest.ShipmentName));

                    // output parameters
                    SqlParameter outputBitParm = new SqlParameter(ConstantResources.ParamIsSuccess, SqlDbType.Bit)
                    {

                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputErrorParm = new SqlParameter(ConstantResources.ParamIsError, SqlDbType.Bit)
                    {
                        Direction = ParameterDirection.Output
                    };
                    SqlParameter outputErrorMessageParm = new SqlParameter(ConstantResources.ParamErrorMsg, SqlDbType.NVarChar, 404)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputBitParm);
                    command.Parameters.Add(outputErrorParm);
                    command.Parameters.Add(outputErrorMessageParm);
                    command.ExecuteScalar();
                    OutputParameterModel parameterModel = new OutputParameterModel
                    {
                        ErrorMessage = Convert.ToString(outputErrorMessageParm.Value),
                        IsError = outputErrorParm.Value as bool? ?? default,
                        IsSuccess = outputBitParm.Value as bool? ?? default,
                    };
                    if (parameterModel.IsSuccess)
                    {
                        response.Status = true;
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.ResponseMessage = parameterModel.ErrorMessage;
                        response.Data = string.Empty;
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        response.Status = false;
                        response.ResponseMessage = ConstantResources.InValidShipmentRequest;
                        response.Data = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = false;
                response.ResponseMessage = ex.Message;
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'},An error occurred while {'" + shipmentRequest.ProductId + "'} product assign to shipment");

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for ProductAssignToShipment repository logic");
            }
            productLoggers.LogInformation("ProductAssignToShipment, Repository operation execution process completed at {'" + DateTime.Now + "'} with status code {'" + response.StatusCode + "'}");
            return response;
        }
        /// <summary>
        /// Used for Get All Product shipment history 
        /// </summary>
        /// <returns></returns>
        public APIResponseModel<object> GetAllShipmentDetails()
        {
            productLoggers.LogInformation("GetAllShipmentDetails, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var shipmentResponse = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                ResponseMessage = ConstantResources.NoShipmetFound
            };
            List<ProductShipmentResponse> response = new List<ProductShipmentResponse>();
            try
            {
                if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                    invDbContext.Database.OpenConnection();
                productLoggers.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for GetAllShipmentDetails repository logic.");
                var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                productLoggers.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                cmd.CommandText = ConstantResources.UspGetProductShipmentDetails;
                productLoggers.LogInformation("{'" + ConstantResources.UspGetAllProducts + "'} getting called at {'" + DateTime.Now + "'}");
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    ProductShipmentResponse productShipment = new ProductShipmentResponse();
                    productShipment.ProductId = reader[ConstantResources.ProductId] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.ProductId]) : 0;
                    productShipment.ProductName = reader[ConstantResources.ProductName] != DBNull.Value ? Convert.ToString(reader[ConstantResources.ProductName]) : string.Empty;
                    productShipment.ShipmentId = reader[ConstantResources.ShipmentId] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.ShipmentId]) : 0;
                    productShipment.ShipmentName = reader[ConstantResources.ShipmentName] != DBNull.Value ? Convert.ToString(reader[ConstantResources.ShipmentName]) : string.Empty;
                    productShipment.ShipmentDate = reader[ConstantResources.ShipmentDate] != DBNull.Value ? Convert.ToDateTime(reader[ConstantResources.ShipmentDate]) : DateTime.Now;
                    productShipment.Quantity = reader[ConstantResources.Quantity] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.Quantity]) : 0;
                    response.Add(productShipment);
                }
                shipmentResponse.Data = response;
                shipmentResponse.StatusCode = (int)HttpStatusCode.OK;
                shipmentResponse.Status = true;
                shipmentResponse.ResponseMessage = ConstantResources.Success;
            }
            catch (Exception ex)
            {
                shipmentResponse.Data = string.Empty;
                shipmentResponse.StatusCode = (int)HttpStatusCode.NotFound;
                shipmentResponse.Status = true;
                shipmentResponse.ResponseMessage = "{'" + ex + "'},An error occurred products while fetching all productd details in Product Repository under GetAllShipmentDetails method at {'" + DateTime.Now + "'}";
                productLoggers.LogInformation("{'" + ex + "'},An error occurred products while fetching all productd details in Product Repository under GetAllShipmentDetails method at {'" + DateTime.Now + "'}");

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for GetAllShipmentDetails repository logic.");
            }
            productLoggers.LogInformation("GetAllShipmentDetails, Repository operation execution process completed at {'" + DateTime.Now + "'}  with total product count {'" + response.Count() + "'}");
            return shipmentResponse;
        }
    }
}
