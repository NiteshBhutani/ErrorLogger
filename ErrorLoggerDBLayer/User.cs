using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerDBLayer
{
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Applications = new HashSet<Application>(); //No Duplicate Application for User that's why Hash set
        }
        //[Required]
        public int UserId { get; set; }

        [Required]
        public string firstName { get; set; }

        [Required]
        public string lastName { get; set; }
        
        //[Required(ErrorMessage = "Role ID  is a required field")]
        public string roleID { get; set; }
        
        //Data annotation for timestamp
        public string lastLoginTimestamp { get; set; }

        [Required]
        public string statusText { get; set; }

        public virtual Login loginDetails { get; set; }

        public virtual ICollection<Application> Applications { get; set; }
    }
}