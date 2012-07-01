
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
        /// Throws a NewSession business exception.
        /// </summary>
        private void ThrowNewSessionBusinessException(NewSessionBusinessException.ErrorCodes errorCode)
        {
            // Build the NewSession business exception.
            NewSessionBusinessException businessException = new NewSessionBusinessException();
            businessException.ErrorMessage = String.Format("The SchedulingBusinessLogicComponent.NewSession() business operation has thrown the {0} error code.", errorCode);
            businessException.ErrorCode = errorCode;

            // Throw the NewSession business exception.
            throw businessException;
        }

        /// <summary>
        /// Validates the NewSession business request.
        /// </summary>
        private async Task ValidateNewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest, NewSessionOperationData operationData)
        {
            // Validate the InvalidSessionCode error code.
            if (!ValidatorHelper.ValidateProperty("SessionCode", businessRequest.Session, businessRequest.Session.SessionCode))
            {
                ThrowNewSessionBusinessException(NewSessionBusinessException.ErrorCodes.InvalidSessionCode);
            }

            // Validate the DuplicateSessionCode error code.
            operationData.SessionDataRow = await this.sessionDataAccessComponent.ReadBySessionCode(databaseConnection, businessRequest.Session.SessionCode);
            if (operationData.SessionDataRow != null)
            {
                ThrowNewSessionBusinessException(NewSessionBusinessException.ErrorCodes.DuplicateSessionCode);
            }

            // Validate the InvalidName error code.
            if (!ValidatorHelper.ValidateProperty("Name", businessRequest.Session, businessRequest.Session.Name))
            {
                ThrowNewSessionBusinessException(NewSessionBusinessException.ErrorCodes.InvalidName);
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
            await this.ValidateNewSession(databaseConnection, businessRequest, operationData);

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
