
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.Infrastructure.Code.DataAnnotations
{
    /// <summary>
    /// Represents the validation error.
    /// </summary>
    public class ValidationError<T>
    {
        /// <summary>
        /// The error code.
        /// </summary>
        public T ErrorCode;
        
        /// <summary>
        /// The erroneous value.
        /// </summary>
        public object ErroneousValue;
    }
}
