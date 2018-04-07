using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;
    public class ApplicationViewDetailMV
    {
        public ApplicationViewDetailMV()
        {
            userEmail = new List<string>();
        }

        public int applicationID { get; set; }
        [Required(ErrorMessage = "Application Name is required field")]
        [Display(Name = "Application Name")]
        public string applicationName { get; set; }
        [Required(ErrorMessage = "Application Description is required field")]
        [Display(Name = "Application Description")]
        public string applicationDesc { get; set; }
        public string applicationStatus { get; set; }
        public List<string> userEmail { get; set; }
        
    }
}
