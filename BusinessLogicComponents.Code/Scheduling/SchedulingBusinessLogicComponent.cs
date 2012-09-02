
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.Infrastructure.Interface.UniqueToken;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component.
    /// </summary>
    public partial class SchedulingBusinessLogicComponent : ISchedulingBusinessLogicComponent
    {
        /// <summary>
        /// The data access components.
        /// </summary>
        private readonly ICourseScheduleDataAccessComponent courseScheduleDataAccessComponent;
        private readonly ISessionDataAccessComponent sessionDataAccessComponent;

        /// <summary>
        /// The unique token generator.
        /// </summary>
        private readonly IUniqueTokenGenerator uniqueTokenGenerator;

        /// <summary>
        /// Initialization constructor.
        /// </summary>
        public SchedulingBusinessLogicComponent(ICourseScheduleDataAccessComponent courseScheduleDataAccessComponent, ISessionDataAccessComponent sessionDataAccessComponent, IUniqueTokenGenerator uniqueTokenGenerator)
        {
            // Initialize the data access components.
            this.courseScheduleDataAccessComponent = courseScheduleDataAccessComponent;
            this.sessionDataAccessComponent = sessionDataAccessComponent;

            // Initialize the unique token generator.
            this.uniqueTokenGenerator = uniqueTokenGenerator;
        }
    }
}
