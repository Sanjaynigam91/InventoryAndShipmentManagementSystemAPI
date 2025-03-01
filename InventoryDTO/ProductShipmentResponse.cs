namespace InventoryDTO
{
    public class ProductShipmentResponse
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public List<ProductShipment> Data { get; set; }

    }
    public class ProductShipment
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int ShipmentId { get; set; }
        public string ShipmentName { get; set; } = string.Empty;
        public DateTime ShipmentDate { get; set; }
        public int Quantity { get; set; }
    }
}
