
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
using SampleHttpApplication.ServiceFacadeComponents.Code;
using SampleHttpApplication.ServiceFacadeComponents.Code.Scheduling.Sessions;

namespace SampleHttpApplication.ServiceFacadeComponents.Tests.Scheduling.Sessions
{
    /// <summary>
    /// Represents the Sessions controller test harness.
    /// </summary>
    public class SessionsControllerTestHarness
    {
        /// <summary>
        /// The mocked database transaction.
        /// </summary>
        public readonly Mock<IDatabaseTransaction> MockedDatabaseTransaction;

        /// <summary>
        /// The mocked database connection.
        /// </summary>
        public readonly Mock<IDatabaseConnection> MockedDatabaseConnection;

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
        public SessionsControllerTestHarness(bool shouldOpenDatabaseConnection, bool shouldCommitDatabaseTransaction)
        {
            // Build the mocked database objects.
            this.MockedDatabaseTransaction = new Mock<IDatabaseTransaction>(MockBehavior.Strict);
            this.MockedDatabaseConnection = new Mock<IDatabaseConnection>(MockBehavior.Strict);
            this.MockedDatabaseConnectionProvider = new Mock<IDatabaseConnectionProvider>(MockBehavior.Strict);

            // Mock the database connection.
            if (shouldOpenDatabaseConnection)
            {
                // Verify the database connection is opened.
                this.MockedDatabaseConnectionProvider
                    .Setup(mock => mock.OpenDatabaseConnection())
                    .Returns(Task.FromResult<IDatabaseConnection>(this.MockedDatabaseConnection.Object))
                    .Verifiable();

                // Verify the database transaction begins.
                this.MockedDatabaseConnection
                    .Setup(mock => mock.BeginDatabaseTransaction())
                    .Returns(this.MockedDatabaseTransaction.Object)
                    .Verifiable();

                // Verify the database transaction is disposed.
                this.MockedDatabaseTransaction
                    .Setup(mock => mock.Dispose())
                    .Verifiable();

                // Verify the database connection is disposed.
                this.MockedDatabaseConnection
                    .Setup(mock => mock.Dispose())
                    .Verifiable();
            }

            // Mock the database transaction.
            if (shouldCommitDatabaseTransaction)
            {
                // Verify the database transaction is committed.
                this.MockedDatabaseTransaction
                    .Setup(mock => mock.Commit())
                    .Verifiable();
            }

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

            // Build the in memory HTTP server.
            HttpServer httpServer = new HttpServer(httpConfiguration);

            // Build the HTTP client.
            this.HttpClient = new HttpClient(httpServer);
        }

        /// <summary>
        /// Builds an HTTP request message.
        /// </summary>
        public HttpRequestMessage BuildHttpRequest(HttpMethod httpMethod, string url)
        {
            // Build an HTTP message with an empty content.
            HttpRequestMessage httpRequestMessage = this.BuildHttpRequest(httpMethod, url, "");
            return httpRequestMessage;
        }

        /// <summary>
        /// Builds an HTTP request message.
        /// </summary>
        public HttpRequestMessage BuildHttpRequest(HttpMethod httpMethod, string url, string content)
        {
            // Build the request uri.
            Uri requestUri = new Uri("http://sample.http.application/" + url);

            // Build the accept header.
            MediaTypeWithQualityHeaderValue acceptHeader = new MediaTypeWithQualityHeaderValue("application/json");

            // Build the content type header.
            MediaTypeWithQualityHeaderValue contentTypeHeader = new MediaTypeWithQualityHeaderValue("application/json");

            // Build the HTTP request message content.
            StringContent httpRequestMessageContent = new StringContent(content);
            httpRequestMessageContent.Headers.ContentType = contentTypeHeader;

            // Build the HTTP request message.
            HttpRequestMessage httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = httpMethod;
            httpRequestMessage.RequestUri = requestUri;
            httpRequestMessage.Headers.Accept.Add(acceptHeader);
            httpRequestMessage.Content = httpRequestMessageContent;

            // Return the HTTP request message.
            return httpRequestMessage;
        }

        /// <summary>
        /// Verifies the mocked components.
        /// </summary>
        public void VerifyMockedComponents()
        {
            // Verify the mocked database objects.
            this.MockedDatabaseTransaction.VerifyAll();
            this.MockedDatabaseConnection.VerifyAll();
            this.MockedDatabaseConnectionProvider.VerifyAll();

            // Verify the mocked Scheduling business logic component.
            this.MockedSchedulingBusinessLogicComponent.VerifyAll();
        }
    }
}
