using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadersNLogic
{
    using ErrorLoggerModel;
    using ErrorLoggerDBLayer;
    using log4net;
    public class ApplicationDataHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public bool updateApplicationInDB(ApplicationViewDetailMV app)
        {
            bool result = false;
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var application = from a in context.Applications
                                      where a.ApplicationId != app.applicationID
                                       select a;
                    foreach(Application a in application)
                    {
                        if (a.applicationName == app.applicationName)
                            return false;
                    }
                        var app_ = context.Applications.Where(x => x.ApplicationId.Equals(app.applicationID)).First();
                        app_.applicationName = app.applicationName;
                        app_.applicationStatus = app.applicationStatus;
                        app_.applicationDescription = app.applicationDesc;
                        context.SaveChanges();
                        result = true;
                    
                }
                catch
                {
                    Log.Error("Function: updateApplicationInDB CustomizeDeveloperMessage: Not able to update Application in DB. Database Error. ");
                    throw new Exception("CustomizeDeveloperMessage: Not able to update Application in DB. Database Error. ");

                }
            }

            return result;
        }

        public bool saveApplicationInDB(CreateApplicationViewModel app)
        {
            bool result = false;
            Application app_ = new Application()
            {
                applicationName = app.applicationName,
                applicationDescription = app.applicationDescription,
                applicationStatus = app.applicationStatus.ToString()
            };

            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var application = context.Applications.Where(x => x.applicationName.Equals(app.applicationName)).Any();
                    if (!application)
                    {
                        context.Applications.Add(app_);
                        context.SaveChanges();
                        result = true;
                    }
                }
                catch
                {
                    Log.Error("Function: saveApplicationInDB CustomizeDeveloperMessage: Not able to save Application in DB. Database Error.");
                    throw new Exception("CustomizeDeveloperMessage: Not able to save Application in DB. Database Error. ");

                }
            }

            return result;
        }

        public ApplicationViewDetailMV getApplicationDetails(int appID)
        {
            ApplicationViewDetailMV appDetails = new ApplicationViewDetailMV();
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var app = context.Applications.Where(x => x.ApplicationId.Equals(appID)).First();
                    if (app != null)
                    {
                        appDetails.applicationID = app.ApplicationId;
                        appDetails.applicationName = app.applicationName;
                        appDetails.applicationStatus = app.applicationStatus;
                        appDetails.applicationDesc = app.applicationDescription;
                        foreach(User u in app.users)
                        {
                            appDetails.userEmail.Add(u.loginDetails.emailID);
                        }
                    }
                }
                catch
                {
                    Log.Error("Function: getApplicationDetails CustomizeDeveloperMessage: Not able to retireve Application details. Database Error.");
                    throw new Exception("CustomizeDeveloperMessage: Not able to retireve Application details. Database Error.");

                }
            }
            return appDetails;
        }

        public int getApplicationID(string appName)
        {
            int appID = -2; 
            
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var application = context.Applications.Where(x => x.applicationName.Equals(appName)).First();
                    if (application != null)
                    {
                        appID = application.ApplicationId;
                    }
                }
                catch
                {
                    Log.Error("Function:getApplicationID CustomizeDeveloperMessage: Not able to get Application ID. Database Error.");
                    throw new Exception("CustomizeDeveloperMessage: Not able to get Application ID. Database Error. ");

                }
            }

            return appID;
        }


    }
}
