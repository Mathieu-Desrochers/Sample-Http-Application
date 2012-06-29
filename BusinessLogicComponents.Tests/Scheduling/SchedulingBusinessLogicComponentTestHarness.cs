
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
        /// The mocked business logic components.
        /// </summary>

        /// <summary>
        /// The Scheduling business logic component.
        /// </summary>
        public readonly SchedulingBusinessLogicComponent SchedulingBusinessLogicComponent;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SchedulingBusinessLogicComponentTestHarness()
        {
            // Build the mocked database connection.
            this.MockedDatabaseConnection = new MockedDatabaseConnection();

            // Build the mocked data access components.
            this.MockedSessionDataAccessComponent = new Mock<ISessionDataAccessComponent>(MockBehavior.Strict);

            // Build the mocked business logic components.

            // Build the Scheduling business logic component.
            this.SchedulingBusinessLogicComponent = new SchedulingBusinessLogicComponent(this.MockedSessionDataAccessComponent.Object);
        }

        /// <summary>
        /// Verifies the mocked components.
        /// </summary>
        public void VerifyMockedComponents()
        {
            // Verify the mocked data access components.
            this.MockedSessionDataAccessComponent.VerifyAll();

            // Verify the mocked business logic components.

        }
    }
}
