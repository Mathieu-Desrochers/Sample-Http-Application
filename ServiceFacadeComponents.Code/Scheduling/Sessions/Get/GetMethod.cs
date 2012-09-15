
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.ServiceFacadeComponents.Interface;
using SampleHttpApplication.ServiceFacadeComponents.Interface.Scheduling.Sessions;

namespace SampleHttpApplication.ServiceFacadeComponents.Code.Scheduling.Sessions
{
    /// <summary>
    /// Represents the Sessions controller.
    /// </summary>
    public partial class SessionsController
    {
        /// <summary>
        /// Invokes the GetSessions business operation.
        /// </summary>
        private async Task<GetSessionsBusinessResponse> InvokeGetSessions(IDatabaseConnection databaseConnection)
        {
            try
            {
                // Build the GetSessions business request.
                GetSessionsBusinessRequest getSessionsBusinessRequest = new GetSessionsBusinessRequest();

                // Invoke the GetSessions business operation.
                GetSessionsBusinessResponse getSessionsBusinessResponse = await this.schedulingBusinessLogicComponent.GetSessions(databaseConnection, getSessionsBusinessRequest);

                // The business operation succeeded.
                return getSessionsBusinessResponse;
            }
            catch (GetSessionsBusinessException getSessionsBusinessException)
            {
                // Wrap the GetSessions business exception into a service exception.
                ServiceException serviceException = ServiceExceptionBuilder.BuildServiceException(
                    "SchedulingServiceComponent.SessionsController.Get()",
                    getSessionsBusinessException,
                    getSessionsBusinessException.Errors.Select(error => error.ErrorCode.ToString()).ToArray(),
                    getSessionsBusinessException.Errors.Select(error => error.ErroneousValue).ToArray());

                // Throw the ErrorCode service exception.
                throw serviceException;
            }
        }

        /// <summary>
        /// Executes the HTTP GET method.
        /// </summary>
        public async Task<HttpResponseMessage> Get()
        {
            // Open a database connection and begin a database transaction.
            using (IDatabaseConnection databaseConnection = await this.databaseConnectionProvider.OpenDatabaseConnection())
            using (IDatabaseTransaction databaseTransaction = databaseConnection.BeginDatabaseTransaction())
            {
                // Invoke the GetSessions business operation.
                GetSessionsBusinessResponse getSessionsBusinessResponse = await this.InvokeGetSessions(databaseConnection);

                // Build the Session resources.
                List<SessionResource> sessionResources = new List<SessionResource>();
                foreach (GetSessionsBusinessResponse.SessionBusinessResponseElement sessionBusinessResponseElement in getSessionsBusinessResponse.Sessions)
                {
                    // Build the Session resource.
                    SessionResource sessionResource = new SessionResource();
                    sessionResources.Add(sessionResource);
                    
                    // Build the Session resource element.
                    SessionResource.SessionResourceElement sessionResourceElement = new SessionResource.SessionResourceElement();
                    sessionResourceElement.SessionCode = sessionBusinessResponseElement.SessionCode;
                    sessionResourceElement.Name = sessionBusinessResponseElement.Name;
                    sessionResourceElement.StartDate = sessionBusinessResponseElement.StartDate;
                    sessionResource.Session = sessionResourceElement;
                }

                // Commit the database transaction.
                databaseTransaction.Commit();

                // Return an HTTP response message.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(HttpStatusCode.OK, sessionResources.ToArray());
                return httpResponseMessage;
            }
        }
    }
}
