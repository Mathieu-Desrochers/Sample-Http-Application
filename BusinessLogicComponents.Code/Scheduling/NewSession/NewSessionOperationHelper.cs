
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.Infrastructure.Code.DataAnnotations;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent
    {
        /// <summary>
        /// Validates the specified NewSession business request property.
        /// </summary>
        private void ValidateNewSessionRequestProperty(object businessRequestElement, string propertyName, object propertyValue, NewSessionBusinessException.ErrorCodes errorCode, List<NewSessionBusinessException.ErrorBusinessExceptionElement> errorBusinessExceptionElements)
        {
            // Check if the property is valid.
            if (ValidatorHelper.ValidateProperty(businessRequestElement, propertyName, propertyValue))
            {
                return;
            }

            // Build the Error business exception element.
            NewSessionBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new NewSessionBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.ErroneousValue = propertyValue;

            // Add the Error business exception element to the list.
            errorBusinessExceptionElements.Add(errorBusinessExceptionElement);
        }

        /// <summary>
        /// Builds a NewSession business exception.
        /// </summary>
        private NewSessionBusinessException BuildNewSessionBusinessException(NewSessionBusinessException.ErrorBusinessExceptionElement[] errorBusinessExceptionElements)
        {
            // Build the business exception.
            NewSessionBusinessException businessException = new NewSessionBusinessException();
            businessException.ErrorMessage = String.Format("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.");
            businessException.Errors = errorBusinessExceptionElements;

            // Return the business exception.
            return businessException;
        }

        /// <summary>
        /// Builds a NewSession business exception.
        /// </summary>
        private NewSessionBusinessException BuildNewSessionBusinessException(NewSessionBusinessException.ErrorCodes errorCode, object erroneousValue)
        {
            // Build an Error business exception element.
            NewSessionBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new NewSessionBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.ErroneousValue = erroneousValue;

            // Build the business exception.
            NewSessionBusinessException businessException = this.BuildNewSessionBusinessException(new NewSessionBusinessException.ErrorBusinessExceptionElement[] { errorBusinessExceptionElement });

            // Return the business exception.
            return businessException;
        }
    }
}
