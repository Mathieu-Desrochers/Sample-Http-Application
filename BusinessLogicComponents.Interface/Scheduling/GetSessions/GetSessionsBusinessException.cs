
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling
{
    /// <summary>
    /// Represents the GetSessions business exception.
    /// </summary>
    public class GetSessionsBusinessException : BusinessException
    {
        /// <summary>
        /// The possible error codes.
        /// </summary>
        public ErrorCodes ErrorCode;
        public enum ErrorCodes
        {
        }
    }
}
