
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using SampleHttpApplication.ServiceComponents.Code.Controllers.Scheduling.Sessions;

namespace SampleHttpApplication.ServiceComponents.Code
{
    /// <summary>
    /// Represents the HTTP application.
    /// </summary>
    public class MvcApplication : HttpApplication
    {
        /// <summary>
        /// Runs when the HTTP application starts.
        /// </summary>
        protected void Application_Start()
        {
            SessionsController.RegisterRoutes(RouteTable.Routes);
        }
    }
}
