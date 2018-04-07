using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerIP.Security
{
    using ErrorLoggerConstant;
    /// <summary>
    /// Security Matrix holds on to the minimal roles required for each controller/page combination
    /// </summary>
    public static class SecurityMatrix
    {
        /// <summary>
        /// Security Matrix, containing all of the controller/actions and the needed roles
        /// </summary>
        public static ICollection<SecurityAccess> Matrix { get; set; }

        /// <summary>
        /// Initializes the security matrix
        /// </summary>
        public static void Initialize()
        {
            Matrix = new List<SecurityAccess>()
            {
                #region Home Controller

                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.HOME_HOME_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.none
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.ERROR_HOME_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.none
                },
                new SecurityAccess()    
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.LOGIN_HOME_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.none
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.REGISTER_HOME_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.none
                },
                new SecurityAccess()
                {
                    Controller = WebConstants.HOME_CONTROLLER,
                    Action = WebConstants.LOGOUT_HOME_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.none
                },
                #endregion

                #region User Controller

                new SecurityAccess()
                {
                    Controller = WebConstants.USER_CONTROLLER,
                    Action = WebConstants.HOME_USER_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.user
                },

                
                #endregion

                #region Admin Controller

                new SecurityAccess()
                {
                    Controller = WebConstants.ADMIN_CONTROLLER,
                    Action = WebConstants.HOME_ADMIN_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.admin
                },

                new SecurityAccess()
                {
                    Controller = WebConstants.ADMIN_CONTROLLER,
                    Action = WebConstants.CREATEAPPLICATION_ADMIN_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.admin
                },

                new SecurityAccess()
                {
                    Controller = WebConstants.ADMIN_CONTROLLER,
                    Action = WebConstants.VIEWAPPLICATIONDETAILS_ADMIN_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.admin
                },

                new SecurityAccess()
                {
                    Controller = WebConstants.ADMIN_CONTROLLER,
                    Action = WebConstants.VIEWUSERDETAILS_ADMIN_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.admin
                },

                 new SecurityAccess()
                {
                    Controller = WebConstants.ADMIN_CONTROLLER,
                    Action = WebConstants.EDITAPP_ADMIN_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.admin
                },

                new SecurityAccess()
                {
                    Controller = WebConstants.ADMIN_CONTROLLER,
                    Action = WebConstants.EDITUSER_ADMIN_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.admin
                },

                #endregion

                #region Error Controller
                new SecurityAccess()
                {
                    Controller = WebConstants.Error_CONTROLLER,
                    Action = WebConstants.VIEWERROR_ERROR_CONTROLLER,
                    MinimumRoleNeeded = RoleEnum.user
                },

                #endregion
            };
        }
    }
}