using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErrorLoggerIP.Controllers
{
    using Security;

    public class BaseController : Controller
    {
        #region String constants

        protected static string DETAILED_UNAUTHORIZED_ERROR = "User {0} attempted to access {1}/{2}.";
        protected static string DETAILED_WITH_APPLICATION_ID_ERROR = "Attempted to access application {0}";
        protected static string USER_UNAUTHORIZED_ERROR = "You are not authorized for the specified page.";

        #endregion

        /// <summary>
        /// Authentication class
        /// </summary>
        private PageAuthentication authentication { get; set; }

        /// <summary>
        /// Base Controller
        /// </summary>
        /// <param name="authentication">Page Authentication class</param>
        public BaseController()
        {
            this.authentication = new PageAuthentication();
            //this.authentication = authentication;
            MvcApplication.logger.log("Controller: Base Action: Contructor  Info: Function Entered", 1);

        }

        #region onActionExecuting
        /// <summary>
        /// Executes before the action executes, so we can set stuff up, make sure we are okay with them running the action
        /// </summary>
        /// <param name="filterContext">Context</param>
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // gather data
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;
            string userName = string.IsNullOrEmpty((string)HttpContext.Session["userName"]) ? "none" : (string)HttpContext.Session["userName"];

            // authenticate user based on security matrix
            if (!this.authentication.IsUserAuthorized(controller, action, userName))
            {
                this.ProcessUserBeingUnauthorized(controller, action, userName);
            }
            else
            {
                // if the user has access to the controller & action in general, then do further authentication based on the data
                this.AuthenticateApplicationIdIfNeeded(filterContext);
            }

            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region onActionExecuted
        /// <summary>
        /// Executes after the action has finished, cleanup work..
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string action = filterContext.ActionDescriptor.ActionName;

            //MvcApplication.logger.log("Controller: Base Action: onActionExecuted  Info: "+string.Format("Finishing { 0}\\{ 1}", controller, action), 1);
            //Optional -> do other checks (such as log performance numbers)
            
            base.OnActionExecuted(filterContext);
        }

        #endregion

        #region onException Function
        /// <summary>
        /// If an unhandled exception was thrown in the controller, this will execute, so we can (but don't have to)
        /// handle it.
        /// </summary>
        /// <param name="filterContext">Context</param>

        protected override void OnException(ExceptionContext filterContext)
        {
            // Log the error
            MvcApplication.logger.log("Controller: Base Action: onException Error:"+ filterContext.Exception.Message,3);

            if (filterContext.Exception.Message.Contains("Login Failed. Check if password is correct otherwise contact the admin."))
            {
                filterContext.ExceptionHandled = true;
                if (filterContext.Exception.Message.Contains("CustomizeDeveloperMessage:"))
                    TempData["errorMessage"] = filterContext.Exception.Message.Remove(0, 26);
                filterContext.Result = RedirectToAction("Error", "Home");

            }
            else if (string.IsNullOrEmpty((string)HttpContext.Session["userName"]))
            {
                filterContext.ExceptionHandled = true;

                filterContext.Result = RedirectToAction("Login", "Home");

            }
            else
            {
                // Blow out the session
                Session.Clear();

                
                //Redirect user to the error page    
                filterContext.ExceptionHandled = true;
                if (filterContext.Exception.Message.Contains("CustomizeDeveloperMessage:"))
                    TempData["errorMessage"] = filterContext.Exception.Message.Remove(0, 26);
                filterContext.Result = RedirectToAction("Error", "Home");
            }
            base.OnException(filterContext);
        }
        #endregion

        #region Private Helpers

        /// <summary>
        /// Checks if further authentication is needed. If it is, it does authenticate.
        /// </summary>
        /// <param name="filterContext">Context</param>
        /// <param name="controller">Controller</param>
        /// <param name="action">Action</param>
        /// <param name="userName">User Name</param>
        private void AuthenticateApplicationIdIfNeeded(ActionExecutingContext filterContext)
        {
            // now let's see if further authentication is needed
            // if course Id and assignment Id exist -> you are trying to view a single assignment.. make sure you actually can?
            int appID = filterContext.ActionParameters.ContainsKey("appID") ? (int)filterContext.ActionParameters["appID"] : -1;
            
            if (appID > 0)
            {
                string userName = string.IsNullOrEmpty((string)HttpContext.Session["userName"]) ? "none" : (string)HttpContext.Session["userName"];

                if (!this.authentication.IsUserAuthorizedForApplicationID(userName, appID))
                {
                    string controller = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                    string action = filterContext.ActionDescriptor.ActionName;

                    this.ProcessUserBeingUnauthorized(controller, action, userName,appID);
                }
            }
        }

        /// <summary>
        /// Processes the fact that the user was unauthorized..
        /// </summary>
        /// <param name="controller">Controller</param>
        /// <param name="action">Action</param>
        /// <param name="userName">User Name</param>
        /// <param name="courseId">Optional Course Id</param>
        /// <param name="assignmentId">Optional Assignment Id</param>
        private void ProcessUserBeingUnauthorized(string controller, string action, string userName,
            int appID = -1)
        {
            // Basic error message that will be displayed to the user
            TempData["errorMessage"] = USER_UNAUTHORIZED_ERROR;

            // Create the detailed error message that will be logged
            string detailedError = string.Format(DETAILED_UNAUTHORIZED_ERROR, userName, controller, action);
            if (appID != -1)
            {
                detailedError += string.Format(DETAILED_WITH_APPLICATION_ID_ERROR, appID);
            }

            // Throw an exception
            throw new UnauthorizedAccessException(detailedError);
        }


        #endregion
    }
}