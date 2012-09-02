
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;
using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Validates the NewCourseSchedule business request.
        /// </summary>
        private void ValidateNewCourseScheduleRequest(NewCourseScheduleBusinessRequest businessRequest)
        {
            // Build the list of errors.
            List<NewCourseScheduleBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements = new List<NewCourseScheduleBusinessException.ErrorBusinessExceptionElement>();

            // Validate the CourseSchedule business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = businessRequest.CourseSchedule;
            this.ValidateNewCourseScheduleRequestProperty(courseScheduleBusinessRequestElement, "Time", courseScheduleBusinessRequestElement.Time, NewCourseScheduleBusinessException.ErrorCodes.InvalidTime, errorBusinessExceptionElements);

            // Check if any errors were added to the list.
            if (errorBusinessExceptionElements.Any())
            {
                // Throw a NewCourseSchedule business exception.
                NewCourseScheduleBusinessException businessException = this.BuildNewCourseScheduleBusinessException(errorBusinessExceptionElements.ToArray());
                throw businessException;
            }
        }

        /// <summary>
        /// Validates the NewCourseSchedule business operation.
        /// </summary>
        private async Task ValidateNewCourseScheduleOperation(IDatabaseConnection databaseConnection, NewCourseScheduleBusinessRequest businessRequest, NewCourseScheduleOperationData operationData)
        {
            // Validate the InvalidSessionCode error code.
            operationData.SessionDataRow = await this.sessionDataAccessComponent.ReadBySessionCode(databaseConnection, businessRequest.Session.SessionCode);
            if (operationData.SessionDataRow == null)
            {
                // Throw a NewCourseSchedule business exception.
                NewCourseScheduleBusinessException businessException = this.BuildNewCourseScheduleBusinessException(NewCourseScheduleBusinessException.ErrorCodes.InvalidSessionCode, businessRequest.Session.SessionCode);
                throw businessException;
            }
        }

        /// <summary>
        /// Executes the NewCourseSchedule business operation.
        /// </summary>
        public async virtual Task<NewCourseScheduleBusinessResponse> NewCourseSchedule(IDatabaseConnection databaseConnection, NewCourseScheduleBusinessRequest businessRequest)
        {
            // Initialize the operation data.
            NewCourseScheduleOperationData operationData = new NewCourseScheduleOperationData();

            // Validate the business request.
            this.ValidateNewCourseScheduleRequest(businessRequest);

            // Validate the business operation.
            await this.ValidateNewCourseScheduleOperation(databaseConnection, businessRequest, operationData);

            // Generate a unique CourseSchedule code.
            string courseScheduleCode = this.uniqueTokenGenerator.GenerateUniqueToken();

            // Create the CourseSchedule data row.
            operationData.CourseScheduleDataRow = new CourseScheduleDataRow();
            operationData.CourseScheduleDataRow.CourseScheduleCode = courseScheduleCode;
            operationData.CourseScheduleDataRow.SessionID = operationData.SessionDataRow.SessionID;
            operationData.CourseScheduleDataRow.DayOfWeek = (int)businessRequest.CourseSchedule.DayOfWeek;
            operationData.CourseScheduleDataRow.Time = businessRequest.CourseSchedule.Time;
            await this.courseScheduleDataAccessComponent.Create(databaseConnection, operationData.CourseScheduleDataRow);

            // Build the business response.
            NewCourseScheduleBusinessResponse businessResponse = new NewCourseScheduleBusinessResponse();

            // Build the CourseSchedule business response element.
            NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement courseScheduleBusinessResponseElement = new NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement();
            courseScheduleBusinessResponseElement.CourseScheduleCode = operationData.CourseScheduleDataRow.CourseScheduleCode;
            businessResponse.CourseSchedule = courseScheduleBusinessResponseElement;

            // Return the business response.
            return businessResponse;
        }
    }
}
