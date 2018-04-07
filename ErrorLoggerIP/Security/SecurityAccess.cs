using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ErrorLoggerIP.Security
{ using ErrorLoggerConstant;
        /// <summary>
        /// Class used to store Security Matrix settings
        /// </summary>
        public class SecurityAccess
        {
            /// <summary>
            /// Action
            /// </summary>
            public string Action { get; set; }

            /// <summary>
            /// Controller
            /// </summary>
            public string Controller { get; set; }

            /// <summary>
            /// Minimum needed role
            /// </summary>
            public RoleEnum MinimumRoleNeeded { get; set; }
        }

 }
