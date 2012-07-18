
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Dependencies;

using Moq;

namespace SampleHttpApplication.ServiceComponents.Tests
{
    /// <summary>
    /// Represents the mocked dependency resolver.
    /// </summary>
    public class MockedDependencyResolver : Mock<IDependencyResolver>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        public MockedDependencyResolver()
            : base(MockBehavior.Loose)
        {
            // Provide valid setups for these methods.
            this.Setup(mock => mock.BeginScope()).Returns(this.Object);
            this.Setup(mock => mock.GetServices(It.IsAny<Type>())).Returns(new object[0]);
        }
    }
}
