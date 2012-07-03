
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.GetSessions;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;

namespace SampleHttpApplication.BusinessLogicComponents.Tests.Scheduling.GetSessions
{
    /// <summary>
    /// Represents the GetSessions operation tests.
    /// </summary>
    [TestClass]
    public class GetSessionsOperationTests
    {
        /// <summary>
        /// Should return zero Session response elements.
        /// </summary>
        [TestMethod]
        public void ShouldReturnZeroSessionsResponseElements()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data rows.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadAll(It.IsAny<IDatabaseConnection>()))
                .Returns(Task.FromResult(new SessionDataRow[0]))
                .Verifiable();

            // Build the GetSessions business request.
            GetSessionsBusinessRequest getSessionsBusinessRequest = new GetSessionsBusinessRequest();

            // Invoke the GetSessions business operation.
            GetSessionsBusinessResponse getSessionsBusinessResponse = testHarness.SchedulingBusinessLogicComponent.GetSessions(testHarness.MockedDatabaseConnection, getSessionsBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the Session business response elements count.
            Assert.IsNotNull(getSessionsBusinessResponse);
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions);
            Assert.AreEqual(0, getSessionsBusinessResponse.Sessions.Length);
        }

        /// <summary>
        /// Should return one Session response element.
        /// </summary>
        [TestMethod]
        public void ShouldReturnOneSessionResponseElement()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data rows.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadAll(It.IsAny<IDatabaseConnection>()))
                .Returns(Task.FromResult(new SessionDataRow[]
                {
                    new SessionDataRow()
                    {
                        SessionID = 10001,
                        SessionCode = "Session-A",
                        Name = "Session Alpha",
                        StartDate = new DateTime(2001, 1, 1)
                    }
                }))
                .Verifiable();

            // Build the GetSessions business request.
            GetSessionsBusinessRequest getSessionsBusinessRequest = new GetSessionsBusinessRequest();

            // Invoke the GetSessions business operation.
            GetSessionsBusinessResponse getSessionsBusinessResponse = testHarness.SchedulingBusinessLogicComponent.GetSessions(testHarness.MockedDatabaseConnection, getSessionsBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the Session business response elements count.
            Assert.IsNotNull(getSessionsBusinessResponse);
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions);
            Assert.AreEqual(1, getSessionsBusinessResponse.Sessions.Length);

            // Validate the Session business response element.
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions[0]);
            Assert.AreEqual(10001, getSessionsBusinessResponse.Sessions[0].SessionID);
            Assert.AreEqual("Session-A", getSessionsBusinessResponse.Sessions[0].SessionCode);
            Assert.AreEqual("Session Alpha", getSessionsBusinessResponse.Sessions[0].Name);
            Assert.AreEqual(new DateTime(2001, 1, 1), getSessionsBusinessResponse.Sessions[0].StartDate);
        }

        /// <summary>
        /// Should return multiple Session response elements.
        /// </summary>
        [TestMethod]
        public void ShouldReturnMultipleSessionsResponseElements()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data rows.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadAll(It.IsAny<IDatabaseConnection>()))
                .Returns(Task.FromResult(new SessionDataRow[]
                {
                    new SessionDataRow()
                    {
                        SessionID = 10001,
                        SessionCode = "Session-A",
                        Name = "Session Alpha",
                        StartDate = new DateTime(2001, 1, 1)
                    },
                    new SessionDataRow()
                    {
                        SessionID = 10002,
                        SessionCode = "Session-B",
                        Name = "Session Bravo",
                        StartDate = new DateTime(2002, 2, 2)
                    },
                    new SessionDataRow()
                    {
                        SessionID = 10003,
                        SessionCode = "Session-C",
                        Name = "Session Charlie",
                        StartDate = new DateTime(2003, 3, 3)
                    }
                }))
                .Verifiable();

            // Build the GetSessions business request.
            GetSessionsBusinessRequest getSessionsBusinessRequest = new GetSessionsBusinessRequest();

            // Invoke the GetSessions business operation.
            GetSessionsBusinessResponse getSessionsBusinessResponse = testHarness.SchedulingBusinessLogicComponent.GetSessions(testHarness.MockedDatabaseConnection, getSessionsBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the Session business response elements count.
            Assert.IsNotNull(getSessionsBusinessResponse);
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions);
            Assert.AreEqual(3, getSessionsBusinessResponse.Sessions.Length);

            // Validate the first Session business response element.
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions[0]);
            Assert.AreEqual(10001, getSessionsBusinessResponse.Sessions[0].SessionID);
            Assert.AreEqual("Session-A", getSessionsBusinessResponse.Sessions[0].SessionCode);
            Assert.AreEqual("Session Alpha", getSessionsBusinessResponse.Sessions[0].Name);
            Assert.AreEqual(new DateTime(2001, 1, 1), getSessionsBusinessResponse.Sessions[0].StartDate);

            // Validate the second Session business response element.
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions[1]);
            Assert.AreEqual(10002, getSessionsBusinessResponse.Sessions[1].SessionID);
            Assert.AreEqual("Session-B", getSessionsBusinessResponse.Sessions[1].SessionCode);
            Assert.AreEqual("Session Bravo", getSessionsBusinessResponse.Sessions[1].Name);
            Assert.AreEqual(new DateTime(2002, 2, 2), getSessionsBusinessResponse.Sessions[1].StartDate);

            // Validate the third Session business response element.
            Assert.IsNotNull(getSessionsBusinessResponse.Sessions[2]);
            Assert.AreEqual(10003, getSessionsBusinessResponse.Sessions[2].SessionID);
            Assert.AreEqual("Session-C", getSessionsBusinessResponse.Sessions[2].SessionCode);
            Assert.AreEqual("Session Charlie", getSessionsBusinessResponse.Sessions[2].Name);
            Assert.AreEqual(new DateTime(2003, 3, 3), getSessionsBusinessResponse.Sessions[2].StartDate);
        }
    }
}
