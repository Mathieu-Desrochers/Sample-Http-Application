
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceComponents.Interface
{
    /// <summary>
    /// Represents the service exception base.
    /// </summary>
    public class ServiceExceptionBase : Exception
    {
        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public ServiceExceptionBase(string message, Exception innerException) :
            base(message, innerException)
        {
        }

        /// <summary>
        /// The service exception details.
        /// </summary>
        public object Details { get; set; }
    }
}
