
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Code.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface.CourseGroup;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.Infrastructure.Interface.UniqueToken;

namespace SampleHttpApplication.BusinessLogicComponents.Tests.Scheduling
{
    /// <summary>
    /// Represents the Scheduling business logic component test harness.
    /// </summary>
    public class SchedulingBusinessLogicComponentTestHarness
    {
        /// <summary>
        /// The mocked database connection.
        /// </summary>
        public readonly MockedDatabaseConnection MockedDatabaseConnection;

        /// <summary>
        /// The mocked data access components.
        /// </summary>
        public readonly Mock<ICourseGroupDataAccessComponent> MockedCourseGroupDataAccessComponent;
        public readonly Mock<ICourseScheduleDataAccessComponent> MockedCourseScheduleDataAccessComponent;
        public readonly Mock<ISessionDataAccessComponent> MockedSessionDataAccessComponent;

        /// <summary>
        /// The mocked unique token generator.
        /// </summary>
        public readonly Mock<IUniqueTokenGenerator> MockedUniqueTokenGenerator;

        /// <summary>
        /// The Scheduling business logic component.
        /// </summary>
        public readonly Mock<SchedulingBusinessLogicComponent> MockedSchedulingBusinessLogicComponent;
        public SchedulingBusinessLogicComponent SchedulingBusinessLogicComponent
        {
            get { return this.MockedSchedulingBusinessLogicComponent.Object; }
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SchedulingBusinessLogicComponentTestHarness()
        {
            // Build the mocked database connection.
            this.MockedDatabaseConnection = new MockedDatabaseConnection();

            // Build the mocked data access components.
            this.MockedCourseGroupDataAccessComponent = new Mock<ICourseGroupDataAccessComponent>(MockBehavior.Strict);
            this.MockedCourseScheduleDataAccessComponent = new Mock<ICourseScheduleDataAccessComponent>(MockBehavior.Strict);
            this.MockedSessionDataAccessComponent = new Mock<ISessionDataAccessComponent>(MockBehavior.Strict);

            // Build the mocked unique token generator.
            this.MockedUniqueTokenGenerator = new Mock<IUniqueTokenGenerator>(MockBehavior.Strict);

            // Build the Scheduling business logic component as a partial mock.
            // This allows business operation A to be mocked while testing business operation B.
            this.MockedSchedulingBusinessLogicComponent = new Mock<SchedulingBusinessLogicComponent>(this.MockedCourseGroupDataAccessComponent.Object, this.MockedCourseScheduleDataAccessComponent.Object, this.MockedSessionDataAccessComponent.Object, this.MockedUniqueTokenGenerator.Object);
            this.MockedSchedulingBusinessLogicComponent.CallBase = true;
        }

        /// <summary>
        /// Verifies the mocked components.
        /// </summary>
        public void VerifyMockedComponents()
        {
            // Verify the mocked data access components.
            this.MockedCourseGroupDataAccessComponent.VerifyAll();
            this.MockedCourseScheduleDataAccessComponent.VerifyAll();
            this.MockedSessionDataAccessComponent.VerifyAll();

            // Verify the mocked unique token generator.
            this.MockedUniqueTokenGenerator.VerifyAll();

            // Verify the Scheduling business logic component.
            this.MockedSchedulingBusinessLogicComponent.VerifyAll();
        }
    }
}
