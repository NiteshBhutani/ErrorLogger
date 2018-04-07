using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace ErrorLoggerDBLayer
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using ErrorLoggerConstant;

    public class Error
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ErrorID { get; set; }
        
        [Required, MaxLength(1000)]
        public string errorDescription { get; set; }
        [Required]
        public LogLevelEnum logLevel { get; set; }
        public string timestamp { get; set; }
        public string exceptionMessage { get; set; }

        public int applicationID { get; set; }

        public virtual Application application { get; set; }
    }
}
