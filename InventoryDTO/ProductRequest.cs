
namespace InventoryDTO
{
    public class ProductRequest
    {
        public int ProductId { get; set; }
        public string? ProductName {  get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set;}
        public DateTime? CreatedDate { get; set; } = default(DateTime?);
        public string? CreatedBy { get; set; }

    }
}
