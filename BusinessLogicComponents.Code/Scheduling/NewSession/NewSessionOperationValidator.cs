
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
    /// Represents the NewSession operation validator.
    /// </summary>
    public static class NewSessionOperationValidator
    {
        /// <summary>
        /// Adds the specified error code to the list when
        /// the validation of a business request element fails.
        /// </summary>
        public static void AddIfInvalid(this List<NewSessionBusinessException.ErrorBusinessExceptionElement> instance, object businessRequestElement, string propertyName, object propertyValue, NewSessionBusinessException.ErrorCodes errorCode)
        {
            // Make sure the business request element is invalid.
            if (ValidatorHelper.ValidateProperty(businessRequestElement, propertyName, propertyValue))
            {
                return;
            }

            // Add the specified error code to the list.
            NewSessionBusinessException.ErrorBusinessExceptionElement errorBusinessExceptionElement = new NewSessionBusinessException.ErrorBusinessExceptionElement();
            errorBusinessExceptionElement.ErrorCode = errorCode;
            errorBusinessExceptionElement.Value = propertyValue;
            instance.Add(errorBusinessExceptionElement);
        }
    }
}
