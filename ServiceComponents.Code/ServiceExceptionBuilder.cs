
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using SampleHttpApplication.BusinessLogicComponents.Interface;
using SampleHttpApplication.ServiceComponents.Interface;

namespace SampleHttpApplication.ServiceComponents.Code
{
    /// <summary>
    /// Represents the service exception builder.
    /// </summary>
    public static class ServiceExceptionBuilder
    {
        /// <summary>
        /// Builds a service exception.
        /// </summary>
        public static ServiceException BuildServiceException(string action, BusinessException businessException, string[] errorCodes, object[] erroneousValues)
        {
            // Format the exception message.
            string exceptionMessage = String.Format("{0} has thrown a service exception. See the ErrorCodes property for details.", action);

            // Build the service exception.
            ServiceException serviceException = new ServiceException(exceptionMessage, businessException);

            // Build the Error service exception elements.
            List<ServiceException.ErrorServiceExceptionElement> errorServiceExceptionElements = new List<ServiceException.ErrorServiceExceptionElement>();
            for (int i = 0; i < errorCodes.Length; i++)
            {
                // Build the Error service exception element.
                ServiceException.ErrorServiceExceptionElement errorServiceExceptionElement = new ServiceException.ErrorServiceExceptionElement();
                errorServiceExceptionElement.ErrorCode = errorCodes[i].ToString();
                errorServiceExceptionElement.ErroneousValue = erroneousValues[i];
                errorServiceExceptionElements.Add(errorServiceExceptionElement);
            }

            // Set the Error service exception elements.
            serviceException.Errors = errorServiceExceptionElements.ToArray();

            // Return the service exception.
            return serviceException;
        }
    }
}
