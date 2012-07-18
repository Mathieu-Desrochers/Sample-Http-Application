
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

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("[");
            expectedJsonContent.Append("]");

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
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

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("[");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"SessionID\":10001,");
            expectedJsonContent.Append("\"SessionCode\":\"Session-A\",");
            expectedJsonContent.Append("\"Name\":\"Session Alpha\",");
            expectedJsonContent.Append("\"StartDate\":\"2001-01-01T00:00:00\"");
            expectedJsonContent.Append("}");
            expectedJsonContent.Append("]");

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }

        /// <summary>
        /// Should return multiple Session resources.
        /// </summary>
        [TestMethod]
        public void ShouldReturnMultipleSessionResources()
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
                        },

                        // Mock the Session business response element.
                        new GetSessionsBusinessResponse.SessionBusinessResponseElement()
                        {
                            SessionID = 10002,
                            SessionCode = "Session-B",
                            Name = "Session Bravo",
                            StartDate = new DateTime(2002, 2, 2)
                        },

                        // Mock the Session business response element.
                        new GetSessionsBusinessResponse.SessionBusinessResponseElement()
                        {
                            SessionID = 10003,
                            SessionCode = "Session-C",
                            Name = "Session Charlie",
                            StartDate = new DateTime(2003, 3, 3)
                        }
                    }
                }))
                .Verifiable();

            // Invoke the HTTP GET method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Get, "api/scheduling/sessions", "application/json");
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("[");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"SessionID\":10001,");
            expectedJsonContent.Append("\"SessionCode\":\"Session-A\",");
            expectedJsonContent.Append("\"Name\":\"Session Alpha\",");
            expectedJsonContent.Append("\"StartDate\":\"2001-01-01T00:00:00\"");
            expectedJsonContent.Append("},");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"SessionID\":10002,");
            expectedJsonContent.Append("\"SessionCode\":\"Session-B\",");
            expectedJsonContent.Append("\"Name\":\"Session Bravo\",");
            expectedJsonContent.Append("\"StartDate\":\"2002-02-02T00:00:00\"");
            expectedJsonContent.Append("},");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"SessionID\":10003,");
            expectedJsonContent.Append("\"SessionCode\":\"Session-C\",");
            expectedJsonContent.Append("\"Name\":\"Session Charlie\",");
            expectedJsonContent.Append("\"StartDate\":\"2003-03-03T00:00:00\"");
            expectedJsonContent.Append("}");
            expectedJsonContent.Append("]");

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }
    }
}
