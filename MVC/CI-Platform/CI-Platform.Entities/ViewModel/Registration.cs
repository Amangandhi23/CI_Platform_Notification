using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CI_Platform.Entities.ViewModel
{
    public class Registration 
    {
        [Required(ErrorMessage = "Please Enter First Name")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter Last Name")]
        public string? LastName { get; set; }


        [Required(ErrorMessage ="Please Enter Phone Number")]
        [RegularExpression("^[0-9]{10}$" , ErrorMessage = "Phone Number Lenght Must be 10")]
        public int PhoneNumber { get; set; }


        [Required(ErrorMessage = "Email Address is required")]
        [EmailAddress(ErrorMessage ="Envalid Email Address")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; } = null!;


        [Required(ErrorMessage = "Password is required")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$" , ErrorMessage = "Password must be Minimum eight characters, at least one letter and one number:")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;


        [Required(ErrorMessage = "Password is required")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password can not match")]
        public string? ConfirmPassword { get; set; } = null!;
    }

}
