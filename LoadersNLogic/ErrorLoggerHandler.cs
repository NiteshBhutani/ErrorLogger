using System;
using System.Collections.Generic;
using System.Linq;
using ErrorLoggerDBLayer;
using ErrorLoggerModel;
using ErrorLoggerConstant;
namespace LoadersNLogic
{
    using log4net;
    public class ErrorLoggerHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Creates the DB
        /// </summary>
        public static void CreateDB()
        {
            Console.WriteLine("~~~~ Creating the DB ~~~~");
            Console.WriteLine();

            using (ErrorLoggerDBContext db = new ErrorLoggerDBContext())
            {
                // Initialize the DB - false doesn't force reinitialization if the DB already exists
                db.Database.Initialize(false);

                // Seeding runs the first time you try to use the DB, so we make it seed here..
                // It only runs IF the initializer condition is met, regardless of the True/False above
                db.Users.Count();
            }
        }

        /// <summary>
        /// Deletes the DB
        /// </summary>
        public static void DeleteDB()
        {
            Console.WriteLine("~~~~ Deleting the DB ~~~~");
            Console.WriteLine();

            using (ErrorLoggerDBContext db = new ErrorLoggerDBContext())
            {
                if (db.Database.Exists())
                {
                    db.Database.Delete();
                }
            }
        }

        /// <summary>
        /// Used for formatting output
        /// </summary>
        public static string SPACE = "   ";

        /// <summary>
        /// Gets the data grouped by Courses
        /// </summary>
        public static void PullOutDataByUser()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by Users): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                foreach (User user in context.Users.ToList())
                {
                    Console.WriteLine(String.Format(SPACE + "User Id: {0}, User EmailID: {1}",
                        user.UserId, user.loginDetails.emailID));

                    foreach (Application app in user.Applications)
                    {
                        Console.WriteLine(String.Format(SPACE + SPACE + "Application Id: {0}, Application Name: {1} Application Desc: {2}",
                            app.ApplicationId, app.applicationName, app.applicationDescription));
                        foreach(Error err in app.errors)
                        {
                            Console.WriteLine(String.Format(SPACE + SPACE + "Error Id: {0}, Error Description: {1}, Error Application Id: {2}",
                            err.ErrorID, err.errorDescription, err.applicationID, app.applicationDescription));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the data grouped by Students
        /// </summary>
        public static void PullOutDataByApplications()
        {
            Console.WriteLine();
            Console.WriteLine("~~~~ Data in the DB (by application): ~~~~");
            Console.WriteLine();

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                foreach (Application App in context.Applications.ToList())
                {
                    Console.WriteLine(String.Format(SPACE + SPACE + "Application Id: {0}, Application: {1}",
                            App.ApplicationId, App.applicationName));

                    foreach (User user in App.users)
                    {
                        Console.WriteLine(String.Format(SPACE + SPACE + "User Id: {0}, User Email: {1}",
                            user.UserId, user.loginDetails.emailID));
                    }
                }
            }
        }


        public bool saveErrorInDB(ErrorRestViewModel err)
        {
            bool result = false;

            Error error1 = new Error()
            {
                errorDescription = err.errorDescription,
                logLevel = err.logLevel,
                applicationID = err.appID,
                exceptionMessage = err.exceptionMessage,
                timestamp = err.timestamp.ToString()
            };
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {   //check if app ID exist - If App ID does not exist context.savechanges will fail and this will be catched in catch block.
                    context.Errors.Add(error1);
                    context.SaveChanges();
                    result = true;
                }
                catch(Exception ex)
                {
                    Log.Error("LoadersnLogic//SaveErrorinDB "+ex.Message);
                    throw ex;
                   
                }
            }

            return result;
        }


        public List<ErrorListViewModel> GetAllErrorLogs(int appID)
        {
            List<ErrorListViewModel> logs = new List<ErrorListViewModel>();
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var app = context.Applications.Where(x => x.ApplicationId.Equals(appID)).First();
                    
                    foreach (Error err in app.errors)
                    {
                        ErrorListViewModel errorView = new ErrorListViewModel
                        {
                            logLevel = err.logLevel,
                            errorDescription = err.errorDescription,
                            timestamp = Convert.ToDateTime(err.timestamp),
                            exceptionMessage = err.exceptionMessage,
                            appID = err.applicationID,
                            applicationName = app.applicationName
                        };
                        logs.Add(errorView);
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(string.Format("Function : GetAllErrorLogs CustomizeDeveloperMessage: Not able to get Errors for application {0}. Excetion Message: {1} ", appID,ex));
                    throw new Exception(string.Format("CustomizeDeveloperMessage: Not able to get Errors for application {0}. ", appID));
                 }
            }
        
            return logs;
        }

        public int getCountOfErrorLogs(int appID, LogLevelEnum level_)
        {
            int count;
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    count = context.Errors.Where(x => x.applicationID == appID && x.logLevel == level_).Count();
                }
                catch (Exception ex)
                {
                    Log.Error(string.Format("Function : getCountOfErrorLogs CustomizeDeveloperMessage: Not able to get Count of error logs for applicationID: {0}. Excetion Message: {1} ", appID, ex));
                    throw new Exception(string.Format("CustomizeDeveloperMessage: Not able to get Count of error logs for applicationID: {0} ", appID));
                }
            }
            return count;
        }

        public AdminIndexViewModel pullAllUserAndApplication()
        {
            AdminIndexViewModel view = new AdminIndexViewModel();
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    foreach (User user in context.Users.ToList())
                    {
                        view.userID.Add(user.UserId);
                        view.userFirstName.Add(user.firstName);
                        view.userLastName.Add(user.lastName);
                        view.userLastLoginTimestamp.Add(user.lastLoginTimestamp);
                    }

                    foreach (Application app in context.Applications.ToList())
                    {
                        view.applicationID.Add(app.ApplicationId);
                        view.applicationName.Add(app.applicationName);
                    }
                }
                catch(Exception ex)
                {
                    Log.Error(string.Format("Function : pullAllUserAndApplication CustomizeDeveloperMessage: Unable to retrieve data. Database Error.Exception Message : {0}", ex.Message));
                    throw new Exception("CustomizeDeveloperMessage: Unable to retrieve data. Database Error.");
                }
            }
            return view;

        }

    }
}