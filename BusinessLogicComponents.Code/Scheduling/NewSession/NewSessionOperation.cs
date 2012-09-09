
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
        /// Validates the NewSession business request.
        /// </summary>
        private void ValidateNewSessionRequest(NewSessionBusinessRequest businessRequest)
        {
            // Build the list of errors.
            List<NewSessionBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements = new List<NewSessionBusinessException.ErrorBusinessExceptionElement>();

            // Validate the Session business request element.
            this.ValidateNewSessionRequestProperty(businessRequest, "Session", businessRequest.Session, NewSessionBusinessException.ErrorCodes.InvalidSession, errorBusinessExceptionElements);
            if (businessRequest.Session != null)
            {
                // Validate the Session business request element properties.
                NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = businessRequest.Session;
                this.ValidateNewSessionRequestProperty(sessionBusinessRequestElement, "Name", sessionBusinessRequestElement.Name, NewSessionBusinessException.ErrorCodes.InvalidName, errorBusinessExceptionElements);
            }

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
        private Task ValidateNewSessionOperation(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest, NewSessionOperationData operationData)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the NewSession business operation.
        /// </summary>
        public async virtual Task<NewSessionBusinessResponse> NewSession(IDatabaseConnection databaseConnection, NewSessionBusinessRequest businessRequest)
        {
            // Validate the business request.
            this.ValidateNewSessionRequest(businessRequest);

            // Initialize the operation data.
            NewSessionOperationData operationData = new NewSessionOperationData();

            // Validate the business operation.
            await this.ValidateNewSessionOperation(databaseConnection, businessRequest, operationData);

            // Generate a unique Session code.
            string sessionCode = this.uniqueTokenGenerator.GenerateUniqueToken();

            // Create the Session data row.
            operationData.SessionDataRow = new SessionDataRow();
            operationData.SessionDataRow.SessionCode = sessionCode;
            operationData.SessionDataRow.Name = businessRequest.Session.Name;
            operationData.SessionDataRow.StartDate = businessRequest.Session.StartDate;
            await this.sessionDataAccessComponent.Create(databaseConnection, operationData.SessionDataRow);

            // Build the business response.
            NewSessionBusinessResponse businessResponse = new NewSessionBusinessResponse();

            // Build the Session business response element.
            NewSessionBusinessResponse.SessionBusinessResponseElement sessionBusinessResponseElement = new NewSessionBusinessResponse.SessionBusinessResponseElement();
            sessionBusinessResponseElement.SessionCode = operationData.SessionDataRow.SessionCode;
            businessResponse.Session = sessionBusinessResponseElement;

            // Return the business response.
            return businessResponse;
        }
    }
}
