using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerConstant
{
    public static class WebConstants
    {
        // Actions - Home Controller
        public static string HOME_HOME_CONTROLLER = "Index";
        public static string REGISTER_HOME_CONTROLLER = "Register";
        public static string LOGIN_HOME_CONTROLLER = "Login";
        public static string LOGOUT_HOME_CONTROLLER = "Logout";
        public static string ERROR_HOME_CONTROLLER = "Error";

        //Actions - User Controller
        public static string HOME_USER_CONTROLLER = "Index";
        //public static string VIEWERROR_USER_CONTROLLER = "viewError";

        //Actions - Admin COntroller
        public static string HOME_ADMIN_CONTROLLER = "Index";
        public static string CREATEAPPLICATION_ADMIN_CONTROLLER = "createApplication";
        public static string VIEWAPPLICATIONDETAILS_ADMIN_CONTROLLER = "viewApplicationDetails";
        public static string VIEWUSERDETAILS_ADMIN_CONTROLLER = "viewUserDetails";
        public static string EDITAPP_ADMIN_CONTROLLER = "EditApplication";
        public static string EDITUSER_ADMIN_CONTROLLER = "EditUser";
        //Action - Error Controller
        public static string VIEWERROR_ERROR_CONTROLLER = "viewError";

        // Controllers
        public static string HOME_CONTROLLER = "Home";

        public static string ADMIN_CONTROLLER = "admin";

        public static string USER_CONTROLLER = "user";

        public static string Error_CONTROLLER = "Error";

    }
}
