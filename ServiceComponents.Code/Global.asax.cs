
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using SampleHttpApplication.Infrastructure.Code.DependencyInjection;
using SampleHttpApplication.ServiceComponents.Code.Scheduling.Sessions;

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
            // Register our dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = DependencyResolverBuilder.BuildDependencyResolver();

            // Register the controller routes.
            SessionsController.RegisterRoutes(RouteTable.Routes);
        }
    }
}
