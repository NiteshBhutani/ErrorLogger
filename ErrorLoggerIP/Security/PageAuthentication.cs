using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerIP.Security
{
    using ErrorLoggerConstant;
    using LoadersNLogic;

    public class PageAuthentication
    {
            /// <summary>
            /// Checkes whether the user is authorized to access the page
            /// </summary>
            /// <param name="controller">Controller</param>
            /// <param name="action">Action</param>
            /// <param name="userName">User Name</param>
            /// <returns>Is user authorized</returns>
            public bool IsUserAuthorized(string controller, string action, string userName)
            {
                UserDataHandler dataSource = new UserDataHandler();
                RoleEnum usersRole;
                if (userName.Equals("none"))
                {
                    usersRole = RoleEnum.none;
                }
                else
                {
                    string userRole =dataSource.getUserRole(userName);

                if (userRole.Equals("User"))
                    usersRole = RoleEnum.user;
                else if (userRole.Equals("Admin"))
                    usersRole = RoleEnum.admin;
                else
                {
                    MvcApplication.logger.log("Class:Page Authentication Function: isUserAuthorized Error: System Authentication Error.Check Database connection", 3);
                    throw new Exception("CustomizeDeveloperMessage: System Authentication Error.Check Database connection.");
                }
                }
                
                // get required role from the Matrix (this will fail if we haven't registered the requested controller/action combination
                RoleEnum requiredRole = SecurityMatrix.Matrix.First(x => x.Controller == controller && x.Action == action).MinimumRoleNeeded;

                return usersRole >= requiredRole;
            }

            /// <summary>
            /// Checks if the user is authorized for the selected Assignment
            /// </summary>
            /// <param name="courseId">Course Id</param>
            /// <param name="assignmentId">Assignment Id</param>
            /// <returns>Whether the user is authorized</returns>
            public bool IsUserAuthorizedForApplicationID(string userName, int appID) {

                bool result = false;
                UserDataHandler dataSource = new UserDataHandler();
            if (dataSource.getUserRole(userName).Equals("Admin"))
            {
                result = true;
            }
            else
            {
                result = dataSource.checkIsUserAuthorizedForApplicationID(userName, appID);
            }
                return result;
            }

        }
 }