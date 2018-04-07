using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;

    public class RegisterUserViewModel
    {
        [Required(ErrorMessage = "Email is required field")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required field")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [Required(ErrorMessage = "Confirm Password is required field")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "First Name is required field")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required field")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Status is required field")]
        [Display(Name = "Status")]
        public Status_ Status { get; set; }

    }
    public enum Status_
    {
        Active,
        NonActive
    }
}
