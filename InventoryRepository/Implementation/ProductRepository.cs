﻿using InventoryRepository.Interface;
using System.Data.Common;
using System.Data;
using System.Net;
using Microsoft.Extensions.Configuration;
using LISCareDataAccess.InventoryDbContext;
using InventoryDTO;
using InventoryUtility;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;




namespace InventoryRepository.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private InventoryDbContext invDbContext;
        private readonly ProductLogger productLogger;

        public ProductRepository(InventoryDbContext dbContext)
        {
            invDbContext = dbContext;
            productLogger = new ProductLogger();
        }
        /// <summary>
        /// Used to Delete Product Details By Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public APIResponseModel<object> DeleteProductDetails(int productId)
        {
            productLogger.LogInformation("DeleteProduct, Repository operation execution process started at {'" + DateTime.Now + "'} for product Id {'" + productId + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = 404,
                Status = false,
                ResponseMessage = ConstantResources.Failed
            };
            try
            {
                productLogger.LogInformation("Checking Product Id, Product Id must be greater than zero {'" + productId + "'}");
                if (productId > 0)
                {
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    command.CommandText = ConstantResources.UspDeleteProduct;
                    productLogger.LogInformation("{'" + ConstantResources.UspDeleteProduct + "'} getting called at {'" + DateTime.Now + "'}");
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
                    invDbContext.Database.GetDbConnection().Open();
                    productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for DeleteProduct repository logic.");
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
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        response.Status = false;
                        response.ResponseMessage = ConstantResources.Failed;

                    }
                }
                else
                {
                    productLogger.LogInformation("Product Id must be greater than zero {'" + productId + "'}");
                    throw new ArgumentException("Product Id must be greater than zero.");

                }
            }
            catch (Exception ex)
            {
                response.ResponseMessage = ex.Message;
                productLogger.LogInformation("{'" + ex + "'},An error occurred while deleting product with Product Id {'" + productId + "'}");
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for DeleteProduct repository logic");
            }
            productLogger.LogInformation("DeleteProduct, Repository operation execution process completed at {'" + DateTime.Now + "'} for product Id {'" + productId + "'} with status code {'" + response.StatusCode + "'} for DeleteProduct repository logic");
            return response;
        }
        /// <summary>
        /// Used for Get All Products
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ProductResponse> GetAllProducts()
        {
            productLogger.LogInformation("GetAllProducts, Repository operation execution process started at {'" + DateTime.Now + "'}");
            List<ProductResponse> response = new List<ProductResponse>();
            try
            {
                if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                    invDbContext.Database.OpenConnection();
                productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for GetAllProducts repository logic.");
                var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                cmd.CommandText = ConstantResources.UspGetAllProducts;
                productLogger.LogInformation("{'" + ConstantResources.UspGetAllProducts + "'} getting called at {'" + DateTime.Now + "'}");
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
            }
            catch (Exception ex)
            {
                productLogger.LogInformation("{'" + ex + "'},An error occurred products while fetching all productd details in Product Repository under GetAllProducts method at {'" + DateTime.Now + "'}");
                throw new Exception("An error occurred while retrieving products.", ex);
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for GetAllProducts repository logic.");
            }
            productLogger.LogInformation("GetAllProducts, Repository operation execution process completed at {'" + DateTime.Now + "'}  with total product count {'" + response.Count() + "'}");
            return response;
        }
        /// <summary>
        /// Used for Get All Product By Id
        /// </summary>
        /// <returns></returns>
        public ProductResponse GetProductById(int productId)
        {
            productLogger.LogInformation("GetProductById, Repository operation execution process started at {'" + DateTime.Now + "'} for the product Id {'" + productId + "'}");
            ProductResponse productResponse = new ProductResponse();
            try
            {
                productLogger.LogInformation("Checking Product Id, Product Id must be greater than zero {'" + productId + "'}");
                if (productId >= 0)
                {
                    if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                        invDbContext.Database.OpenConnection();
                    productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for GetProductById repository logic.");
                    var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    cmd.CommandText = ConstantResources.UspGetProductDetailsById;
                    productLogger.LogInformation("{'" + ConstantResources.UspGetProductDetailsById + "'} getting called at {'" + DateTime.Now + "'}");
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
                }
                else
                {
                    productLogger.LogInformation("Product Id must be greater than zero {'" + productId + "'}");
                    throw new ArgumentException("Product Id must be greater than zero.");
                }

            }
            catch (Exception ex)
            {
                productLogger.LogInformation("{'" + ex + "'},An error occurred products while fetching product details by product Id {'" + productId + "'} in Product Repository under GetProductById method at {'" + DateTime.Now + "'}");
                throw new Exception("An error occurred while retrieving products.", ex);
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for GetProductById repository logic.");
            }
            productLogger.LogInformation("GetProductById, Repository operation execution process completed at {'" + DateTime.Now + "'} for the product Id {'" + productId + "'}");
            return productResponse;
        }
        /// <summary>
        /// Used for Save Product Details
        /// </summary>
        /// <param name="productRequest"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public APIResponseModel<object> SaveProductDetails(ProductRequest productRequest)
        {
            productLogger.LogInformation("SaveProductDetails, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = 404,
                Status = false,
                ResponseMessage = ConstantResources.Failed
            };
            try
            {
                if (!string.IsNullOrEmpty(productRequest.ToString()))
                {
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    command.CommandText = ConstantResources.UspSaveProductDetails;
                    productLogger.LogInformation("{'" + ConstantResources.UspSaveProductDetails + "'} getting called at {'" + DateTime.Now + "'}");
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
                    invDbContext.Database.GetDbConnection().Open();
                    productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for SaveProductDetails repository logic.");
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
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        response.Status = false;
                        response.ResponseMessage = ConstantResources.Failed;

                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseMessage = ex.Message;
                productLogger.LogInformation("{'" + ex + "'},An error occurred while saving product details");

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for SaveProductDetails repository logic");
            }
            productLogger.LogInformation("SaveProductDetails, Repository operation execution process completed at {'" + DateTime.Now + "'} with status code {'" + response.StatusCode + "'}");
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
            productLogger.LogInformation("UpdateProductDetails, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = 404,
                Status = false,
                ResponseMessage = ConstantResources.Failed
            };
            try
            {
                productLogger.LogInformation("Checking Product Id, Product Id must be greater than zero {'" + productRequest.ProductId + "'}");
                if (productRequest.ProductId > 0)
                {
                    if (!string.IsNullOrEmpty(productRequest.ToString()))
                    {
                        var command = invDbContext.Database.GetDbConnection().CreateCommand();
                        productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                        command.CommandText = ConstantResources.UspUpdateProductDetails;
                        productLogger.LogInformation("{'" + ConstantResources.UspUpdateProductDetails + "'} getting called at {'" + DateTime.Now + "'}");
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
                        invDbContext.Database.GetDbConnection().Open();
                        productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for UpdateProductDetails repository logic.");
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
                        }
                        else
                        {
                            response.StatusCode = (int)HttpStatusCode.NotFound;
                            response.Status = false;
                            response.ResponseMessage = ConstantResources.Failed;

                        }
                    }
                }
                else
                {
                    productLogger.LogInformation("Product Id must be greater than zero {'" + productRequest.ProductId + "'}");
                    throw new ArgumentException("Product Id must be greater than zero.");
                }

            }
            catch (Exception ex)
            {
                response.ResponseMessage = ex.Message;
                productLogger.LogInformation("{'" + ex + "'},An error occurred while updating product details");
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for UpdateProductDetails repository logic");
            }
            productLogger.LogInformation("UpdateProductDetails, Repository operation execution process completed at {'" + DateTime.Now + "'} with status code {'" + response.StatusCode + "'}");
            return response;
        }
        /// <summary>
        ///  Used for Product need to assign shipment
        /// </summary>
        /// <param name="shipmentRequest"></param>
        /// <returns></returns>
        public APIResponseModel<object> ProductAssignToShipment(ShipmentRequest shipmentRequest)
        {
            productLogger.LogInformation("ProductAssignToShipment, Repository operation execution process started at {'" + DateTime.Now + "'}");
            var response = new APIResponseModel<object>
            {
                StatusCode = 404,
                Status = false,
                ResponseMessage = ConstantResources.Failed
            };
            try
            {
                if (!string.IsNullOrEmpty(shipmentRequest.ToString()))
                {
                    var command = invDbContext.Database.GetDbConnection().CreateCommand();
                    productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                    command.CommandText = ConstantResources.UspAssignProductToShipment;
                    productLogger.LogInformation("{'" + ConstantResources.UspSaveProductDetails + "'} getting called at {'" + DateTime.Now + "'}");
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
                    invDbContext.Database.GetDbConnection().Open();
                    productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for ProductAssignToShipment repository logic.");
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
                    }
                    else
                    {
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        response.Status = false;
                        response.ResponseMessage = ConstantResources.Failed;

                    }
                }
            }
            catch (Exception ex)
            {
                response.ResponseMessage = ex.Message;
                productLogger.LogInformation("{'" + ex + "'},An error occurred while {'" + shipmentRequest.ProductId + "'} product assign to shipment");

            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for ProductAssignToShipment repository logic");
            }
            productLogger.LogInformation("ProductAssignToShipment, Repository operation execution process completed at {'" + DateTime.Now + "'} with status code {'" + response.StatusCode + "'}");
            return response;
        }
        /// <summary>
        /// Used for Get All Product shipment history 
        /// </summary>
        /// <returns></returns>
        public List<ProductShipmentResponse> GetAllShipmentDetails()
        {
            productLogger.LogInformation("GetAllShipmentDetails, Repository operation execution process started at {'" + DateTime.Now + "'}");
            List<ProductShipmentResponse> response = new List<ProductShipmentResponse>();
            try
            {
                if (invDbContext.Database.GetDbConnection().State == ConnectionState.Closed)
                    invDbContext.Database.OpenConnection();
                productLogger.LogInformation("Data base connection open at {'" + DateTime.Now + "'} for GetAllShipmentDetails repository logic.");
                var cmd = invDbContext.Database.GetDbConnection().CreateCommand();
                productLogger.LogInformation("Getting data base connection at {'" + DateTime.Now + "'}");
                cmd.CommandText = ConstantResources.UspGetProductShipmentDetails;
                productLogger.LogInformation("{'" + ConstantResources.UspGetAllProducts + "'} getting called at {'" + DateTime.Now + "'}");
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
            }
            catch (Exception ex)
            {
                productLogger.LogInformation("{'" + ex + "'},An error occurred products while fetching all productd details in Product Repository under GetAllShipmentDetails method at {'" + DateTime.Now + "'}");
                throw new Exception("An error occurred while retrieving all shipments details.", ex);
            }
            finally
            {
                invDbContext.Database.GetDbConnection().Close();
                productLogger.LogInformation("Data base connection closed at {'" + DateTime.Now + "'} for GetAllShipmentDetails repository logic.");
            }
            productLogger.LogInformation("GetAllShipmentDetails, Repository operation execution process completed at {'" + DateTime.Now + "'}  with total product count {'" + response.Count() + "'}");
            return response;
        }
    }
}
