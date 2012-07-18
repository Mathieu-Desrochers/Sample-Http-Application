
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceComponents.Interface
{
    /// <summary>
    /// Represents the InvalidFields service exception.
    /// </summary>
    public class InvalidFieldsServiceException : ServiceException
    {
        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public InvalidFieldsServiceException()
        {
            this.Details = new InvalidValuesServiceExceptionDetails();
        }

        /// <summary>
        /// The exception details.
        /// </summary>
        public InvalidValuesServiceExceptionDetails Details;
        public class InvalidValuesServiceExceptionDetails
        {
            /// <summary>
            /// Gets or sets the invalid fields.
            /// </summary>
            public string[] InvalidFields { get; set; }
        }
    }
}
