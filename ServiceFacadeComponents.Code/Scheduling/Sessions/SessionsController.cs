
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.ServiceFacadeComponents.Code.Scheduling.Sessions
{
    /// <summary>
    /// Represents the Sessions controller.
    /// </summary>
    public partial class SessionsController : ApiController
    {
        /// <summary>
        /// The database connection provider.
        /// </summary>
        private readonly IDatabaseConnectionProvider databaseConnectionProvider;

        /// <summary>
        /// The business logic components.
        /// </summary>
        private readonly ISchedulingBusinessLogicComponent schedulingBusinessLogicComponent;

        /// <summary>
        /// Initialization conctructor.
        /// </summary>
        public SessionsController(IDatabaseConnectionProvider databaseConnectionProvider, ISchedulingBusinessLogicComponent schedulingBusinessLogicComponent)
        {
            // Initialize the database connection provider.
            this.databaseConnectionProvider = databaseConnectionProvider;

            // Initialize the business logic components.
            this.schedulingBusinessLogicComponent = schedulingBusinessLogicComponent;
        }

        /// <summary>
        /// Registers the HTTP routes.
        /// </summary>
        public static void RegisterRoutes(HttpRouteCollection httpRouteCollection)
        {
            // Build the HTTP method contraint.
            HttpMethodConstraint httpMethodConstraint = new HttpMethodConstraint(HttpMethod.Get, HttpMethod.Post);

            // Map the HTTP route.
            httpRouteCollection.MapHttpRoute(
                name: "api/scheduling/sessions",
                routeTemplate: "api/scheduling/sessions",
                defaults: new { controller = "Sessions" },
                constraints: new { httpMethods = httpMethodConstraint });
        }
    }
}
