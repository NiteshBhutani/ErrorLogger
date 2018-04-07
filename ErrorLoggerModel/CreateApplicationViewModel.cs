using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using System.ComponentModel.DataAnnotations;
    using ErrorLoggerConstant;
    public class CreateApplicationViewModel
    {
        [Required(ErrorMessage = "Application Name is required field")]
        [Display(Name = "Application Name")]
        public string applicationName { get; set; }

        [Required(ErrorMessage = "Application Description is required field")]
        [Display(Name = "Application Description")]
        public string applicationDescription { get; set; }

        [Required(ErrorMessage = "Application Status is required field")]
        [Display(Name = "Status")]
        public StatusEnum applicationStatus { get; set; }

    }
}
