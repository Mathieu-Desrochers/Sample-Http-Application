
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
                // Build a service exception.
                ServiceException serviceException = new ServiceException();
                serviceException.ErrorMessage = String.Format("SchedulingServiceComponent.SessionsController.Get() has invoked SessionBusinessLogicComponent.NewSession() and has caught a NewSession business exception. See the Errors property for details.");

                // Build the Error service exception elements.
                List<ServiceException.ErrorServiceExceptionElement> errorServiceExceptionElements = new List<ServiceException.ErrorServiceExceptionElement>();
                foreach (NewSessionBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement in ex.Errors)
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
        /// Executes the HTTP POST method.
        /// </summary>
        public async Task<HttpResponseMessage> Post(SessionResource sessionResource)
        {
            //// Make sure the Session resource is valid.
            //if (!this.ModelState.IsValid)
            //{
            //    // Build the InvalidFields service exception.
            //    InvalidFieldsServiceException invalidFieldsServiceException = new InvalidFieldsServiceException();
            //    invalidFieldsServiceException.ErrorMessage = "SchedulingServiceComponent.SessionsController.Post() was invoked with invalid fields.";
            //    invalidFieldsServiceException.Details.InvalidFields = this.ModelState.GetInvalidFields();

            //    // Throw the InvalidFields service exception.
            //    throw invalidFieldsServiceException;
            //}

            // Open a database connection and begin a database transaction.
            using (IDatabaseConnection databaseConnection = await this.databaseConnectionProvider.OpenDatabaseConnection())
            using (IDatabaseTransaction databaseTransaction = databaseConnection.BeginDatabaseTransaction())
            {
                // Invoke the NewSession business operation.
                NewSessionBusinessResponse newSessionBusinessResponse = await this.InvokeNewSession(databaseConnection, sessionResource);

                // Update the Session resource.
                sessionResource.SessionID = newSessionBusinessResponse.Session.SessionID;

                // Commit the database transaction.
                databaseTransaction.Commit();

                // Build an HTTP response message containing the Session resource.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse<SessionResource>(HttpStatusCode.Created, sessionResource);
                return httpResponseMessage;
            }
        }
    }
}
