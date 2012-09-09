
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions;
using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Validates the specified GetSessions business request property.
        /// </summary>
        private void ValidateGetSessionsRequestProperty(object businessRequestElement, string propertyName, object propertyValue, GetSessionsBusinessException.ErrorCodes errorCode, List<GetSessionsBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements)
        {
            // Check if the property is valid.
            if (ValidatorHelper.ValidateProperty(businessRequestElement, propertyName, propertyValue))
            {
                return;
            }

            // Build the Error business exception element.
            GetSessionsBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new GetSessionsBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.ErroneousValue = propertyValue;

            // Add the Error business exception element to the list.
            errorBusinessExceptionElements.Add(errorBusinessExceptionElement);
        }

        /// <summary>
        /// Builds a GetSessions business exception.
        /// </summary>
        private GetSessionsBusinessException BuildGetSessionsBusinessException(GetSessionsBusinessException.ErrorBusinessExceptionElement[] errorBusinessExceptionElements)
        {
            // Build the business exception.
            GetSessionsBusinessException businessException = new GetSessionsBusinessException();
            businessException.ErrorMessage = String.Format("SchedulingBusinessLogicComponent.GetSessions() has thrown a GetSessions business exception. See the Errors property for details.");
            businessException.Errors = errorBusinessExceptionElements;

            // Return the business exception.
            return businessException;
        }

        /// <summary>
        /// Builds a GetSessions business exception.
        /// </summary>
        private GetSessionsBusinessException BuildGetSessionsBusinessException(GetSessionsBusinessException.ErrorCodes errorCode, object erroneousValue)
        {
            // Build an Error business exception element.
            GetSessionsBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new GetSessionsBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.ErroneousValue = erroneousValue;

            // Build the business exception.
            GetSessionsBusinessException businessException = this.BuildGetSessionsBusinessException(new GetSessionsBusinessException.ErrorBusinessExceptionElement[] { errorBusinessExceptionElement });

            // Return the business exception.
            return businessException;
        }
    }
}
