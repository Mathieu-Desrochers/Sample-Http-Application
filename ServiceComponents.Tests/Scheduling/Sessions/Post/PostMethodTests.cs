﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.ServiceComponents.Tests.Scheduling.Sessions.Post
{
    /// <summary>
    /// Represents the HTTP POST method tests.
    /// </summary>
    [TestClass]
    public class PostMethodTests
    {
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
                        newSessionBusinessRequest.Session.SessionCode == "Session-A" &&
                        newSessionBusinessRequest.Session.Name == "Session Alpha" &&
                        newSessionBusinessRequest.Session.StartDate == new DateTime(2001, 1, 1)
                    ))))
                .Returns(Task.FromResult(new NewSessionBusinessResponse()
                {
                    // Mock the Session business response element.
                    Session = new NewSessionBusinessResponse.SessionBusinessResponseElement()
                    {
                        SessionID = 10001,
                    }
                }))
                .Verifiable();

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"sessionCode\":\"Session-A\",");
            requestJsonContent.Append("\"name\":\"Session Alpha\",");
            requestJsonContent.Append("\"startDate\":\"2001-01-01T00:00:00\"");
            requestJsonContent.Append("}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/sessions", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"sessionID\":10001,");
            expectedJsonContent.Append("\"sessionCode\":\"Session-A\",");
            expectedJsonContent.Append("\"name\":\"Session Alpha\",");
            expectedJsonContent.Append("\"startDate\":\"2001-01-01T00:00:00\"");
            expectedJsonContent.Append("}");

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }

        /// <summary>
        /// Should return the SessionCode invalid field.
        /// </summary>
        [TestMethod]
        public void ShouldReturnSessionCodeInvalidField()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness(false, false);

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"sessionCode\":\"\",");
            requestJsonContent.Append("\"name\":\"Session Alpha\",");
            requestJsonContent.Append("\"startDate\":\"2001-01-01T00:00:00\"");
            requestJsonContent.Append("}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/sessions", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"invalidFields\":[\"sessionCode\"]");
            expectedJsonContent.Append("}");

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }

        /// <summary>
        /// Should return the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldReturnInvalidSessionCodeErrorCode()
        {
            // Build the test harness.
            SessionsControllerTestHarness testHarness = new SessionsControllerTestHarness(true, false);

            // Mock the invocation of the NewSession business operation.
            testHarness.MockedSchedulingBusinessLogicComponent
                .Setup(mock => mock.NewSession(It.IsAny<IDatabaseConnection>(), It.IsAny<NewSessionBusinessRequest>()))
                .Throws(new NewSessionBusinessException()
                {
                    ErrorCode = NewSessionBusinessException.ErrorCodes.InvalidSessionCode
                })
                .Verifiable();

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"sessionCode\":\"Session-A\",");
            requestJsonContent.Append("\"name\":\"Session Alpha\",");
            requestJsonContent.Append("\"startDate\":\"2001-01-01T00:00:00\"");
            requestJsonContent.Append("}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/sessions", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"errorCode\":\"InvalidSessionCode\"");
            expectedJsonContent.Append("}");

            // Validate the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }
    }
}