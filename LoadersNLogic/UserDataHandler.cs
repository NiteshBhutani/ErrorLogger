using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.SqlClient;

namespace LoadersNLogic
{
    using ErrorLoggerModel;
    using ErrorLoggerDBLayer;
    using AuthenticationLib;
    using System.Web.Mvc;
    using log4net;
    public class UserDataHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<User> userData = new List<User>();

        public UserDataHandler()
        {

            // the using statement will make sure the object is disposed when it goes out of scope
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    foreach (User user in context.Users
                        .Include(x => x.loginDetails) // Eager Loading 
                        .ToList())
                    {
                        //Console.WriteLine(String.Format("  " + "User Id: {0}, User EmailID: {1}",
                        //    user.userModelId, user.emailId));
                        userData.Add(user);
                    }
                }
                catch(Exception ex)
                {
                    Log.Error("Function: Constructor CustomizeDeveloperMessage: Unable to initialize user application page. Database Error Exception Message : "+ex.Message);
                    throw new Exception("CustomizeDeveloperMessage: Unable to initialize user application page. Database Error");
                }
            }
        }

        public ICollection<User> GetAllUserData()
        {
            
            return userData;
        }

        public bool AddUser(RegisterUserViewModel newUser)
        {
            bool result = false;
            //hashing the password
            string hashedPassword = string.Empty;
            try
            {
                char[] delim = { '@' };
                string[] userNameAndDomain = newUser.Email.Split(delim);   
                hashedPassword = Authentication.HashPassword(userNameAndDomain[0], newUser.Password);
            }
            catch
            {
                Log.Error("Function : AddUser CustomizeDeveloperMessage: Registration Failed due to server Error. Unable to hash given password.");
                throw new Exception("CustomizeDeveloperMessage: Registration Failed due to server Error. Unable to hash given password.");
            }

            Login login = new Login()
            {
                emailID = newUser.Email,
                password = hashedPassword
            };
            User user = new User()
            {
                loginDetails = login,
                firstName = newUser.FirstName,
                lastName = newUser.LastName,
                roleID = "User", //by Default User
                statusText = newUser.Status.ToString()
                
            };

            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    if (!userData.Any(x => x.loginDetails.emailID == login.emailID))
                    {
                        context.Logins.Add(login);
                        context.Users.Add(user);
                        context.SaveChanges();
                        result = true;
                    }
                }
                catch
                {
                    Log.Error("Function: AddUser CustomizeDeveloperMessage: Registration Failed. Database Access Error. ");
                    throw new Exception("CustomizeDeveloperMessage: Registration Failed. Database Access Error.");
                    
                }
            }

            return result;
        }

        public List<string> checkIfUserExist(LoginUserViewModel loginUser)
        {
            List<string> loggedUser = new List<string>();
            string hashedPassword = string.Empty; 
            try
            {
                char[] delim = { '@' };
                string[] userNameAndDomain = loginUser.Email.Split(delim);
                hashedPassword = Authentication.HashPassword(userNameAndDomain[0], loginUser.Password);
            }
            catch
            {
                Log.Error("Function:checkIfUserExist CustomizeDeveloperMessage: Login Failed due to server Error. ");
                throw new Exception("CustomizeDeveloperMessage: Login Failed due to server Error.");   
             }
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user = context.Users.Where(x => x.loginDetails.emailID.Equals(loginUser.Email) && x.loginDetails.password.Equals(hashedPassword)&& x.statusText.Equals("Active"))
                        .Include(x => x.loginDetails).FirstOrDefault();
                    loggedUser.Add(user.loginDetails.emailID);
                    loggedUser.Add(user.UserId.ToString());
                }
                catch
                {
                    Log.Error("Function:checkIfUserExist CustomizeDeveloperMessage:Login Failed. Check if password is correct otherwise contact the admin.");
                    throw new Exception("CustomizeDeveloperMessage: Login Failed. Check if password is correct otherwise contact the admin.");
                }
            }
                return loggedUser;
        }

        public string getUserRole(string userName)
        {
            string role= String.Empty;

            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user = context.Users.Where(x => x.loginDetails.emailID.Equals(userName)).FirstOrDefault();
                    role = user.roleID;
                }
                catch(Exception ex)
                {
                   Log.Error("Function:getUserRole  CustomizeDeveloperMessage: "+ ex.Message);             
                }
            }

            return role;
        }

        public bool checkIsUserAuthorizedForApplicationID(string userName, int appID)
        {
            bool result = false;
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user = context.Users.Where(x => x.loginDetails.emailID.Equals(userName)).First();
                    foreach(Application app in user.Applications)
                    {
                        if (app.ApplicationId == appID && app.applicationStatus.Equals("Active"))
                            result = true;
                    }
                    
                }
                catch
                {
                    Log.Error("Function:getUserRole  CustomizeDeveloperMessage: System Authentication Failed. Check Database connection.");
                    throw new Exception("CustomizeDeveloperMessage: System Authentication Failed. Check Database connection.");
                }
            }
            return result;
        }

        public void updateLastLoginTimestamp(String emailID)
        {
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user = context.Users.Where(x => x.loginDetails.emailID.Equals(emailID)).First();
                    user.lastLoginTimestamp = DateTime.Now.ToString();
                    context.SaveChanges();
                }
                catch
                {
                    Log.Error("Function: updateLastLoginTimestamp CustomizeDeveloperMessage: unable to update timestamp");
                    throw new Exception("CustomizeDeveloperMessage: Login Failed. Database Error.");
                }
            }
        }
        
        public ICollection<UserIndexViewModel> GetAllApplicationForUser(String emailID)
        {
            List<UserIndexViewModel> userAppLogs = new List<UserIndexViewModel>();
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user = context.Users.Where(x => x.loginDetails.emailID.Equals(emailID)).First();

                   foreach (Application app in user.Applications)
                    {
                        if (app.applicationStatus.Equals("Active"))
                        {
                            UserIndexViewModel application = new UserIndexViewModel
                            {
                                applicationName = app.applicationName,
                                applicationStatus = app.applicationStatus,
                                applicationID = app.ApplicationId
                            };
                            userAppLogs.Add(application);
                        }
                    }
                        
                    
                }
                catch
                {
                    Log.Error("Function: GetAllApplicationForUser" + string.Format(" CustomizeDeveloperMessage: Not able to get all the applications for {0} ", emailID));
                    throw new Exception(string.Format("CustomizeDeveloperMessage: Not able to get all the applications for {0} ", emailID));
                }
            }
            return userAppLogs;
        }

        public UserDetailsMV getUserDetails(int userID)
        {
            UserDetailsMV userDetails = new UserDetailsMV();
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user = context.Users.Where(x => x.UserId.Equals(userID)).First();
                    if (user != null)
                    {
                        userDetails.UserID = user.UserId;
                        userDetails.Email = user.loginDetails.emailID;
                        userDetails.Role = user.roleID;
                        userDetails.FirstName = user.firstName;
                        userDetails.LastName = user.lastName;
                        userDetails.Status = user.statusText;
                        userDetails.lastLoginTimestamp = user.lastLoginTimestamp;
                        foreach (Application a in user.Applications)
                        {
                            userDetails.applications.Add(a.applicationName);
                        }
                    }
                }
                catch 
                {
                    Log.Error("Function: getUserDetails CustomizeDeveloperMessage: Not able to retireve User details. Database Error. ");
                    throw new Exception("CustomizeDeveloperMessage: Not able to retireve User details. Database Error.");

                }
            }
            return userDetails;
        }

        public EditUserMV setEditData(int userID)
        {
            EditUserMV applications = new EditUserMV();
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                  
                    var app = context.Applications.Select(c => new
                    {
                        AppID = c.ApplicationId,
                        ApplicationName = c.applicationName
                    }).ToList();
                    app.Insert(0,new { AppID = -1, ApplicationName = "Null" });
                    var user = context.Users.Where(x => x.UserId.Equals(userID)).First();
                    applications.appID = new int[user.Applications.Count]; 
                    if (user != null)
                    {
                        applications.UserID = user.UserId;
                        applications.Email = user.loginDetails.emailID;
                        applications.Role = user.roleID;
                        applications.FirstName = user.firstName;
                        applications.LastName = user.lastName;
                        applications.Status = user.statusText;
                        applications.lastLoginTimestamp = user.lastLoginTimestamp;
                        int i = 0;
                        foreach (Application a in user.Applications)
                        {
                            applications.appID[i] = a.ApplicationId;
                            i++;
                        }
                    }
                    applications.Applications = new MultiSelectList(app, "AppID", "ApplicationName",applications.appID);


                }
                catch
                {
                    Log.Error("Function: setEditData CustomizeDeveloperMessage: Not able to retireve all Applications. Database Error.");
                    throw new Exception("CustomizeDeveloperMessage: Not able to retireve all Applications. Database Error.");

                }
            }
            return applications;
        }

        public bool updateUserInDB(EditUserMV user)
        {
            bool result = false;
            using (ErrorLoggerDBContext context = new ErrorLoggerDBContext())
            {
                try
                {
                    var user_ = context.Users.Include(x => x.loginDetails).Where(x => x.UserId != user.UserID).ToList();
                    foreach (User a in user_)
                    {
                        if (a.loginDetails.emailID == user.Email)
                            return false;
                    }
                    var updateUser = context.Users.Where(x => x.UserId.Equals(user.UserID)).First();
                    updateUser.loginDetails.emailID = user.Email;
                    updateUser.firstName = user.FirstName;
                    updateUser.lastName = user.LastName;
                    updateUser.roleID = user.Role;
                    updateUser.statusText = user.Status;
                    updateUser.Applications.Clear();
                    foreach (int appID in user.submittedApplications)
                    {
                        if(appID == -1)
                        {
                            break;
                        }
                        var application = context.Applications.Where(x => x.ApplicationId.Equals(appID)).First();
                        updateUser.Applications.Add(application);
                      }
                    context.SaveChanges();
                    result = true;

                }
                catch
                {
                    Log.Error("Function:updateUserInDB CustomizeDeveloperMessage: Not able to update Application in DB. Database Error.");
                    throw new Exception("CustomizeDeveloperMessage: Not able to update Application in DB. Database Error. ");

                }
            }

            return result;
        }

    }
}

