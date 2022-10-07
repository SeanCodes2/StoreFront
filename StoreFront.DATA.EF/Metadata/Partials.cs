using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.DATA.EF.Models//Metadata
{
    //internal class Partials
    //{
    //}

    [ModelMetadataType(typeof(CategoryMetadata))]
    public partial class Category { }

    [ModelMetadataType(typeof(CustomerMetadata))]
    public partial class Customer { }

    [ModelMetadataType(typeof(ManufacturerMetadata))]
    public partial class Manufacturer { }

    [ModelMetadataType(typeof(OrderMetadata))]
    public partial class Order { }

    [ModelMetadataType(typeof(ProductMetadata))]
    public partial class Product 
    {
        [NotMapped]
        public IFormFile? Image { get; set; }
    }



}
