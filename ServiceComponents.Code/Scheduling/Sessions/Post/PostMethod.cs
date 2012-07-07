
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.DataAccessComponents.Interface;
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
        /// Invokes the NewSession business operation.
        /// </summary>
        private async Task<NewSessionBusinessResponse> InvokeNewSession(IDatabaseConnection databaseConnection, SessionResource sessionResource)
        {
            try
            {
                // Build the business request.
                NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

                // Build the Session business request element.
                NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewSessionBusinessRequest.SessionBusinessRequestElement();
                sessionBusinessRequestElement.SessionCode = sessionResource.SessionCode;
                sessionBusinessRequestElement.Name = sessionResource.Name;
                sessionBusinessRequestElement.StartDate = sessionResource.StartDate;
                newSessionBusinessRequest.Session = sessionBusinessRequestElement;

                // Invoke the NewSession business operation.
                NewSessionBusinessResponse newSessionBusinessResponse = await this.schedulingBusinessLogicComponent.NewSession(databaseConnection, newSessionBusinessRequest);

                // The business operation succeeded.
                return newSessionBusinessResponse;
            }
            catch (NewSessionBusinessException ex)
            {
                // Wrap the business exception into a service exception.
                string serviceExceptionMessage = String.Format("The SchedulingServiceComponent.SessionsController.Post() service operation has caught the error code {0} while invoking the SessionBusinessLogicComponent.NewSession() business operation.", ex.ErrorCode);
                ServiceException serviceException = new ServiceException(serviceExceptionMessage, ex);

                // Build the service exception details.
                ServiceExceptionDetails serviceExceptionDetails = new ServiceExceptionDetails();
                serviceExceptionDetails.ErrorCode = ex.ErrorCode.ToString();
                serviceException.Details = serviceExceptionDetails;

                // Throw the service exception.
                throw serviceException;
            }
        }

        /// <summary>
        /// Executes the HTTP POST method.
        /// </summary>
        public async Task<HttpResponseMessage> Post(SessionResource sessionResource)
        {
            // Make sure the NewSession service request...
            if (!this.ModelState.IsValid)
            {
                HttpResponseMessage badRequest = this.Request.CreateResponse(HttpStatusCode.BadRequest);
                return badRequest;
            }

            // Open a database connection.
            // Well, a transaction scope would be usefull here.
            // Watchout for: TransactionScope doit être supprimé sur le même thread que celui où il a été créé
            using (IDatabaseConnection databaseConnection = await this.databaseConnectionProvider.OpenDatabaseConnection())
            {
                // Invoke the NewSession business operation.
                NewSessionBusinessResponse newSessionBusinessResponse = await this.InvokeNewSession(databaseConnection, sessionResource);

                // Update the Session resource.
                sessionResource.SessionID = newSessionBusinessResponse.Session.SessionID;

                // Build the HTTP response message with the Created status code.
                // Set the content to the Session resource.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse<SessionResource>(HttpStatusCode.Created, sessionResource);

                // Return the HTTP response message.
                return httpResponseMessage;
            }
        }
    }
}
