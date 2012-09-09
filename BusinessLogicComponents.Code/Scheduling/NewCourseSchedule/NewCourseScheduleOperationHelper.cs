
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Validates the specified NewCourseSchedule business request property.
        /// </summary>
        private void ValidateNewCourseScheduleRequestProperty(object businessRequestElement, string propertyName, object propertyValue, NewCourseScheduleBusinessException.ErrorCodes errorCode, List<NewCourseScheduleBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements)
        {
            // Check if the property is valid.
            if (ValidatorHelper.ValidateProperty(businessRequestElement, propertyName, propertyValue))
            {
                return;
            }

            // Build the Error business exception element.
            NewCourseScheduleBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new NewCourseScheduleBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.ErroneousValue = propertyValue;

            // Add the Error business exception element to the list.
            errorBusinessExceptionElements.Add(errorBusinessExceptionElement);
        }

        /// <summary>
        /// Builds a NewCourseSchedule business exception.
        /// </summary>
        private NewCourseScheduleBusinessException BuildNewCourseScheduleBusinessException(NewCourseScheduleBusinessException.ErrorBusinessExceptionElement[] errorBusinessExceptionElements)
        {
            // Build the business exception.
            NewCourseScheduleBusinessException businessException = new NewCourseScheduleBusinessException();
            businessException.ErrorMessage = String.Format("SchedulingBusinessLogicComponent.NewCourseSchedule() has thrown a NewCourseSchedule business exception. See the Errors property for details.");
            businessException.Errors = errorBusinessExceptionElements;

            // Return the business exception.
            return businessException;
        }

        /// <summary>
        /// Builds a NewCourseSchedule business exception.
        /// </summary>
        private NewCourseScheduleBusinessException BuildNewCourseScheduleBusinessException(NewCourseScheduleBusinessException.ErrorCodes errorCode, object erroneousValue)
        {
            // Build an Error business exception element.
            NewCourseScheduleBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new NewCourseScheduleBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.ErroneousValue = erroneousValue;

            // Build the business exception.
            NewCourseScheduleBusinessException businessException = this.BuildNewCourseScheduleBusinessException(new NewCourseScheduleBusinessException.ErrorBusinessExceptionElement[] { errorBusinessExceptionElement });

            // Return the business exception.
            return businessException;
        }
    }
}
