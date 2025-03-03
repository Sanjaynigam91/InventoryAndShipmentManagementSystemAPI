using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryDTO
{
    public class ProductModel
    {
        public int StatusCode { get; set; }
        public bool Status { get; set; }
        public string ResponseMessage { get; set; } = string.Empty;
        public Product Data { get; set; } 
    }
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }=string.Empty;

    }
}
