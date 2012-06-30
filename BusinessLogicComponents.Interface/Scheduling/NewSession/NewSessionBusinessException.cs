
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling
{
    /// <summary>
    /// Represents a NewSession business exception.
    /// </summary>
    public class NewSessionBusinessException : BusinessException
    {
        /// <summary>
        /// The possible error codes.
        /// </summary>
        public ErrorCodes ErrorCode;
        public enum ErrorCodes
        {
            InvalidSessionCode,
            DuplicateSessionCode,
            InvalidName
        }
    }
}
