
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.Infrastructure.Code.DataAnnotations
{
    /// <summary>
    /// Helper class to invoke the Validator object.
    /// </summary>
    public static class ValidatorHelper
    {
        /// <summary>
        /// Returns whether the specified property is valid.
        /// </summary>
        public static bool ValidateProperty(object instance, string propertyName, object propertyValue)
        {
            // Build a validation context for the specified property.
            ValidationContext validationContext = new ValidationContext(instance, null, null);
            validationContext.MemberName = propertyName;

            // Validate the specified property value.
            List<ValidationResult> validationResults = new List<ValidationResult>();
            bool isPropertyValid = Validator.TryValidateProperty(propertyValue, validationContext, validationResults);

            // Return whether the property is valid.
            return isPropertyValid;
        }
    }
}
