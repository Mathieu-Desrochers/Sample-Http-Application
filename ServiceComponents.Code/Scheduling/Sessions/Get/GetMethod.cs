
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.Infrastructure.Code.Http;
using SampleHttpApplication.ServiceComponents.Interface;
using SampleHttpApplication.ServiceComponents.Interface.Scheduling.Sessions;

namespace SampleHttpApplication.ServiceComponents.Code.Scheduling.Sessions
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
                // Build the business request.
                GetSessionsBusinessRequest getSessionsBusinessRequest = new GetSessionsBusinessRequest();

                // Invoke the GetSessions business operation.
                GetSessionsBusinessResponse getSessionsBusinessResponse = await this.schedulingBusinessLogicComponent.GetSessions(databaseConnection, getSessionsBusinessRequest);

                // The business operation succeeded.
                return getSessionsBusinessResponse;
            }
            catch (GetSessionsBusinessException ex)
            {
                // Build a service exception.
                ServiceException serviceException = new ServiceException();
                serviceException.ErrorMessage = String.Format("SchedulingServiceComponent.SessionsController.Get() has invoked SessionBusinessLogicComponent.GetSessions() and has caught a GetSessions business exception. See the Errors property for details.");
                
                // Build the Error service exception elements.
                List<ServiceException.ErrorServiceExceptionElement> errorServiceExceptionElements = new List<ServiceException.ErrorServiceExceptionElement>();
                foreach (GetSessionsBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement in ex.Errors)
                {
                    ServiceException.ErrorServiceExceptionElement errorServiceExceptionElement = new ServiceException.ErrorServiceExceptionElement();
                    errorServiceExceptionElement.ErrorCode = errorBusinessExceptionElement.ErrorCode.ToString();
                    errorServiceExceptionElement.Value = errorBusinessExceptionElement.Value;
                    errorServiceExceptionElements.Add(errorServiceExceptionElement);
                }

                // Set the Error service exception elements.
                serviceException.Errors = errorServiceExceptionElements.ToArray();

                // Throw the service exception.
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

                // Build the Sessions resources.
                List<SessionResource> sessionResources = new List<SessionResource>();
                foreach (GetSessionsBusinessResponse.SessionBusinessResponseElement sessionBusinessResponseElement in getSessionsBusinessResponse.Sessions)
                {
                    // Build the Sessions resource.
                    SessionResource sessionResource = new SessionResource();
                    sessionResource.SessionID = sessionBusinessResponseElement.SessionID;
                    sessionResource.SessionCode = sessionBusinessResponseElement.SessionCode;
                    sessionResource.Name = sessionBusinessResponseElement.Name;
                    sessionResource.StartDate = sessionBusinessResponseElement.StartDate;
                    sessionResources.Add(sessionResource);
                }

                // Commit the database transaction.
                databaseTransaction.Commit();

                // Return an HTTP response message containing the Session resources.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse<SessionResource[]>(HttpStatusCode.OK, sessionResources.ToArray());
                return httpResponseMessage;
            }
        }
    }
}
