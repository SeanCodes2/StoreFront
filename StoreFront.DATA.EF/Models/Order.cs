using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Order
    {
        public Order()
        {
            ProductOrders = new HashSet<ProductOrder>();
        }

        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime ShipDate { get; set; }
        public int ItemsSold { get; set; }
        public decimal OrderAmoutTotal { get; set; }

        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<ProductOrder> ProductOrders { get; set; }
    }
}
