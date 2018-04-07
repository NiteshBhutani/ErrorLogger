using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;

    public class UserDetailsMV
    {
        public UserDetailsMV()
        {
            applications = new List<string>();
        }
        public int UserID { get; set; }
        [Required(ErrorMessage = "Email is required field")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "First Name is required field")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name is required field")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Status { get; set; }
        public string Role { get; set; }
        public string lastLoginTimestamp { get; set; }
        [Required(ErrorMessage = "Application is required field")]
        [Display(Name = "Applications")]
        public List<string> applications { get; set; }

    }
}
