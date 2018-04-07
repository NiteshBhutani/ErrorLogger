using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerRestAPI.Common
{
    using log4net;
    public class ErrorLogService
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void LogError(Exception ex, string functionName)
        {
            logger.Error("Global Error Handler in WEB API. Function: " +functionName+" Exception: "+ ex.Message);
        }
    }
}