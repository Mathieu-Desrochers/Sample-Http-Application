
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface.Session;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents a Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent : ISchedulingBusinessLogicComponent
    {
        /// <summary>
        /// The data access components.
        /// </summary>
        private readonly ISessionDataAccessComponent sessionDataAccessComponent;

        /// <summary>
        /// The business logic components.
        /// </summary>

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public SchedulingBusinessLogicComponent(ISessionDataAccessComponent sessionDataAccessComponent)
        {
            // Initialize the data access components.
            this.sessionDataAccessComponent = sessionDataAccessComponent;

            // Initialize the business logic components.
        }

        /// <summary>
        /// Formats an error message for the specified error code.
        /// </summary>
        private string FormatErrorMessage(string operationName, string errorCode)
        {
            string errorMessage = String.Format("The SchedulingBusinessLogicComponent.{0} operation returned the {1} error code.", operationName, errorCode);
            return errorMessage;
        }
    }
}
