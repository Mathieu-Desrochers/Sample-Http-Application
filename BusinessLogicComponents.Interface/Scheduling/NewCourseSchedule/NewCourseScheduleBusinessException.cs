
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule
{
    /// <summary>
    /// Represents the NewCourseSchedule business exception.
    /// </summary>
    public class NewCourseScheduleBusinessException : BusinessException
    {
        /// <summary>
        /// The possible error codes.
        /// </summary>
        public enum ErrorCodes
        {
            InvalidSessionCode,
            InvalidTime
        }

        /// <summary>
        /// The errors.
        /// </summary>
        public ErrorBusinessExceptionElement[] Errors;
        public class ErrorBusinessExceptionElement
        {
            /// <summary>
            /// The error code.
            /// </summary>
            public ErrorCodes ErrorCode;

            /// <summary>
            /// The erroneous value.
            /// </summary>
            public object ErroneousValue;
        }
    }
}
