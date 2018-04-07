using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerIP.Controllers
{
    using LoadersNLogic;
    using ErrorLoggerModel;
    public class adminController : BaseController
    {
        #region Index
        // GET: admin
        public ActionResult Index()
        {
            MvcApplication.logger.log("Controller: Admin Action: Index Method: GET Info: Index function entered", 1);
            ErrorLoggerHandler dataSource = new ErrorLoggerHandler();
            AdminIndexViewModel add = dataSource.pullAllUserAndApplication();
            if(add==null)
                MvcApplication.logger.log("Controller: Admin Action: Index Method: GET Debug: No application and user exist as sunch for now", 2);

            return View(add);
        }
        #endregion

        #region Create Application
        // GET: admin/createApplication
        public ActionResult createApplication()
        {
            MvcApplication.logger.log("Controller: Admin Action: Create Application Method: GET Info: Create Application function entered", 1);

            return View();
        }

        //POST: admin/createApplication
        /// <summary>
        /// Handles the Post action of an Registration
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult createApplication(CreateApplicationViewModel application)
        {
            if (ModelState.IsValid)
            {
                MvcApplication.logger.log("Controller: Admin Action: Create Application Method: POST Info: Modal State is Valid", 1);
                ApplicationDataHandler dataSource = new ApplicationDataHandler();
                if (dataSource.saveApplicationInDB(application))
                {
                    return RedirectToAction("Index", "Admin", null);
                } else
                {
                    MvcApplication.logger.log("Controller: Admin Action: Create Application Method: POST Error:  Application already exist.", 3);
                    throw new Exception("CustomizeDeveloperMessage: Not able to save Application in DB. Check if, Application already exist.");
                }
            }

            return View(application);
        }
        #endregion

        #region View Application Details
        // GET: admin/viewApplicationDetails
        public ActionResult viewApplicationDetails(int appID)
        {
            MvcApplication.logger.log("Controller: Admin Action: View Application details Method: GET Error: View Application Details function entered", 1);
            ApplicationDataHandler dataSource = new ApplicationDataHandler();
            ApplicationViewDetailMV app = dataSource.getApplicationDetails(appID);
            if (app == null)
            {
                MvcApplication.logger.log("Controller: Admin Action: View Application details Method: GET Error: Check App ID", 3);
                throw new Exception("CustomizeDeveloperMessage: Not able to retireve Application details. Check, Application ID.");
            }
            return View(app);
        }
        #endregion

        #region View User Details
        // GET: admin/viewUserDetails
        public ActionResult viewUserDetails(int userID)
        {
            MvcApplication.logger.log("Controller: Admin Action: View User details Method: GET Info: Function Entered", 1);
            UserDataHandler dataSource = new UserDataHandler();
            UserDetailsMV user = dataSource.getUserDetails(userID);
            if (user == null)
            {
                MvcApplication.logger.log("Controller: Admin Action: View User details Method: GET Error: Check User ID", 3);
                throw new Exception("CustomizeDeveloperMessage: Not able to retireve User details. Check, User ID.");
            }
            return View(user);
        }
        #endregion

        #region Edit Application
        // GET: admin/EditApplication
        public ActionResult EditApplication(int appID)
        {
            MvcApplication.logger.log("Controller: Admin Action: Edit Application Method: GET Info: Function Entered", 1);
            ApplicationDataHandler dataSource = new ApplicationDataHandler();
            ApplicationViewDetailMV app = dataSource.getApplicationDetails(appID);
            if (app == null)
            {
                MvcApplication.logger.log("Controller: Admin Action: Edit Details Method: GET Error:Check App ID", 3);
                throw new Exception("CustomizeDeveloperMessage: Not able to retireve Application details. Check, Application ID.");
            }
            TempData["appID"] = app.applicationID;
            return View(app);
            
        }

        //POST: admin/EditApplication
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditApplication(ApplicationViewDetailMV application)
        {
            if (ModelState.IsValid)
            {
                MvcApplication.logger.log("Controller: Admin Action: Edit Application Method: POST Info: Modal State is Valid", 1);
                ApplicationDataHandler dataSource = new ApplicationDataHandler();
                application.applicationID = (int)TempData["appID"];
                if (dataSource.updateApplicationInDB(application))
                {
                    return RedirectToAction("viewApplicationDetails", "Admin", new { appID = application.applicationID});
                }
                else
                {
                    MvcApplication.logger.log("Controller: Admin Action: Edit Application Method: POST Error: Application Name already Exist", 3);
                    TempData["applicationCreateMessage"] = "Application Name already exist.";
                }
            }

            return View(application);
        }
        #endregion


        #region Edit User
        // GET: admin/EditUser
        public ActionResult EditUser(int userID)
        {
            MvcApplication.logger.log("Controller: Admin Action: Edit User Method: GET Info: Function Entered", 1);
            UserDataHandler dataSource = new UserDataHandler();
            //Get all the user data and applications
            EditUserMV user = dataSource.setEditData(userID);
            if (user == null)
            {
                 MvcApplication.logger.log("Controller: Admin Action: Edit User Method: GET Error: Initilization Error", 3);
                throw new Exception("CustomizeDeveloperMessage: Not able to retireve all User details and Application List. Initialization Error.");
            }
            TempData["userID"] = user.UserID;
            return View(user); 
        }

        //POST: admin/EditApplication
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditUser(EditUserMV user)
        {
            if (ModelState.IsValid)
            {
                UserDataHandler dataSource = new UserDataHandler();
                user.UserID = (int)TempData["userID"];
                if (dataSource.updateUserInDB(user))
                {
                    return RedirectToAction("viewUserDetails", "Admin", new { userID = user.UserID });
                }
                else
                {
                    MvcApplication.logger.log("Controller: Admin Action: Edit User  Method: POST Error: User Email Already exist", 3);
                    TempData["userCreateMessage"] = "User Email already exist.";
                }
            }

            return View(user);
        }

        #endregion

        
    }
}