
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewSession;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;

namespace SampleHttpApplication.BusinessLogicComponents.Tests.Scheduling.NewSession
{
    /// <summary>
    /// Represents the NewSession operation tests.
    /// </summary>
    [TestClass]
    public class NewSessionOperationTests
    {
        /// <summary>
        /// Should return the Session response element.
        /// </summary>
        [TestMethod]
        public void ShouldReturnSessionResponseElement()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadBySessionCode(It.IsAny<IDatabaseConnection>(), "Session-A"))
                .Returns(Task.FromResult<SessionDataRow>(null))
                .Verifiable();

            // Mock the creation of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<SessionDataRow>(sessionDataRow =>
                    (
                        sessionDataRow.SessionCode == "Session-A" &&
                        sessionDataRow.Name == "Session Alpha" &&
                        sessionDataRow.StartDate == new DateTime(2001, 1, 1)
                    ))))
                .Returns(Task.FromResult<object>(null))
                .Callback((IDatabaseConnection databaseConnection, SessionDataRow sessionDataRow) =>
                {
                    sessionDataRow.SessionID = 10001;
                })
                .Verifiable();

            // Build the NewSession business request.
            NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

            // Build the Session business request element.
            NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewSessionBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = "Session-A";
            sessionBusinessRequestElement.Name = "Session Alpha";
            sessionBusinessRequestElement.StartDate = new DateTime(2001, 1, 1);
            newSessionBusinessRequest.Session = sessionBusinessRequestElement;

            // Invoke the NewSession business operation.
            NewSessionBusinessResponse newSessionBusinessResponse = testHarness.SchedulingBusinessLogicComponent.NewSession(testHarness.MockedDatabaseConnection, newSessionBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the Session business response element.
            Assert.IsNotNull(newSessionBusinessResponse);
            Assert.IsNotNull(newSessionBusinessResponse.Session);
            Assert.AreEqual(10001, newSessionBusinessResponse.Session.SessionID);
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        private void ShouldThrowErrorCodeInvalidSessionCode(string sessionCode)
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Build the NewSession business request.
            NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

            // Build the Session business request element.
            NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewSessionBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = sessionCode;
            sessionBusinessRequestElement.Name = "Session Alpha";
            sessionBusinessRequestElement.StartDate = new DateTime(2001, 1, 1);
            newSessionBusinessRequest.Session = sessionBusinessRequestElement;

            try
            {
                // Invoke the NewSession business operation.
                testHarness.SchedulingBusinessLogicComponent.NewSession(testHarness.MockedDatabaseConnection, newSessionBusinessRequest).Wait();

                // Validate an exception was thrown.
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Verify the mocked components.
                testHarness.VerifyMockedComponents();

                // Validate a NewSession business exception was thrown.
                NewSessionBusinessException newSessionBusinessException = ex.InnerExceptions[0] as NewSessionBusinessException;
                Assert.IsNotNull(newSessionBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.", newSessionBusinessException.Message);

                // Validate NewSession business exception has the InvalidSessionCode error code.
                Assert.IsNotNull(newSessionBusinessException.Errors);
                Assert.AreEqual(1, newSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.InvalidSessionCode, newSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(sessionCode, newSessionBusinessException.Errors[0].Value);
            }
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeInvalidSessionCode_GivenNullSessionCode()
        {
            ShouldThrowErrorCodeInvalidSessionCode(null);
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeInvalidSessionCode_GivenEmptySessionCode()
        {
            ShouldThrowErrorCodeInvalidSessionCode("");
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeInvalidSessionCode_GivenTooLongSessionCode()
        {
            ShouldThrowErrorCodeInvalidSessionCode(new String('a', 51));
        }

        /// <summary>
        /// Should throw the DuplicateSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeDuplicateSessionCode()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadBySessionCode(It.IsAny<IDatabaseConnection>(), "Session-A"))
                .Returns(Task.FromResult(new SessionDataRow()))
                .Verifiable();

            // Build the NewSession business request.
            NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

            // Build the Session business request element.
            NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewSessionBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = "Session-A";
            sessionBusinessRequestElement.Name = "Session Alpha";
            sessionBusinessRequestElement.StartDate = new DateTime(2001, 1, 1);
            newSessionBusinessRequest.Session = sessionBusinessRequestElement;

            try
            {
                // Invoke the NewSession business operation.
                testHarness.SchedulingBusinessLogicComponent.NewSession(testHarness.MockedDatabaseConnection, newSessionBusinessRequest).Wait();

                // Validate an exception was thrown.
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Verify the mocked components.
                testHarness.VerifyMockedComponents();

                // Validate a NewSession business exception was thrown.
                NewSessionBusinessException newSessionBusinessException = ex.InnerExceptions[0] as NewSessionBusinessException;
                Assert.IsNotNull(newSessionBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.", newSessionBusinessException.Message);

                // Validate NewSession business exception has the DuplicateSessionCode error code.
                Assert.IsNotNull(newSessionBusinessException.Errors);
                Assert.AreEqual(1, newSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.DuplicateSessionCode, newSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual("Session-A", newSessionBusinessException.Errors[0].Value);
            }
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        private void ShouldThrowErrorCodeInvalidName(string name)
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadBySessionCode(It.IsAny<IDatabaseConnection>(), "Session-A"))
                .Returns(Task.FromResult<SessionDataRow>(null))
                .Verifiable();

            // Build the NewSession business request.
            NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

            // Build the Session business request element.
            NewSessionBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewSessionBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = "Session-A";
            sessionBusinessRequestElement.Name = name;
            sessionBusinessRequestElement.StartDate = new DateTime(2001, 1, 1);
            newSessionBusinessRequest.Session = sessionBusinessRequestElement;

            try
            {
                // Invoke the NewSession business operation.
                testHarness.SchedulingBusinessLogicComponent.NewSession(testHarness.MockedDatabaseConnection, newSessionBusinessRequest).Wait();

                // Validate an exception was thrown.
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Verify the mocked components.
                testHarness.VerifyMockedComponents();

                // Validate a NewSession business exception was thrown.
                NewSessionBusinessException newSessionBusinessException = ex.InnerExceptions[0] as NewSessionBusinessException;
                Assert.IsNotNull(newSessionBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.", newSessionBusinessException.Message);

                // Validate NewSession business exception has the InvalidName error code.
                Assert.IsNotNull(newSessionBusinessException.Errors);
                Assert.AreEqual(1, newSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.InvalidName, newSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(name, newSessionBusinessException.Errors[0].Value);
            }
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeInvalidName_GivenNullName()
        {
            ShouldThrowErrorCodeInvalidName(null);
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeInvalidName_GivenEmptyName()
        {
            ShouldThrowErrorCodeInvalidName("");
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowErrorCodeInvalidName_GivenTooLongName()
        {
            ShouldThrowErrorCodeInvalidName(new String('a', 51));
        }
    }
}
