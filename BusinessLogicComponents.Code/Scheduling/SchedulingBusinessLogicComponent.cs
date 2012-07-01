
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
        /// Initialization constructor.
        /// </summary>
        public SchedulingBusinessLogicComponent(ISessionDataAccessComponent sessionDataAccessComponent)
        {
            // Initialize the data access components.
            this.sessionDataAccessComponent = sessionDataAccessComponent;
        }
    }
}
