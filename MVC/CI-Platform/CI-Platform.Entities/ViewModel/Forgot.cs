using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModel
{
    public class Forgot
    {

        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage = "Envalid Email Address")]
        public string Email { get; set; } = null!;
    }
}
