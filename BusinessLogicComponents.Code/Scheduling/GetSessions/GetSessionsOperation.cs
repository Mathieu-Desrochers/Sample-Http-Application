
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
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Throws a GetSessions business exception.
        /// </summary>
        private void ThrowGetSessionsBusinessException(GetSessionsBusinessException.ErrorCodes errorCode)
        {
            // Build the GetSessions business exception.
            GetSessionsBusinessException businessException = new GetSessionsBusinessException();
            businessException.ErrorMessage = String.Format("The SchedulingBusinessLogicComponent.GetSessions() business operation has thrown the {0} error code.", errorCode);
            businessException.ErrorCode = errorCode;

            // Throw the GetSessions business exception.
            throw businessException;
        }

        /// <summary>
        /// Validates the GetSessions business request.
        /// </summary>
        private Task ValidateGetSessions(IDatabaseConnection databaseConnection, GetSessionsBusinessRequest businessRequest, GetSessionsOperationData operationData)
        {
            return Task.FromResult<object>(null);
        }

        /// <summary>
        /// Executes the GetSessions business operation.
        /// </summary>
        public async virtual Task<GetSessionsBusinessResponse> GetSessions(IDatabaseConnection databaseConnection, GetSessionsBusinessRequest businessRequest)
        {
            // Initialize the operation data.
            GetSessionsOperationData operationData = new GetSessionsOperationData();

            // Validate the business request.
            await this.ValidateGetSessions(databaseConnection, businessRequest, operationData);

            // Read the Session data rows.
            operationData.SessionDataRows = await this.sessionDataAccessComponent.ReadAll(databaseConnection);

            // Build the business response.
            GetSessionsBusinessResponse businessResponse = new GetSessionsBusinessResponse();

            // Build the Session business response elements.
            List<GetSessionsBusinessResponse.SessionBusinessResponseElement> sessionBusinessResponseElements = new List<GetSessionsBusinessResponse.SessionBusinessResponseElement>();
            foreach (SessionDataRow sessionDataRow in operationData.SessionDataRows)
            {
                // Build the Session business response element.
                GetSessionsBusinessResponse.SessionBusinessResponseElement sessionBusinessResponseElement = new GetSessionsBusinessResponse.SessionBusinessResponseElement();
                sessionBusinessResponseElement.SessionID = sessionDataRow.SessionID;
                sessionBusinessResponseElement.SessionCode = sessionDataRow.SessionCode;
                sessionBusinessResponseElement.Name = sessionDataRow.Name;
                sessionBusinessResponseElement.StartDate = sessionDataRow.StartDate;
                sessionBusinessResponseElements.Add(sessionBusinessResponseElement);
            }

            // Set the Session business response elements.
            businessResponse.Sessions = sessionBusinessResponseElements.ToArray();

            // Return the business response.
            return businessResponse;
        }
    }
}
