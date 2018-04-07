using System;
using System.Collections.Generic;
using System.Data.Entity;
namespace ErrorLoggerDBLayer
{
    using AuthenticationLib;
    using ErrorLoggerConstant;

    /// <summary>
    /// A custom initializer that will seed the data in.
    /// 
    /// There are 3 types of Initializers..
    ///     DropCreateDatabaseIfModelChanges
    ///     CreateDatabaseIfNotExists
    ///     DropCreateDatabaseAlways
    /// </summary>
    public class ErrorLoggerDBInitializer : DropCreateDatabaseIfModelChanges<ErrorLoggerDBContext>
    {   
        // <summary>
        /// Seeds data into the DB
        /// </summary>
        protected override void Seed(ErrorLoggerDBContext context)
        {
            Console.WriteLine(" ### Seeding ###");

            Application StackOverflow_ = new Application()
            {
                //applicationID = 1,
                applicationName = "Stack Overflow",
                applicationDescription = "Stack Overflow is programmers Q/A website",
                applicationStatus = "Active",
            };

            Application mongodb_ = new Application()
            {
                //applicationId = 2,
                applicationName = "Mongo DB",
                applicationDescription = "MongoDb is No-SQL DB having several benifits over traditional DB",
                applicationStatus = "Active"

            };

            Application Facebook_ = new Application()
            {
                //applicationId = 2,
                applicationName = "Facebook",
                applicationDescription = "Social Media Site",
                applicationStatus = "NonActive"

            };

            string login1Hashedpassword = Authentication.HashPassword("dpalider", "dusanpalider");
            Login Login1 = new Login()
            {
                emailID = "dpalider@syr.edu",
                password = login1Hashedpassword
            };

            string login2Hashedpassword = Authentication.HashPassword("nibhutan", "niteshbhutani");
            Login Login2 = new Login()
            {
                emailID = "nibhutan@syr.edu",
                password = login2Hashedpassword
            };

            Error error1 = new Error()
            {
                errorDescription = "Error for StackoverFlow Application",
                logLevel = LogLevelEnum.Debug,
                applicationID = 1
            };

            Error error2 = new Error()
            {
                errorDescription = "Error for MongoDB Application",
                logLevel = LogLevelEnum.Error,
                applicationID = 2

            };

            
            User user1 = new User()
            {   loginDetails = Login1,
                firstName = "Dusan" ,
                lastName = "Palider",
                roleID = "User" ,
                statusText ="Active",
                Applications = new List<Application>() { StackOverflow_, mongodb_ }
                
            };
            User user2 = new User()
            {
                loginDetails = Login2,
                firstName = "nitesh",   
                lastName = "bhutani",
                roleID = "Admin",
                statusText = "Active",
                Applications = new List<Application>() { StackOverflow_ }
                
            };

            // The order is important, since we are setting up references
            context.Applications.Add(StackOverflow_);
            context.Applications.Add(mongodb_);
            context.Errors.Add(error1);
            context.Errors.Add(error2);
            context.Logins.Add(Login1);
            context.Logins.Add(Login2);
            context.Users.Add(user1);
            context.Users.Add(user2);

            // letting the base method do anything it needs to get done
            base.Seed(context);

            // Save the changes you made, when adding the data above
            context.SaveChanges();
        }
    }
}
