
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.Infrastructure.Code.DataAnnotations.ValidationAttributes
{
    /// <summary>
    /// Represents the RequiredArrayItems validation attribute.
    /// </summary>
    public class RequiredArrayItemsAttribute : ValidationAttribute
    {
        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        public override bool IsValid(object value)
        {
            // A null array contains no null items.
            // It is therefore valid.
            if (value == null)
            {
                return true;
            }

            // Make sure an array is being validated.
            Array array = value as Array;
            if (array == null)
            {
                return false;
            }

            // Make sure the array contains no null items.
            bool isValid = array.Cast<object>().All(arrayItem => arrayItem != null);
            return isValid;
        }
    }
}
