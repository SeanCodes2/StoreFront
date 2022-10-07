using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public int ManufacturerId { get; set; }
        public string ProductName { get; set; } = null!;
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int UnitsInStock { get; set; }
        public int? UnitsOnOrder { get; set; }
        public bool Discontinued { get; set; }
        public string? ProductImage { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
