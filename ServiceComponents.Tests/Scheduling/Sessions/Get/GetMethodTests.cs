
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.ServiceComponents.Tests.Scheduling.Sessions.Get
{
    /// <summary>
    /// Represents the HTTP GET method tests.
    /// </summary>
    [TestClass]
    public class GetMethodTests
    {
        /// <summary>
        /// Should return zero Session resources.
        /// </summary>
        [TestMethod]
        public void ShouldReturnZeroSessionResources()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness();

            // Mock the invocation of the GetSessions business operation.
            testHarness.MockedSchedulingBusinessLogicComponent
                .Setup(mock => mock.GetSessions(It.IsAny<IDatabaseConnection>(), It.IsAny<GetSessionsBusinessRequest>()))
                .Returns(Task.FromResult(new GetSessionsBusinessResponse()
                {
                    // Mock the Session business response elements.
                    Sessions = new GetSessionsBusinessResponse.SessionBusinessResponseElement[0]
                }))
                .Verifiable();

            // Invoke the HTTP GET method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Get, "api/scheduling/sessions", "application/json");
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual("[]", httpResponseMessageContent);
        }

        /// <summary>
        /// Should return one Session resource.
        /// </summary>
        [TestMethod]
        public void ShouldReturnOneSessionResource()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness();

            // Mock the invocation of the GetSessions business operation.
            testHarness.MockedSchedulingBusinessLogicComponent
                .Setup(mock => mock.GetSessions(It.IsAny<IDatabaseConnection>(), It.IsAny<GetSessionsBusinessRequest>()))
                .Returns(Task.FromResult(new GetSessionsBusinessResponse()
                {
                    // Mock the Session business response elements.
                    Sessions = new GetSessionsBusinessResponse.SessionBusinessResponseElement[]
                    {
                        // Mock the Session business response element.
                        new GetSessionsBusinessResponse.SessionBusinessResponseElement()
                        {
                            SessionID = 10001,
                            SessionCode = "Session-A",
                            Name = "Session Alpha",
                            StartDate = new DateTime(2001, 1, 1)
                        }
                    }
                }))
                .Verifiable();

            // Invoke the HTTP GET method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Get, "api/scheduling/sessions", "application/json");
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual("[{\"SessionID\":10001,\"SessionCode\":\"Session-A\",\"Name\":\"Session Alpha\",\"StartDate\":\"2001-01-01T00:00:00\"}]", httpResponseMessageContent);
        }
    }
}
