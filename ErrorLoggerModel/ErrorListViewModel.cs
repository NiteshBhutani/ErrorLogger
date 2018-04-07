using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    using ErrorLoggerConstant;
    public class ErrorListViewModel
    {
        public string errorDescription { get; set; }
        public LogLevelEnum logLevel { get; set; }
        public DateTime timestamp { get; set; }
        public string exceptionMessage { get; set; }
        public int appID { get; set; }
        public string applicationName { get; set; }

    }
}
