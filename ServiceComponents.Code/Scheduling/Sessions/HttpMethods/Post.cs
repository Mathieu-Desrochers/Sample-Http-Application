
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
using SampleHttpApplication.ServiceComponents.Interface.Scheduling.Sessions;

namespace SampleHttpApplication.ServiceComponents.Code.Scheduling.Sessions
{
    /// <summary>
    /// Represents the Sessions controller.
    /// </summary>
    public partial class SessionsController
    {
        /// <summary>
        /// Handles the HTTP POST method.
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

                // Update the Session resource.
                sessionResource.SessionID = newSessionBusinessResponse.Session.SessionID;

                // Return the NewSession service response.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse<SessionResource>(HttpStatusCode.Created, sessionResource);
                return httpResponseMessage;
            }
        }
    }
}
