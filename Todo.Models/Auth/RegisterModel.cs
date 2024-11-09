using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo.Models.Auth;
public class RegisterModel
{
    [Required(ErrorMessage = "{0} is required")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "{0} must be between {2} and {1} characters")]
    [Display(Name = "Person Name")]
    public string? PersonName { get; set; }

    [DataType(DataType.EmailAddress, ErrorMessage = "Invalid Email Address")]
    [Display(Name = "Email Address")]
    [Required(ErrorMessage = "{0} is required")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Phone Number")]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^\+?\d{1,3}[-. ]?\(?\d{1,4}\)?[-. ]?\d{1,4}[-. ]?\d{1,4}[-. ]?\d{1,4}$", ErrorMessage = "Invalid Phone Number")]
    public string? PhoneNumber { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    [Required(ErrorMessage = "{0} is required")]
    [Display(Name = "Confirm Password")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match")]
    public string? ConfirmPassword { get; set; }
    [Required(ErrorMessage = "You have to Accept the terms and conditions")]
    public bool Agreement { get; set; }
}
