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
        public string ShipToFirstName { get; set; } = null!;
        public string ShipToLastName { get; set; } = null!;
        public string? ShipToCity { get; set; }
        public string? ShipToState { get; set; }
        public string? ShipToZip { get; set; }
        public string? UserId { get; set; }
        public string? ShipToAddress { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
