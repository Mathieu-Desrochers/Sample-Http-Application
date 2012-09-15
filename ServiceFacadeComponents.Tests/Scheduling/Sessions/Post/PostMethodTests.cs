
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.ServiceFacadeComponents.Tests.Scheduling.Sessions.Post
{
    /// <summary>
    /// Represents the HTTP POST method tests.
    /// </summary>
    [TestClass]
    public class PostMethodTests
    {
        /// <summary>
        /// Should succeed.
        /// </summary>
        [TestMethod]
        public void ShouldSucceed()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness(true, true);

            // Mock the invocation of the NewSession business operation.
            testHarness.MockedSchedulingBusinessLogicComponent
                .Setup(mock => mock.NewSession(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<NewSessionBusinessRequest>(newSessionBusinessRequest =>
                    (
                        // Match the Session business request element.
                        newSessionBusinessRequest.Session.Name == "Session Alpha" &&
                        newSessionBusinessRequest.Session.StartDate == new DateTime(2001, 1, 1)
                    ))))
                .Returns(Task.FromResult(new NewSessionBusinessResponse()
                {
                    // Mock the Session business response element.
                    Session = new NewSessionBusinessResponse.SessionBusinessResponseElement()
                    {
                        SessionCode = "6dk61ufcuzp3f7vs"
                    }
                }))
                .Verifiable();

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{");
            requestJsonContent.Append("session:");
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"name\":\"Session Alpha\",");
            requestJsonContent.Append("\"startDate\":\"2001-01-01T00:00:00\"");
            requestJsonContent.Append("}");
            requestJsonContent.Append("}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/sessions", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"session\":");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"sessionCode\":\"6dk61ufcuzp3f7vs\",");
            expectedJsonContent.Append("\"name\":\"Session Alpha\",");
            expectedJsonContent.Append("\"startDate\":\"2001-01-01T00:00:00\"");
            expectedJsonContent.Append("}");
            expectedJsonContent.Append("}");

            // Read the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            // Validate the HTTP response message.
            Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }

        /// <summary>
        /// Should return all the error codes.
        /// </summary>
        [TestMethod]
        public void ShouldReturnErrorCodes()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness(true, false);

            // Mock the invocation of the NewSession business operation.
            testHarness.MockedSchedulingBusinessLogicComponent
                .Setup(mock => mock.NewSession(It.IsAny<IDatabaseConnection>(), It.IsAny<NewSessionBusinessRequest>()))
                .Throws(new NewSessionBusinessException()
                {
                    // Mock the NewSession business exception.
                    Errors = new NewSessionBusinessException.ErrorBusinessExceptionElement[]
                    {
                        new NewSessionBusinessException.ErrorBusinessExceptionElement() { ErrorCode = NewSessionBusinessException.ErrorCodes.InvalidSession, ErroneousValue = null },
                        new NewSessionBusinessException.ErrorBusinessExceptionElement() { ErrorCode = NewSessionBusinessException.ErrorCodes.InvalidName, ErroneousValue = "Session Alpha" }
                    }
                })
                .Verifiable();

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{");
            requestJsonContent.Append("}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/sessions", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("[");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"errorCode\":\"InvalidSession\",");
            expectedJsonContent.Append("\"erroneousValue\":null");
            expectedJsonContent.Append("},");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"errorCode\":\"InvalidName\",");
            expectedJsonContent.Append("\"erroneousValue\":\"Session Alpha\"");
            expectedJsonContent.Append("}");
            expectedJsonContent.Append("]");

            // Read the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            // Validate the HTTP response message.
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }

        /// <summary>
        /// Should return BadRequest.
        /// </summary>
        [TestMethod]
        public void ShouldReturnBadRequest()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness(false, false);

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{x}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/sessions", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the HTTP response message.
            Assert.AreEqual(HttpStatusCode.BadRequest, httpResponseMessage.StatusCode);
        }
    }
}
