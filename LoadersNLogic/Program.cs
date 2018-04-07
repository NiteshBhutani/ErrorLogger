using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoadersNLogic
{
    class Program
    {
        /// <summary>
        /// Entry point into the app
        /// </summary>
        static void Main(string[] args)
        {
        // Do cleanup first, so I don't have to manually delete them..
        //ErrorLoggerHandler.DeleteDB();

        #region Demonstrate Code First

        Console.WriteLine("****** Creation the DB ******");

        // Create the DB
        ErrorLoggerHandler.CreateDB();

        // Doing this twice to prove that the many-to-many relationship works both ways via navigation properties,
        // even though when we seeded it only through Courses
        ErrorLoggerHandler.PullOutDataByUser();
        ErrorLoggerHandler.PullOutDataByApplications();
        
        #endregion

        Console.ReadLine();
    }
}
}
