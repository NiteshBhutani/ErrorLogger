using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerDBLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Login
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LoginId { get; set; }
        
        [Required]
        public string emailID { get; set; }

        [Required, MinLength(8)]
        public string password { get; set; }

        public virtual User userDetails { get; set; }

    }
}
