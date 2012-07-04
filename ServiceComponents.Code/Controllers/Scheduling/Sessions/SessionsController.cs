
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

using SampleHttpApplication.BusinessLogicComponents.Code.Scheduling;
using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling;
using SampleHttpApplication.DataAccessComponents.Code;
using SampleHttpApplication.DataAccessComponents.Code.Session;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.ServiceComponents.Code.Controllers.Scheduling.Sessions
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
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.MapHttpRoute(
                name: "scheduling/sessions",
                routeTemplate: "scheduling/sessions",
                defaults: new { controller = "Sessions" },
                constraints: new { httpMethods = new HttpMethodConstraint("GET", "POST") });
        }
    }
}
