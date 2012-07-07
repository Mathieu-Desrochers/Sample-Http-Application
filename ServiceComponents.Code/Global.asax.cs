
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using SampleHttpApplication.Infrastructure.Code.DependencyInjection;
using SampleHttpApplication.ServiceComponents.Code.Scheduling.Sessions;
using SampleHttpApplication.ServiceComponents.Interface;

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
            // Register the error handling filters.
            GlobalConfiguration.Configuration.Filters.Add(new ServiceExceptionFilterAttribute());
            GlobalConfiguration.Configuration.Filters.Add(new UnhandledExceptionFilterAttribute());

            // Register the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = DependencyResolverBuilder.BuildDependencyResolver();

            // Register the controller routes.
            SessionsController.RegisterRoutes(RouteTable.Routes);
        }
    }
}
