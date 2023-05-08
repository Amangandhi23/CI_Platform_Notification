using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModel
{
    public class Reset 
    {
        public string Email { get; set; } = null!;

        public string Token { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$", ErrorMessage = "Password must be Minimum eight characters, at least one letter and one number:")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password can not match")]
        public string? ConfirmPassword { get; set; } = null!;
    }
}
