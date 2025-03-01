using InventoryRepository.Interface;
using System.Data.Common;
using System.Data;
using System.Net;
using LISCareDataAccess.InventoryDbContext;
using InventoryDTO;
using InventoryUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using InventoryUtility.Interface;

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
        public async Task<APIResponseModel<object>> DeleteProductDetails(int productId)
        {
            productLoggers.LogInformation(ConstantResources.DeleteProductRepoStart + productId);
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidProductId
            };
            try
            {
                productLoggers.LogInformation(ConstantResources.CheckingProductId);
                if (productId > 0)
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        productLoggers.LogInformation(ConstantResources.DBConnectionForDeleteProduct);
                    invDbContext.Database.OpenConnection();
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation(ConstantResources.GetDBConnection);
                    command.CommandText = ConstantResources.UspDeleteProduct;
                    productLoggers.LogInformation("{'" + ConstantResources.UspDeleteProduct + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
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
                    await command.ExecuteScalarAsync();
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
                    productLoggers.LogInformation(ConstantResources.ProductIdGreaterThanZero + productId);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.ResponseMessage = (ConstantResources.ProductIdGreaterThanZero + productId);
                    response.Data = string.Empty;

                }
            }
            catch (Exception ex)
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = false;
                response.ResponseMessage = "{'" + ex + "'}, " + ConstantResources.ExceptionWhileDeleteProductRepo + productId;
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileDeleteProductRepo + productId);
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForDeleteProduct);
            }
            productLoggers.LogInformation(ConstantResources.DeleteProductRepoComplete + productId + ConstantResources.WithStatus + response.Status);
            return response;
        }
        /// <summary>
        /// Used for Get All Products
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<ProductDataResponse> GetAllProducts()
        {
            productLoggers.LogInformation(ConstantResources.GetAllProductsByIdRepoStart);
            var allProducts = new ProductDataResponse
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Status = false,
                ResponseMessage = ConstantResources.NoProductsFound
            };

            List<ProductResponse> response = new List<ProductResponse>();
            try
            {
                if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                    invDbContext.Database.OpenConnection();
                productLoggers.LogInformation(ConstantResources.DBConnectionForGetAllProducts);
                var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                productLoggers.LogInformation(ConstantResources.GetDBConnection);
                cmd.CommandText = ConstantResources.UspGetAllProducts;
                productLoggers.LogInformation("{'" + ConstantResources.UspGetAllProducts + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader reader = cmd.ExecuteReader();
                while (await reader.ReadAsync())
                {
                    ProductResponse productResponse = new ProductResponse();
                    productResponse.ProductId = reader[ConstantResources.ProductId] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.ProductId]) : 0;
                    productResponse.ProductName = reader[ConstantResources.ProductName] != DBNull.Value ? Convert.ToString(reader[ConstantResources.ProductName]) : string.Empty;
                    productResponse.Quantity = reader[ConstantResources.Quantity] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.Quantity]) : 0;
                    productResponse.Price = reader[ConstantResources.Price] != DBNull.Value ? Convert.ToDecimal(reader[ConstantResources.Price]) : 0;
                    productResponse.CreatedDate = reader[ConstantResources.CreatedDate] != DBNull.Value ? Convert.ToDateTime(reader[ConstantResources.CreatedDate]) : DateTime.Now;
                    productResponse.CreatedBy = reader[ConstantResources.CreatedBy] != DBNull.Value ? Convert.ToString(reader[ConstantResources.CreatedBy]) : string.Empty;
                    response.Add(productResponse);
                }
                if (response.Count > 0)
                {
                    allProducts.Data = response;
                    allProducts.StatusCode = (int)HttpStatusCode.OK;
                    allProducts.Status = true;
                    allProducts.ResponseMessage = ConstantResources.Success;
                }
                else
                {
                    allProducts.StatusCode = (int)HttpStatusCode.NotFound;
                    allProducts.Status = false;
                    allProducts.ResponseMessage = ConstantResources.NoProductsFound;
                }

            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileGettingAllProductsInRepo);
                allProducts.StatusCode = (int)HttpStatusCode.BadRequest;
                allProducts.Status = false;
                allProducts.ResponseMessage = "{'" + ex + "'}, " + ConstantResources.ExceptionWhileGettingAllProductsInRepo;
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForGetAllProducts);
            }
            productLoggers.LogInformation(ConstantResources.GetAllProductsByIdRepoComplete + ConstantResources.WithTotalProductCount + response.Count);
            return allProducts;
        }
        /// <summary>
        /// Used for Get All Product By Id
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponseModel<object>> GetProductById(int productId)
        {
            productLoggers.LogInformation(ConstantResources.GetProductByIdRepoStart + productId);
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidProductId
            };
            ProductResponse productResponse = new ProductResponse();
            try
            {
                productLoggers.LogInformation(ConstantResources.CheckingProductId);
                if (productId > 0)
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        invDbContext.Database.OpenConnection();
                    productLoggers.LogInformation(ConstantResources.DBConnectionForGetProductById);
                    var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation(ConstantResources.GetDBConnection);
                    cmd.CommandText = ConstantResources.UspGetProductDetailsById;
                    productLoggers.LogInformation("{'" + ConstantResources.UspGetProductDetailsById + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter(ConstantResources.ParamProductId, productId));
                    DbDataReader reader = cmd.ExecuteReader();
                    while (await reader.ReadAsync())
                    {

                        productResponse.ProductId = reader[ConstantResources.ProductId] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.ProductId]) : 0;
                        productResponse.ProductName = reader[ConstantResources.ProductName] != DBNull.Value ? Convert.ToString(reader[ConstantResources.ProductName]) : string.Empty;
                        productResponse.Quantity = reader[ConstantResources.Quantity] != DBNull.Value ? Convert.ToInt32(reader[ConstantResources.Quantity]) : 0;
                        productResponse.Price = reader[ConstantResources.Price] != DBNull.Value ? Convert.ToDecimal(reader[ConstantResources.Price]) : 0;
                        productResponse.CreatedDate = reader[ConstantResources.CreatedDate] != DBNull.Value ? Convert.ToDateTime(reader[ConstantResources.CreatedDate]) : DateTime.Now;
                        productResponse.CreatedBy = reader[ConstantResources.CreatedBy] != DBNull.Value ? Convert.ToString(reader[ConstantResources.CreatedBy]) : string.Empty;
                    }
                    if (productResponse.ProductId > 0)
                    {
                        response.Status = true;
                        response.StatusCode = (int)HttpStatusCode.OK;
                        response.ResponseMessage = ConstantResources.Success;
                        response.Data = productResponse;
                    }
                    else
                    {
                        response.Status = false;
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        response.ResponseMessage = ConstantResources.NoProductFound + " which is " + productId;
                        response.Data = string.Empty;
                    }
                }
                else
                {
                    productLoggers.LogInformation(ConstantResources.ProductIdGreaterThanZero + productId);
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Status = false;
                    response.ResponseMessage = ConstantResources.ProductIdGreaterThanZero + productId;
                    response.Data = string.Empty;
                }
            }
            catch (Exception ex)
            {
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileGettingProductByIdInRepo + productId + ConstantResources.GetProductByIdMethodAt);
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Status = false;
                response.ResponseMessage = "{'" + ex + "'}, " + ConstantResources.ExceptionWhileGettingProductByIdInRepo + productId + ConstantResources.GetProductByIdMethodAt;
                response.Data = string.Empty;
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForGetProductById);
            }
            productLoggers.LogInformation(ConstantResources.GetProductByIdRepoComplete + productId);
            return response;
        }
        /// <summary>
        /// Used for Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<APIResponseModel<object>> SaveProductDetails(ProductRequest productRequest)
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
                    await command.ExecuteScalarAsync();
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
                response.ResponseMessage = "{'" + ex + "'}, " + " " + ConstantResources.ExecptionOnSavingProduct;
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'}, " + " " + ConstantResources.ExecptionOnSavingProduct);

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
        public async Task<APIResponseModel<object>> UpdateProductDetails(ProductRequest productRequest)
        {
            productLoggers.LogInformation(ConstantResources.UpdateProductDetailsRepoStart);
            var response = new APIResponseModel<object>
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Status = false,
                ResponseMessage = ConstantResources.InValidRequest
            };
            try
            {
                productLoggers.LogInformation(ConstantResources.CheckingProductId);
                if (productRequest.ProductId > 0)
                {
                    if (!string.IsNullOrEmpty(productRequest.ToString()))
                    {
                        if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                            invDbContext.Database.GetDbConnection().Open();
                        productLoggers.LogInformation(ConstantResources.DBConnectionForUpdateProductDetails);
                        var command = invDbContext.Database.GetDbConnection().CreateCommand();
                        productLoggers.LogInformation(ConstantResources.GetDBConnection);
                        command.CommandText = ConstantResources.UspUpdateProductDetails;
                        productLoggers.LogInformation("{'" + ConstantResources.UspUpdateProductDetails + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
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
                        await command.ExecuteScalarAsync();
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
                    productLoggers.LogInformation(ConstantResources.ProductIdGreaterThanZero + productRequest.ProductId);
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
                response.ResponseMessage = "{'" + ex + "'}, " + ConstantResources.ExceptionWhileUpdatingProductDetails;
                response.Data = string.Empty;
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionWhileUpdatingProductDetails);
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForUpdateProduct);
            }
            productLoggers.LogInformation(ConstantResources.UpdateProductDetailsRepoComplete + response.StatusCode);
            return response;
        }
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        public async Task<APIResponseModel<object>> ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            productLoggers.LogInformation(ConstantResources.AssignToShipmentRepoStart);
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
                    productLoggers.LogInformation(ConstantResources.DBConnectionForAssignToShipment);
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLoggers.LogInformation(ConstantResources.GetDBConnection);
                    command.CommandText = ConstantResources.UspAssignProductToShipment;
                    productLoggers.LogInformation("{'" + ConstantResources.UspSaveProductDetails + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
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
                    await command.ExecuteScalarAsync();
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
                productLoggers.LogInformation("{'" + ex + "'}, " + ConstantResources.ExceptionAssignToShipmentRepo + shipmentRequest.ProductId);

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForAssignToShipment);
            }
            productLoggers.LogInformation(ConstantResources.AssignToShipmentRepoComplete + response.Status);
            return response;
        }
        /// <summary>
        /// Used for Get All Product shipment history 
        /// </summary>
        /// <returns></returns>
        public async Task<APIResponseModel<object>> GetAllShipmentDetails()
        {
            productLoggers.LogInformation(ConstantResources.GetAllShipmentsRepoStart);
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
                productLoggers.LogInformation(ConstantResources.DBConnectionForGetAllShipments);
                var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                productLoggers.LogInformation(ConstantResources.GetDBConnection);
                cmd.CommandText = ConstantResources.UspGetProductShipmentDetails;
                productLoggers.LogInformation("{'" + ConstantResources.UspGetAllProducts + "'} getting called at {'" + ConstantResources.timeStamp + "'}");
                cmd.CommandType = CommandType.StoredProcedure;
                DbDataReader reader = cmd.ExecuteReader();
                while (await reader.ReadAsync())
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
                if (response.Count > 0)
                {
                    shipmentResponse.Data = response;
                    shipmentResponse.StatusCode = (int)HttpStatusCode.OK;
                    shipmentResponse.Status = true;
                    shipmentResponse.ResponseMessage = ConstantResources.Success;
                }
                else
                {
                    shipmentResponse.Data = response;
                    shipmentResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    shipmentResponse.Status = false;
                    shipmentResponse.ResponseMessage = ConstantResources.NoShipmetFound;
                }

            }
            catch (Exception ex)
            {
                shipmentResponse.Data = string.Empty;
                shipmentResponse.StatusCode = (int)HttpStatusCode.BadRequest;
                shipmentResponse.Status = true;
                shipmentResponse.ResponseMessage = "{'" + ex + "'}," + ConstantResources.ExceptionGetAllShipmentsRepo;
                productLoggers.LogInformation("{'" + ex + "'}," + ConstantResources.ExceptionGetAllShipmentsRepo);

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLoggers.LogInformation(ConstantResources.DBConnectionClosedForGetAllShipments);
            }
            productLoggers.LogInformation(ConstantResources.GetAllShipmentsRepoComplete + response.Count);
            return shipmentResponse;
        }
    }
}
