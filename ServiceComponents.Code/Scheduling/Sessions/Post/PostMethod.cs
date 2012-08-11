
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http.ModelBinding;

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
                // Build the ErrorCode service exception.
                ErrorCodeServiceException errorCodeServiceException = new ErrorCodeServiceException();
                errorCodeServiceException.ErrorMessage = String.Format("SchedulingServiceComponent.SessionsController.Post() has invoked SessionBusinessLogicComponent.NewSession() and has caught a NewSession business exception. See the ErrorCodes property for details.");

                // Build the ErrorCode service exception elements.
                List<ErrorCodeServiceException.ErrorCodeServiceExceptionElement> errorCodeServiceExceptionElements = new List<ErrorCodeServiceException.ErrorCodeServiceExceptionElement>();
                foreach (NewSessionBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement in ex.Errors)
                {
                    // Build the ErrorCode service exception element.
                    ErrorCodeServiceException.ErrorCodeServiceExceptionElement errorCodeServiceExceptionElement = new ErrorCodeServiceException.ErrorCodeServiceExceptionElement();
                    errorCodeServiceExceptionElement.ErrorCode = errorBusinessExceptionElement.ErrorCode.ToString();
                    errorCodeServiceExceptionElement.Value = errorBusinessExceptionElement.Value;
                    errorCodeServiceExceptionElements.Add(errorCodeServiceExceptionElement);
                }

                // Set the ErrorCode service exception elements.
                errorCodeServiceException.ErrorCodes = errorCodeServiceExceptionElements.ToArray();

                // Throw the ErrorCode service exception.
                throw errorCodeServiceException;
            }
        }

        /// <summary>
        /// Executes the HTTP POST method.
        /// </summary>
        public async Task<HttpResponseMessage> Post(SessionResource sessionResource)
        {
            // Make sure the Session resource is valid.
            if (!this.ModelState.IsValid)
            {
                // Build the BadFormat service exception.
                BadFormatServiceException badFormatServiceException = new BadFormatServiceException();
                badFormatServiceException.ErrorMessage = "SchedulingServiceComponent.SessionsController.Post() was invoked with badly formatted values. See the BadFormats property for details.";

                // Build the BadFormat service exception elements.
                List<BadFormatServiceException.BadFormatServiceExceptionElement> badFormatServiceExceptionElements = new List<BadFormatServiceException.BadFormatServiceExceptionElement>();
                foreach (KeyValuePair<string, ModelState> invalidModelState in this.ModelState.Where(modelStateEntry => modelStateEntry.Value.Errors.Any()))
                {
                    // Build the BadFormat service exception element.
                    BadFormatServiceException.BadFormatServiceExceptionElement badFormatServiceExceptionElement = new BadFormatServiceException.BadFormatServiceExceptionElement();
                    badFormatServiceExceptionElement.BadFormat = invalidModelState.Key.Split('.').Last();
                    badFormatServiceExceptionElements.Add(badFormatServiceExceptionElement);
                }

                // Set the BadFormat service exception elements.
                badFormatServiceException.BadFormats = badFormatServiceExceptionElements.ToArray();

                // Throw the BadFormat service exception.
                throw badFormatServiceException;
            }

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
