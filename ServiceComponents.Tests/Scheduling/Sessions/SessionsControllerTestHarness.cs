
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dependencies;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.ServiceComponents.Code;
using SampleHttpApplication.ServiceComponents.Code.Scheduling.Sessions;

namespace SampleHttpApplication.ServiceComponents.Tests.Scheduling.Sessions
{
    /// <summary>
    /// Represents the Sessions controller test harness.
    /// </summary>
    public class SessionsControllerTestHarness
    {
        /// <summary>
        /// The mocked database connection provider.
        /// </summary>
        public readonly Mock<IDatabaseConnectionProvider> MockedDatabaseConnectionProvider;

        /// <summary>
        /// The mocked Scheduling business logic component.
        /// </summary>
        public readonly Mock<ISchedulingBusinessLogicComponent> MockedSchedulingBusinessLogicComponent;

        /// <summary>
        /// The Sessions controller.
        /// </summary>
        public readonly SessionsController SessionsController;

        /// <summary>
        /// The HTTP client.
        /// </summary>
        public readonly HttpClient HttpClient;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SessionsControllerTestHarness()
        {
            // Build the mocked database connection provider.
            this.MockedDatabaseConnectionProvider = new Mock<IDatabaseConnectionProvider>(MockBehavior.Strict);

            // Mock the opening of the database connection.
            this.MockedDatabaseConnectionProvider
                .Setup(mock => mock.OpenDatabaseConnection())
                .Returns(Task.FromResult<IDatabaseConnection>(new MockedDatabaseConnection()))
                .Verifiable();

            // Build the mocked Scheduling business logic component.
            this.MockedSchedulingBusinessLogicComponent = new Mock<ISchedulingBusinessLogicComponent>(MockBehavior.Strict);

            // Build the Sessions controller.
            this.SessionsController = new SessionsController(this.MockedDatabaseConnectionProvider.Object, this.MockedSchedulingBusinessLogicComponent.Object);

            // Build the HTTP configuration.
            HttpConfiguration httpConfiguration = new HttpConfiguration();
            GlobalHttpApplication.ConfigureApplication(httpConfiguration);

            // Build the mocked dependency resolver.
            MockedDependencyResolver mockedDependencyResolver = new MockedDependencyResolver();
            mockedDependencyResolver.Setup(mock => mock.GetService(typeof(SessionsController)))
                .Returns(this.SessionsController);

            // Set the mocked dependency resolver.
            httpConfiguration.DependencyResolver = mockedDependencyResolver.Object;

            // Build the HTTP client.
            HttpServer httpServer = new HttpServer(httpConfiguration);
            this.HttpClient = new HttpClient(httpServer);
        }

        /// <summary>
        /// Builds an HTTP request message.
        /// </summary>
        public HttpRequestMessage BuildHttpRequest(HttpMethod httpMethod, string url, string accept)
        {
            // Build the request uri.
            Uri requestUri = new Uri("http://sample.http.application/" + url);

            // Build the accept header.
            MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue(accept);

            // Build the HTTP request message.
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = requestUri;
            httpRequestMessage.Headers.Accept.Add(acceptHeader);

            // Return the HTTP request message.
            return httpRequestMessage;
        }

        /// <summary>
        /// Verifies the mocked components.
        /// </summary>
        public void VerifyMockedComponents()
        {
            // Verify the mocked database connection provider.
            this.MockedDatabaseConnectionProvider.VerifyAll();

            // Verify the mocked Scheduling business logic component.
            this.MockedSchedulingBusinessLogicComponent.VerifyAll();
        }
    }
}
