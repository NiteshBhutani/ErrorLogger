using System.Collections.Generic;

namespace ErrorLoggerDBLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public class Application
    {
        public Application()
        {
            this.users = new HashSet<User>(); //No Duplicate User for Application that's why Hash set
            this.errors = new List<Error>(); //Error can be duplicate for particular application that's why List
        }
        //[Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }

        [Required]
        public string applicationName { get; set; }
        [MaxLength(3000)]
        public string applicationDescription { get; set; }

        public string applicationStatus { get; set; }

        public virtual ICollection<Error> errors { get; set; }
        public virtual ICollection<User> users { get; set; }
    }
}