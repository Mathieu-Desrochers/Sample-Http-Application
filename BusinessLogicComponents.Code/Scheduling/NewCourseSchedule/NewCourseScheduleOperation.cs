
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.CourseGroup;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;
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
        /// Validates the NewCourseSchedule business request.
        /// </summary>
        private void ValidateNewCourseScheduleRequest(NewCourseScheduleBusinessRequest businessRequest)
        {
            // Build the list of errors.
            List<NewCourseScheduleBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements = new List<NewCourseScheduleBusinessException.ErrorBusinessExceptionElement>();

            // Validate the Session business request element.
            this.ValidateNewCourseScheduleRequestProperty(businessRequest, "Session", businessRequest.Session, NewCourseScheduleBusinessException.ErrorCodes.InvalidSession, errorBusinessExceptionElements);
            if (businessRequest.Session != null)
            {
                // Validate the Session business request element properties.
                NewCourseScheduleBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = businessRequest.Session;
                this.ValidateNewCourseScheduleRequestProperty(sessionBusinessRequestElement, "SessionCode", sessionBusinessRequestElement.SessionCode, NewCourseScheduleBusinessException.ErrorCodes.InvalidSessionCode, errorBusinessExceptionElements);
            }

            // Validate the CourseSchedule business request element.
            this.ValidateNewCourseScheduleRequestProperty(businessRequest, "CourseSchedule", businessRequest.CourseSchedule, NewCourseScheduleBusinessException.ErrorCodes.InvalidCourseSchedule, errorBusinessExceptionElements);
            if (businessRequest.CourseSchedule != null)
            {
                // Validate the CourseSchedule business request element properties.
                NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = businessRequest.CourseSchedule;
                this.ValidateNewCourseScheduleRequestProperty(courseScheduleBusinessRequestElement, "Time", courseScheduleBusinessRequestElement.Time, NewCourseScheduleBusinessException.ErrorCodes.InvalidTime, errorBusinessExceptionElements);

                // Validate the CourseGroup business request elements.
                this.ValidateNewCourseScheduleRequestProperty(courseScheduleBusinessRequestElement, "CourseGroups", courseScheduleBusinessRequestElement.CourseGroups, NewCourseScheduleBusinessException.ErrorCodes.InvalidCourseGroup, errorBusinessExceptionElements);
                if (courseScheduleBusinessRequestElement.CourseGroups != null)
                {
                    // Skip the null business request elements.
                    NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement[] nonNullCourseGroupBusinessRequestElements = courseScheduleBusinessRequestElement.CourseGroups.Where(courseGroupBusinessRequestElement => courseGroupBusinessRequestElement != null).ToArray();
                    foreach (NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement courseGroupBusinessRequestElement in nonNullCourseGroupBusinessRequestElements)
                    {
                        // Validate the CourseGroup business request element properties.
                        this.ValidateNewCourseScheduleRequestProperty(courseGroupBusinessRequestElement, "PlacesCount", courseGroupBusinessRequestElement.PlacesCount, NewCourseScheduleBusinessException.ErrorCodes.InvalidPlacesCount, errorBusinessExceptionElements);
                    }
                }
            }

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
            // Validate the business request.
            this.ValidateNewCourseScheduleRequest(businessRequest);

            // Initialize the operation data.
            NewCourseScheduleOperationData operationData = new NewCourseScheduleOperationData();

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

            // Create the CourseGroup data rows.
            foreach (NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement courseGroupBusinessRequestElement in businessRequest.CourseSchedule.CourseGroups)
            {
                // Generate a unique CourseGroup code.
                string courseGroupCode = this.uniqueTokenGenerator.GenerateUniqueToken();

                // Build the CourseGroup data row.
                CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
                operationData.CourseGroupDataRows.Add(courseGroupDataRow);

                // Create the CourseGroup data row.
                courseGroupDataRow = new CourseGroupDataRow();
                courseGroupDataRow.CourseGroupCode = courseGroupCode;
                courseGroupDataRow.CourseScheduleID = operationData.CourseScheduleDataRow.CourseScheduleID;
                courseGroupDataRow.PlacesCount = courseGroupBusinessRequestElement.PlacesCount;
                await this.courseGroupDataAccessComponent.Create(databaseConnection, courseGroupDataRow);
            }

            // Build the business response.
            NewCourseScheduleBusinessResponse businessResponse = new NewCourseScheduleBusinessResponse();

            // Build the CourseSchedule business response element.
            NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement courseScheduleBusinessResponseElement = new NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement();
            courseScheduleBusinessResponseElement.CourseScheduleCode = operationData.CourseScheduleDataRow.CourseScheduleCode;
            businessResponse.CourseSchedule = courseScheduleBusinessResponseElement;

            // Build the CourseGroup business response elements.
            List<NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement.CourseGroupBusinessResponseElement> courseGroupBusinessResponseElements = new List<NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement.CourseGroupBusinessResponseElement>();
            for (int courseGroupIndex = 0; courseGroupIndex < businessRequest.CourseSchedule.CourseGroups.Length; courseGroupIndex++)
            {
                // Get the CourseGroup code.
                string courseGroupCode = operationData.CourseGroupDataRows[courseGroupIndex].CourseGroupCode;

                // Build the CourseGroup business response element.
                NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement.CourseGroupBusinessResponseElement courseGroupBusinessResponseElement = new NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement.CourseGroupBusinessResponseElement();
                courseGroupBusinessResponseElement.CourseGroupCode = courseGroupCode;
                courseGroupBusinessResponseElements.Add(courseGroupBusinessResponseElement);
            }

            // Return the business response.
            return businessResponse;
        }
    }
}
