
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.ServiceFacadeComponents.Interface;
using SampleHttpApplication.ServiceFacadeComponents.Interface.Scheduling.CourseSchedules;

namespace SampleHttpApplication.ServiceFacadeComponents.Code.Scheduling.CourseSchedules
{
    /// <summary>
    /// Represents the CourseSchedules controller.
    /// </summary>
    public partial class CourseSchedulesController
    {
        /// <summary>
        /// Invokes the NewCourseSchedule business operation.
        /// </summary>
        private async Task<NewCourseScheduleBusinessResponse> InvokeNewCourseSchedule(IDatabaseConnection databaseConnection, CourseScheduleResource resource)
        {
            try
            {
                // Build the NewCourseSchedule business request.
                NewCourseScheduleBusinessRequest newCourseScheduleBusinessRequest = new NewCourseScheduleBusinessRequest();

                // Build the Session business request element.
                if (resource.Session != null)
                {
                    NewCourseScheduleBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewCourseScheduleBusinessRequest.SessionBusinessRequestElement();
                    sessionBusinessRequestElement.SessionCode = resource.Session.SessionCode;
                    newCourseScheduleBusinessRequest.Session = sessionBusinessRequestElement;
                }

                // Build the CourseSchedule business request element.
                if (resource.CourseSchedule != null)
                {
                    NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement();
                    courseScheduleBusinessRequestElement.DayOfWeek = resource.CourseSchedule.DayOfWeek;
                    courseScheduleBusinessRequestElement.Time = resource.CourseSchedule.Time;
                    newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;
                }

                // Invoke the NewCourseSchedule business operation.
                NewCourseScheduleBusinessResponse newCourseScheduleBusinessResponse = await this.schedulingBusinessLogicComponent.NewCourseSchedule(databaseConnection, newCourseScheduleBusinessRequest);

                // The business operation succeeded.
                return newCourseScheduleBusinessResponse;
            }
            catch (NewCourseScheduleBusinessException newCourseScheduleBusinessException)
            {
                // Wrap the NewCourseSchedule business exception into a service exception.
                ServiceException serviceException = ServiceExceptionBuilder.BuildServiceException(
                    "SchedulingServiceComponent.CourseSchedulesController.Post()",
                    newCourseScheduleBusinessException,
                    newCourseScheduleBusinessException.Errors.Select(error => error.ErrorCode.ToString()).ToArray(),
                    newCourseScheduleBusinessException.Errors.Select(error => error.ErroneousValue).ToArray());
                
                // Throw the service exception.
                throw serviceException;
            }
        }

        /// <summary>
        /// Executes the HTTP POST method.
        /// </summary>
        public async Task<HttpResponseMessage> Post(CourseScheduleResource resource)
        {
            // Make sure the resource is valid.
            if (!this.ModelState.IsValid)
            {
                HttpResponseMessage httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
                HttpResponseException httpResponseException = new HttpResponseException(httpResponseMessage);
                throw httpResponseException; 
            }

            // Open a database connection and begin a database transaction.
            using (IDatabaseConnection databaseConnection = await this.databaseConnectionProvider.OpenDatabaseConnection())
            using (IDatabaseTransaction databaseTransaction = databaseConnection.BeginDatabaseTransaction())
            {
                // Invoke the NewCourseSchedule business operation.
                NewCourseScheduleBusinessResponse newCourseScheduleBusinessResponse = await this.InvokeNewCourseSchedule(databaseConnection, resource);

                // Update the resource.
                resource.CourseSchedule.CourseScheduleCode = newCourseScheduleBusinessResponse.CourseSchedule.CourseScheduleCode;

                // Commit the database transaction.
                databaseTransaction.Commit();

                // Build the HTTP response message.
                HttpResponseMessage httpResponseMessage = this.Request.CreateResponse(HttpStatusCode.Created, resource);
                return httpResponseMessage;
            }
        }
    }
}
