
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Code.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface.Session;

namespace SampleHttpApplication.BusinessLogicComponents.Tests.Scheduling
{
    /// <summary>
    /// Represents a Scheduling business logic component test harness.
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
        public readonly Mock<ISessionDataAccessComponent> MockedSessionDataAccessComponent;

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
            this.MockedSessionDataAccessComponent = new Mock<ISessionDataAccessComponent>(MockBehavior.Strict);

            // Build the Scheduling business logic component as a partial mock.
            // This allows BusinessOperationA to be mocked while testing BusinessOperationB.
            this.MockedSchedulingBusinessLogicComponent = new Mock<SchedulingBusinessLogicComponent>(this.MockedSessionDataAccessComponent.Object);
            this.MockedSchedulingBusinessLogicComponent.CallBase = true;
        }

        /// <summary>
        /// Verifies the mocked components.
        /// </summary>
        public void VerifyMockedComponents()
        {
            // Verify the mocked data access components.
            this.MockedSessionDataAccessComponent.VerifyAll();

            // Verify the Scheduling business logic component.
            this.MockedSchedulingBusinessLogicComponent.VerifyAll();
        }
    }
}
