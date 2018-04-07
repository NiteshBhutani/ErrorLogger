using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Filters;
namespace ErrorLoggerRestAPI
{
    using ErrorLoggerRestAPI.Common;
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            log4net.Config.XmlConfigurator.Configure();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            GlobalConfiguration.Configuration.Filters.Add(new LogExceptionFilterAttribute());

        }
    }

    //Create filter
    public class LogExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            ErrorLogService.LogError(context.Exception, "LogExceptionFilterAttribute");
        }
    }
}
