﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface
{
    /// <summary>
    /// Represents the business exception.
    /// </summary>
    public class BusinessException : Exception
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
    }
}
