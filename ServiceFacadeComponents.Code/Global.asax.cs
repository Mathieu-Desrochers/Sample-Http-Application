
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

using Newtonsoft.Json.Serialization;

using SampleHttpApplication.Infrastructure.Code.DependencyInjection;
using SampleHttpApplication.ServiceFacadeComponents.Code.Scheduling.Sessions;
using SampleHttpApplication.ServiceFacadeComponents.Interface;

namespace SampleHttpApplication.ServiceFacadeComponents.Code
{
    /// <summary>
    /// Represents the HTTP application.
    /// </summary>
    public class GlobalHttpApplication : HttpApplication
    {
        /// <summary>
        /// Configures the application.
        /// </summary>
        public static void ConfigureApplication(HttpConfiguration httpConfiguration)
        {
            // Configure the JSON formatter.
            httpConfiguration.Formatters.JsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Register the error handling filters.
            httpConfiguration.Filters.Add(new ServiceExceptionFilterAttribute());
            httpConfiguration.Filters.Add(new UnhandledExceptionFilterAttribute());

            // Register the controller routes.
            SessionsController.RegisterRoutes(httpConfiguration.Routes);
        }

        /// <summary>
        /// Runs when the HTTP application starts.
        /// </summary>
        protected void Application_Start()
        {
            // Configure the application.
            GlobalHttpApplication.ConfigureApplication(GlobalConfiguration.Configuration);

            // Register the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = DependencyResolverBuilder.BuildDependencyResolver();
        }
    }
}
