using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerIP.Controllers
{
    using LoadersNLogic;
    using ErrorLoggerModel;
    
    public class userController : BaseController
    {
        // GET: user
        public ActionResult Index()
        {
            MvcApplication.logger.log("Controller: User Action: Index Method: GET Info: Index function entered", 1);
            UserDataHandler userDataHander = new UserDataHandler();
            //if (Session["userName"] == null)
            //{
            //    return RedirectToAction("Login", "Home");
            //}
            if (userDataHander.getUserRole(Session["userName"].ToString()).Equals("Admin"))
            {
                MvcApplication.logger.log("Controller: User Action: Index Method: GET Info: Admin logged in!", 1);

                return RedirectToAction("Index","Admin");
            }
            
            ICollection<UserIndexViewModel> user = userDataHander.GetAllApplicationForUser(Session["userName"].ToString());
            if (user == null)
                MvcApplication.logger.log("Controller: User Action: Index Method: GET Info: No application under the user", 1);
            return View(user);
        }

        
        //// GET: user/ViewDetails
        //public ActionResult viewError(int appID)
        //{
        //    ErrorLoggerHandler datasource = new ErrorLoggerHandler();
        //    if (Session["userName"] == null)
        //    {
        //        return RedirectToAction("Login", "Home");
        //    }
        //    List<ErrorListViewModel> logs = datasource.GetAllErrorLogs(appID);
        //    return View(logs);
            
        //}
        
    }
}