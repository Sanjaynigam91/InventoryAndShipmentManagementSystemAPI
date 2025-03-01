
namespace InventoryDTO
{
    public class ProductDataResponse
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public List<ProductResponse> Data { get; set; }

    }
    public class ProductResponse
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; } = string.Empty;

    }
}
