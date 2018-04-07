using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerIP.Controllers
{
    using LoadersNLogic;
    using ErrorLoggerModel;
    using log4net;
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region Register
        //Get : Home/Register
        public ActionResult Register()
        {
           MvcApplication.logger.log("Controller: Home Action: Regsiter Method: Get Info: Register function entered", 1);
            return View();
        }

        // Post: Home/Register
        /// <summary>
        /// Handles the Post action of an Registration
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Register(RegisterUserViewModel newUser)
        {
            if (ModelState.IsValid)
            {
                MvcApplication.logger.log("Controller: Home Action: Regsiter Method: Post Info: ModelState is valid", 1);
                UserDataHandler dataSource = new UserDataHandler();
                if(dataSource.AddUser(newUser))
                {
                    MvcApplication.logger.log("Controller: Home Action: Regsiter Method: Post Info:"+newUser.Email+ " save in the database.", 1);
                    return RedirectToAction("Login", "Home");
                }

            }
            return View(newUser);
        }

        #endregion

        #region Login
        public ActionResult Login()
        {
            MvcApplication.logger.log("Controller: Home Action: Login Method: Get Info: Login function entered", 1);
            if (Session["userName"] != null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        
        // Post: Home/Login
        /// <summary>
        /// Handles the Post action of an Registration
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Login(LoginUserViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                MvcApplication.logger.log("Controller: Home Action: Login Method: Post Info: ModelState is valid", 1);
                UserDataHandler dataSource = new UserDataHandler();
                List<string> loggedUser = dataSource.checkIfUserExist(loginUser);//also check if user is active
                if (loggedUser.Count != 0)
                {
                    //Update LastLoginTimestamp for LoggedUser
                    dataSource.updateLastLoginTimestamp(loginUser.Email);
                    Session["userID"] = loggedUser[1];
                    Session["userName"] = loggedUser[0];
                    string userRole = dataSource.getUserRole(loggedUser[0]);
                    if (userRole.Equals("Admin"))
                        return RedirectToAction("Index", "Admin");
                    else if (userRole.Equals("User"))
                        return RedirectToAction("Index", "User");
                    else
                    {
                        MvcApplication.logger.log("Controller:Home Action: Login Method:Post Error: Unable to get user role. Check Database connection",3);
                        throw new Exception("CustomizeDeveloperMessage: Unable to get User Role. Check Database connection");
                    }
                }
            }

            return View(loginUser);
        }

        #endregion

       
        #region Logout
        // GET: Home/Logout
        public ActionResult Logout()
        {
            MvcApplication.logger.log("Controller: Home Action: Logout Method: GET Info: Logout function entered", 1);
            // Blow out the session
            Session.Clear();

            return RedirectToAction("Index");
        }
        #endregion

        // GET: Home/Error
        #region Error
        public ActionResult Error()
        {
            MvcApplication.logger.log("Controller: Home Action: Error Method: GET Info: Error Action entered", 1);
            return View();
        }
        #endregion
    }

}