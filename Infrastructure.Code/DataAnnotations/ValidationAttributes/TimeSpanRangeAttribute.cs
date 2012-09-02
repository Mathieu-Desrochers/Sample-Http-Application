
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.Infrastructure.Code.DataAnnotations.ValidationAttributes
{
    /// <summary>
    /// Represents the TimeSpanRange validation attribute.
    /// </summary>
    public class TimeSpanRangeAttribute : ValidationAttribute
    {
        /// <summary>
        /// The allowed TimeSpan range.
        /// </summary>
        private TimeSpan minimumTimeSpan;
        private TimeSpan maximumTimeSpan;

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public TimeSpanRangeAttribute(string minimum, string maximum)
        {
            this.minimumTimeSpan = TimeSpan.Parse(minimum);
            this.maximumTimeSpan = TimeSpan.Parse(maximum);
        }

        /// <summary>
        /// Determines whether the specified value of the object is valid.
        /// </summary>
        public override bool IsValid(object value)
        {
            // Make sure a value is specified.
            if (value == null)
            {
                return false;
            }

            // Check if the value is within the TimeSpan range.
            TimeSpan timeSpan = (TimeSpan)value;
            bool isMinimumValid = timeSpan >= this.minimumTimeSpan;
            bool isMaximumValid = timeSpan <= this.maximumTimeSpan;
            bool isValid = isMinimumValid && isMaximumValid;

            // Return whether the value is valid.
            return isValid;
        }
    }
}
