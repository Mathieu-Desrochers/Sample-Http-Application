
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceComponents.Interface
{
    /// <summary>
    /// Represents the BadFormat service exception.
    /// </summary>
    public class BadFormatServiceException : Exception
    {
        /// <summary>
        /// The error message.
        /// </summary>
        private string errorMessage;
        public string ErrorMessage
        {
            get { return this.errorMessage; }
            set { this.errorMessage = value; }
        }

        /// <summary>
        /// Gets the exception's message.
        /// </summary>
        public override string Message
        {
            get { return this.errorMessage; }
        }

        /// <summary>
        /// The bad formats.
        /// </summary>
        public BadFormatServiceExceptionElement[] BadFormats;
        public class BadFormatServiceExceptionElement
        {
            /// <summary>
            /// The badly formatted property.
            /// </summary>
            public string BadFormat;
        }
    }
}
