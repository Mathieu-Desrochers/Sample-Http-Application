
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents a Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Validates the NewSession business request.
        /// </summary>
        private async Task<NewSessionBusinessException.ErrorCodes?> ValidateNewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest, NewSessionOperationData operationData)
        {
            // Validate the session code.
            if (!ValidatorHelper.ValidateProperty("SessionCode", businessRequest.Session, businessRequest.Session.SessionCode))
            {
                return NewSessionBusinessException.ErrorCodes.InvalidSessionCode;
            }

            // Validate the session code is not already in use.
            operationData.SessionDataRow = await this.sessionDataAccessComponent.ReadBySessionCode(databaseConnection, businessRequest.Session.SessionCode);
            if (operationData.SessionDataRow != null)
            {
                return NewSessionBusinessException.ErrorCodes.DuplicateSessionCode;
            }

            // Validate the name.
            if (!ValidatorHelper.ValidateProperty("Name", businessRequest.Session, businessRequest.Session.Name))
            {
                return NewSessionBusinessException.ErrorCodes.InvalidName;
            }

            // Validate the start date.
            if (!ValidatorHelper.ValidateProperty("StartDate", businessRequest.Session, businessRequest.Session.StartDate))
            {
                return NewSessionBusinessException.ErrorCodes.InvalidStartDate;
            }

            // The business request is valid.
            return null;
        }

        /// <summary>
        /// Executes the NewSession business operation.
        /// </summary>
        public async Task<NewSessionBusinessResponse> NewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest)
        {
            // Initialize the operation data.
            NewSessionOperationData operationData = new NewSessionOperationData();

            // Validate the business request.
            NewSessionBusinessException.ErrorCodes? errorCode = await this.ValidateNewSession(databaseConnection, businessRequest, operationData);
            if (errorCode.HasValue)
            {
                NewSessionBusinessException businessException = new NewSessionBusinessException();
                businessException.ErrorCode = errorCode.Value;
                businessException.ErrorMessage = this.FormatErrorMessage("NewSession", errorCode.Value.ToString());
                throw businessException;
            }

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
