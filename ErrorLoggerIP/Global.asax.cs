using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using DLLLibrary;
namespace ErrorLoggerIP
{
    using Security;
    public class MvcApplication : System.Web.HttpApplication
    {
        //Initialize logger - Using your own Logger
        public static ErrorLogger logger = new ErrorLogger(5);
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/Web.config")));
            //Initialize security matrix
            SecurityMatrix.Initialize();

            
        }

        protected void Application_Error(object sender, EventArgs e)
        {  // Blow out the session
            Session.Clear();
            Exception exception = Server.GetLastError();
            Log.Error("Exception in Global Error handler. Exception: "+ exception.Message);
            Server.ClearError();
            Response.Redirect("/Home/Error");
        }
    }
}
