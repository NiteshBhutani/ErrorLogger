using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ErrorLoggerModel
{
    public class AdminIndexViewModel
    {
        public AdminIndexViewModel()
        {
            userID = new List<int>();
            applicationID = new List<int>();
            applicationName = new List<string>();
            userFirstName = new List<string>();
            userLastName = new List<string>();
            userLastLoginTimestamp = new List<string>();

        }
        public List<int> userID { get; set; }
        public List<int> applicationID { get; set; }
        public List<string> applicationName { get; set; }
        public List<string> userFirstName { get; set; }
        public List<string> userLastName { get; set; }
        public List<string> userLastLoginTimestamp { get; set; }
    }
}
