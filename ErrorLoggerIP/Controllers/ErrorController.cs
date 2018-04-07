using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerIP.Controllers
{
    using LoadersNLogic;
    using ErrorLoggerModel;
    public class ErrorController : BaseController
    {
        // GET: user/ViewDetails
        public ActionResult viewError(int appID, string sortParam = "")
        {
            ViewBag.LogLevelParm = sortParam == "Level" ? "level_desc" : "Level";
            ViewBag.DateSortParm = sortParam == "Date" ? "date_desc" : "Date";
            ViewBag.ErrDesc = sortParam == "Desc" ? "desc_desc" : "Desc";
            ViewBag.ExMsg = sortParam == "ExMsg" ? "exMsg_desc" : "ExMsg";

            MvcApplication.logger.log("Controller: Error Action: viewError Method: GET Info: viewError function entered", 1);

            ErrorLoggerHandler datasource = new ErrorLoggerHandler();
            List<ErrorListViewModel> logs = datasource.GetAllErrorLogs(appID);

            //Collecting count of all Log Level
            TempData["info"] = datasource.getCountOfErrorLogs(appID, ErrorLoggerConstant.LogLevelEnum.Info);
            TempData["debug"] = datasource.getCountOfErrorLogs(appID, ErrorLoggerConstant.LogLevelEnum.Debug);
            TempData["error"] = datasource.getCountOfErrorLogs(appID, ErrorLoggerConstant.LogLevelEnum.Error);
            TempData["performance"] = datasource.getCountOfErrorLogs(appID, ErrorLoggerConstant.LogLevelEnum.Performance);
            TempData["fatal"] = datasource.getCountOfErrorLogs(appID, ErrorLoggerConstant.LogLevelEnum.Fatal);

            if (logs == null )
                MvcApplication.logger.log("Controller: Error Action: viewError Method: GET Info: No  logs for praticular application ID", 2);

            switch (sortParam)
            {
                case "Level":
                    logs.Sort((x, y) => Nullable.Compare<ErrorLoggerConstant.LogLevelEnum>(x.logLevel, y.logLevel));
                    break;
                case "level_desc":
                    logs.Sort((x,y) => -1 * Nullable.Compare<ErrorLoggerConstant.LogLevelEnum>(x.logLevel, y.logLevel));
                    break;
                case "Date":
                    logs.Sort((x, y) => Nullable.Compare<DateTime>(x.timestamp, y.timestamp));
                    break;
                case "date_desc":
                    logs.Sort((x, y) => -1 * Nullable.Compare<DateTime>(x.timestamp, y.timestamp));
                    break;
                case "Desc":
                    logs.Sort((x, y) =>  x.errorDescription.CompareTo(y.errorDescription));
                    break;
                case "desc_desc":
                    logs.Sort((x, y) => -1 * x.errorDescription.CompareTo(y.errorDescription));
                    break;
                case "ExMsg":
                    logs.Sort(delegate (ErrorListViewModel x, ErrorListViewModel y)
                    {
                        if (x.exceptionMessage == null && y.exceptionMessage == null) return 0;
                        else if (x.exceptionMessage == null) return -1;
                        else if (y.exceptionMessage == null) return 1;
                        else return x.exceptionMessage.CompareTo(y.exceptionMessage);
                    });
                    break;
                case "exMsg_desc":
                    logs.Sort(delegate (ErrorListViewModel x, ErrorListViewModel y)
                    {
                        if (x.exceptionMessage == null && y.exceptionMessage == null) return 0;
                        else if (x.exceptionMessage == null) return 1;
                        else if (y.exceptionMessage == null) return -1;
                        else return  -1 * x.exceptionMessage.CompareTo(y.exceptionMessage);
                    });
                    
                    break;
                default:
                    break;
            }
            return View(logs);

        }
    }
}