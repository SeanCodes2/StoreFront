using System;
using System.Collections.Generic;

namespace StoreFront.DATA.EF.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string ShipToName { get; set; } = null!;
        public string? ShipToCity { get; set; }
        public string? ShipToState { get; set; }
        public string? ShipToZip { get; set; }
        public string? UserId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
