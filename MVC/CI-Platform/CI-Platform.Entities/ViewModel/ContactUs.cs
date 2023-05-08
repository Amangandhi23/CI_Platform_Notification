using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CI_Platform.Entities.ViewModel
{
    public  class ContactUs
    {
        [Required(ErrorMessage = "Name Address is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Envalid Email Address")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Subject Address is required")]
        public string Subject { get; set; } = null!;

        [Required(ErrorMessage = "Message Address is required")]
        public string Message { get; set; } = null!;
    }
}
