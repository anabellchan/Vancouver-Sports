using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace VancouverSports.ViewModel
{
    public class ProductPicked
    {
        
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        [Range(1,100, ErrorMessage = "Qty must be within 1 to 100.")]
        public int QtyOrdered { get; set; }
        public DateTime Updated { get; set; }


        public ProductPicked() {}
        public ProductPicked(int productID, string productName, decimal price, int qtyOrdered)
        {
            ProductID = productID;
            ProductName = productName;
            Price = price;
            QtyOrdered = qtyOrdered;
            Updated = DateTime.Now;
        }

    }
}