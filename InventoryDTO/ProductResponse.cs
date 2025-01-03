
namespace InventoryDTO
{
    public class ProductResponse
    {
        public int productId {  get; set; }
        public string productName { get; set; } = string.Empty;
        public int quantity { get; set; }
        public decimal price { get; set; }
        public DateTime createdDate { get; set; }
        public string createdBy {  get; set; }= string.Empty;
    }
}
