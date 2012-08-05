
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Builds a NewSession business exception.
        /// </summary>
        private NewSessionBusinessException BuildNewSessionBusinessException(NewSessionBusinessException.ErrorBusinessExceptionElement[] errorBusinessExceptionElements)
        {
            // Build the NewSession business exception.
            NewSessionBusinessException businessException = new NewSessionBusinessException();
            businessException.ErrorMessage = String.Format("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.");
            businessException.Errors = errorBusinessExceptionElements;

            // Return the NewSession business exception.
            return businessException;
        }

        /// <summary>
        /// Validates the NewSession business request.
        /// </summary>
        private void ValidateNewSessionRequest(NewSessionBusinessRequest businessRequest)
        {
            // Build the list of errors.
            List<NewSessionBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements = new List<NewSessionBusinessException.ErrorBusinessExceptionElement>();

            // Validate the Session business request element.
            NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = businessRequest.Session;
            errorBusinessExceptionElements.AddIfInvalid(sessionBusinessRequestElement, "SessionCode", sessionBusinessRequestElement.SessionCode, NewSessionBusinessException.ErrorCodes.InvalidSessionCode);
            errorBusinessExceptionElements.AddIfInvalid(sessionBusinessRequestElement, "Name", sessionBusinessRequestElement.Name, NewSessionBusinessException.ErrorCodes.InvalidName);

            // Check if any errors were added to the list.
            if (errorBusinessExceptionElements.Any())
            {
                // Throw a NewSession business exception.
                NewSessionBusinessException businessException = this.BuildNewSessionBusinessException(errorBusinessExceptionElements.ToArray());
                throw businessException;
            }
        }

        /// <summary>
        /// Validates the NewSession business operation.
        /// </summary>
        private async Task ValidateNewSessionOperation(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest, NewSessionOperationData operationData)
        {
            // Validate the DuplicateSessionCode error code.
            operationData.SessionDataRow = await this.sessionDataAccessComponent.ReadBySessionCode(databaseConnection, businessRequest.Session.SessionCode);
            if (operationData.SessionDataRow != null)
            {
                // Throw a NewSession business exception.
                NewSessionBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new NewSessionBusinessException.ErrorBusinessExceptionElement() { ErrorCode = NewSessionBusinessException.ErrorCodes.DuplicateSessionCode, Value = businessRequest.Session.SessionCode };
                NewSessionBusinessException businessException = this.BuildNewSessionBusinessException(new NewSessionBusinessException.ErrorBusinessExceptionElement[] { errorBusinessExceptionElement });
                throw businessException;
            }
        }

        /// <summary>
        /// Executes the NewSession business operation.
        /// </summary>
        public async virtual Task<NewSessionBusinessResponse> NewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest)
        {
            // Initialize the operation data.
            NewSessionOperationData operationData = new NewSessionOperationData();

            // Validate the business request.
            this.ValidateNewSessionRequest(businessRequest);

            // Validate the business operation.
            await this.ValidateNewSessionOperation(databaseConnection, businessRequest, operationData);

            // Create the Session data row.
            operationData.SessionDataRow = new SessionDataRow();
            operationData.SessionDataRow.SessionCode = businessRequest.Session.SessionCode;
            operationData.SessionDataRow.Name = businessRequest.Session.Name;
            operationData.SessionDataRow.StartDate = businessRequest.Session.StartDate;
            await this.sessionDataAccessComponent.Create(databaseConnection, operationData.SessionDataRow);

            // Build the business response.
            NewSessionBusinessResponse businessResponse = new NewSessionBusinessResponse();

            // Build the Session business response element.
            NewSessionBusinessResponse.SessionBusinessResponseElement sessionBusinessResponseElement = new NewSessionBusinessResponse.SessionBusinessResponseElement();
            sessionBusinessResponseElement.SessionID = operationData.SessionDataRow.SessionID;
            businessResponse.Session = sessionBusinessResponseElement;

            // Return the business response.
            return businessResponse;
        }
    }
}
