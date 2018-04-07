using System;
using System.Collections.Generic;
using System.Net.Http;
using ErrorLoggerModel;
using ErrorLoggerConstant;

namespace DLLLibrary
{
    using log4net; 
    public class ErrorLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
 
        #region Constants for the REST Service
        //private static int SERVICE_PORT = 49212;
        //private static string SERVICE_URL = "http://localhost:{0}/";
        private static string SERVICE_URL = "http://localhost/ErrorLoggerRestAPI/";

        private static string GENERAL_ACTION = "Api/Values";
        #endregion



        private static int appID_;

        public ErrorLogger(int applicationID)
        {
            appID_ = applicationID;
        }

        public void log(string error_, int logLevel_, Exception ex = null)
        {

            ErrorListViewModel err = new ErrorListViewModel()
            {
                errorDescription = error_,
                logLevel = (LogLevelEnum)logLevel_,
                timestamp = DateTime.Now,
                appID = appID_
            };

            if (ex != null)
            {
                err.exceptionMessage = ex.Message;
            }

            try
            {
                CreateItem(err);
            }
            catch(Exception exception)
            {
                Log.Error("Exception caught in DLLLibrary exception block. Exception :" + exception.Message);
            }
        }

        private static void CreateItem(ErrorListViewModel newItem)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(SERVICE_URL);

            var response = client.PostAsJsonAsync(GENERAL_ACTION, newItem).Result;
            if(response.StatusCode.Equals(System.Net.HttpStatusCode.InternalServerError))
            {
                Log.Debug("Post Fail. Exception Message : "+response.ReasonPhrase);
            }
        }

    }
}
