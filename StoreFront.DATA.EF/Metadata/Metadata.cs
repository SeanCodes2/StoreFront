using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.DATA.EF.Models//Metadata
{
    //internal class Metadata
    //{
    //}
    #region COMMON METADATA ATTRIBUTES
    /*
        * * COMMON METADATA ATTRIBUTES
        * 
        * VALIDATION:
        * [Required(ErrorMessage = "MessageHere")]
        *   - forces us to provide a value for this property
        * [StringLength(##, ErrorMessage = "MessageHere")]
        *   - sets the maximum number of characters a user can enter for a value
        * [DataType(DataType.SomeType)]
        *   - forces default validation based on 'SomeType' that is selected
        *   - options include: (PostalCode, PhoneNumber, EmailAddress, etc.)
        * [Range(minValue#, maxValue#)]
        *   - only allows input between the specified min and max values
        * 
        * DISPLAY:
        * [Display(Name = "Name goes here")]
        *  - Updates the label for a property - initially tied to table header values
        * [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true/false)]
        *  - Chages the way values are presented in a View
        *  - You can replace the placeholder with your preferred formatting ({0:c}, {0:n2}... etc)
        *  - ApplyFormatInEditMode will forcibly validate the string formatting in text boxes
        *      - IF this is TRUE:
        *          - When editing or creating a record with formatting like {0:d}, you must input the date
        *          as "MM/dd/yyyy"
        *          - When editing/creating a record with formatting like {0:c}, you must input the value
        *          as $5.99
        *      - IF this is FALSE:
        *          - Special formatting characters are not required when editing/creating         *   
        */
    #endregion

    #region nothing
    //sdfg

    #endregion


    public class CategoryMetadata
    {
        
        public int CategoryId { get; set; }

        [StringLength(100, ErrorMessage ="*Cannot Exceed 100 Characters")]
        [Required(ErrorMessage ="*Category is Required")]
        [Display(Name ="Category")]
        public string CategoryName { get; set; } = null!;

        [StringLength(500, ErrorMessage ="*Cannot Exceed 500 Characters")]
        [DataType(DataType.MultilineText)]
        [Display(Name ="Description")]
        public string? CategoryDescription { get; set; }
    }

    
    public class CustomerMetadata
    {
        public int CustomerId { get; set; }

        [StringLength(50, ErrorMessage ="*Cannot Exceed 50 Characters")]
        [Display(Name ="First Name")]
        [Required(ErrorMessage ="*First Name is Required")]
        public string ShipToFirstName { get; set; } = null!;      
        
        [StringLength(50, ErrorMessage ="*Cannot Exceed 50 Characters")]
        [Display(Name ="Last Name")]
        [Required(ErrorMessage ="*Last Name is Required")]
        public string ShipToLastName { get; set; } = null!;

        [StringLength(50, ErrorMessage = "*Cannot Exceed 50 Characters")]
        [Display(Name = "City")]        
        public string? ShipToCity { get; set; }

        [StringLength(50, ErrorMessage = "*Cannot Exceed 50 Characters")]
        [Display(Name = "State")]
        public string? ShipToState { get; set; }

        [StringLength(5, ErrorMessage = "*5 Digit Zipcode")]
        [Display(Name = "ZipCode")]
        [DataType(DataType.PostalCode)]
        public string? ShipToZip { get; set; }

        public string? UserId { get; set; }


        [Display(Name ="Address")]
        [StringLength(150, ErrorMessage ="*Cannot Exceed 150 Characters")]
        public string? ShipToAddress { get; set; }
    }

   
    public class ManufacturerMetadata
    {
        public int ManufacturerId { get; set; }

        [StringLength(100, ErrorMessage ="*Cannot Exceed 100 Characters")]
        [Required(ErrorMessage ="*Name is Required")]
        [Display(Name ="Name")]
        public string ManufacturerName { get; set; } = null!;
    }

    
    public class OrderMetadata
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "*Order Date is Required")]
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)] // {0:d} = MM/dd/yyy
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage = "*Ship Date is Required")]
        [Display(Name = "Ship Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)] // {0:d} = MM/dd/yyy
        public DateTime ShipDate { get; set; }

        [Display(Name ="Total Items")]
        [Required(ErrorMessage ="*Total Items Required")]
        public int ItemsSold { get; set; }

        [Display(Name = "Order Total")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "*Order Total is Required")]
        [Range(0, double.MaxValue)]
        public decimal OrderAmoutTotal { get; set; }
    }

    
    public class ProductMetadata
    {
        
        public int ProductId { get; set; }
        [Display(Name="Category Name")]
        public int CategoryId { get; set; }
        [Display(Name ="Manufacturer Name")]
        public int ManufacturerId { get; set; }

        [StringLength(100, ErrorMessage ="*Cannot Exceed 100 Characters")]
        [Display(Name ="Name")]
        [Required(ErrorMessage ="*Name is Required")]
        public string ProductName { get; set; } = null!;

        [StringLength(500, ErrorMessage = "*Cannot Exceed 500 Characters")]
        [Display(Name = "Description")]
        [DataType(DataType.MultilineText)]
        public string? ProductDescription { get; set; }

        [Display(Name = "Price")]
        [DisplayFormat(DataFormatString = "{0:c}", ApplyFormatInEditMode = false)]
        [Required(ErrorMessage = "*Price is Required")]
        [Range(0, double.MaxValue)]        
        public decimal ProductPrice { get; set; }

        [Display(Name = "In Stock")]
        [Required(ErrorMessage = "*In Stock is Required")]
        [Range(0, short.MaxValue)]
        public int UnitsInStock { get; set; }

        [Display(Name = "On Order")]        
        [Range(0, short.MaxValue)]
        public int? UnitsOnOrder { get; set; }

        [Display(Name ="Discontinued?")]
        public bool Discontinued { get; set; }
    }
}


