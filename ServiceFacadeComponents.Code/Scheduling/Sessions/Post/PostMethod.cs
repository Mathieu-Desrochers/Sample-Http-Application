﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
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
        /// Invokes the NewSession business operation.
        /// </summary>
        private async Task<NewSessionBusinessResponse> InvokeNewSession(IDatabaseConnection databaseConnection, SessionResource resource)
        {
            try
            {
                // Build the NewSession business request.
                NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

                // Build the Session business request element.
                if (resource.Session != null)
                {
                    NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewSessionBusinessRequest.SessionBusinessRequestElement();
                    sessionBusinessRequestElement.Name = resource.Session.Name;
                    sessionBusinessRequestElement.StartDate = resource.Session.StartDate;
                    newSessionBusinessRequest.Session = sessionBusinessRequestElement;
                }

                // Invoke the NewSession business operation.
                NewSessionBusinessResponse newSessionBusinessResponse = await this.schedulingBusinessLogicComponent.NewSession(databaseConnection, newSessionBusinessRequest);

                // The business operation succeeded.
                return newSessionBusinessResponse;
            }
            catch (NewSessionBusinessException newSessionBusinessException)
            {
                // Wrap the NewSession business exception into a service exception.
                ServiceException serviceException = ServiceExceptionBuilder.BuildServiceException(
                    "SchedulingServiceComponent.SessionsController.Post()",
                    newSessionBusinessException,
                    newSessionBusinessException.Errors.Select(error => error.ErrorCode.ToString()).ToArray(),
                    newSessionBusinessException.Errors.Select(error => error.ErroneousValue).ToArray());
                
                // Throw the service exception.
                throw serviceException;
            }
        }

        /// <summary>
        /// Executes the HTTP POST method.
        /// </summary>
        public async Task<HttpResponseMessage> Post(SessionResource resource)
        {
            // Make sure the resource is valid.
            bool resourceIsNull = resource == null;
            bool resourceIsInvalid = this.ModelState.IsValid == false;
            if (resourceIsNull || resourceIsInvalid)
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                HttpResponseException httpResponseException = new HttpResponseException(httpResponseMessage);
                throw httpResponseException; 
            }

            // Open a database connection and begin a database transaction.
            using (IDatabaseConnection databaseConnection = await this.databaseConnectionProvider.OpenDatabaseConnection())
            using (IDatabaseTransaction databaseTransaction = databaseConnection.BeginDatabaseTransaction())
            {
                // Invoke the NewSession business operation.
                NewSessionBusinessResponse newSessionBusinessResponse = await this.InvokeNewSession(databaseConnection, resource);

                // Update the Session resource element.
                if (resource.Session != null)
                {
                    resource.Session.SessionCode = newSessionBusinessResponse.Session.SessionCode;
                }

                // Commit the database transaction.
                databaseTransaction.Commit();

                // Build an HTTP response message.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(HttpStatusCode.Created, resource);
                return httpResponseMessage;
            }
        }
    }
}
