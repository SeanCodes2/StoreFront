using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace StoreFront.UI.MVC.Models
{
    [Keyless]
    public class ContactViewModel
    {
        [Required(ErrorMessage ="*")]
        public string Name { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "*")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "*")]
        [DataType(DataType.MultilineText)]
        public string Message { get; set; }
    }
}
