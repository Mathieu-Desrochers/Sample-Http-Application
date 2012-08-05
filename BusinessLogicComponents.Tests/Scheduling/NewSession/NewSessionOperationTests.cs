
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
        private void ShouldThrowInvalidSessionCodeErrorCode(string sessionCode)
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
                NewSessionBusinessException NewSessionBusinessException = ex.InnerExceptions[0] as NewSessionBusinessException;
                Assert.IsNotNull(NewSessionBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.", NewSessionBusinessException.Message);

                // Validate the NewSession business exception contains the InvalidSessionCode error code.
                Assert.IsNotNull(NewSessionBusinessException.Errors);
                Assert.AreEqual(1, NewSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.InvalidSessionCode, NewSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(sessionCode, NewSessionBusinessException.Errors[0].Value);
            }
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidSessionCodeErrorCode_GivenNullSessionCode()
        {
            ShouldThrowInvalidSessionCodeErrorCode(null);
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidSessionCodeErrorCode_GivenEmptySessionCode()
        {
            ShouldThrowInvalidSessionCodeErrorCode("");
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidSessionCodeErrorCode_GivenTooLongSessionCode()
        {
            ShouldThrowInvalidSessionCodeErrorCode(new String('a', 51));
        }

        /// <summary>
        /// Should throw the DuplicateSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowDuplicateSessionCodeErrorCode()
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
                NewSessionBusinessException NewSessionBusinessException = ex.InnerExceptions[0] as NewSessionBusinessException;
                Assert.IsNotNull(NewSessionBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.", NewSessionBusinessException.Message);

                // Validate the NewSession business exception contains the DuplicateSessionCode error code.
                Assert.IsNotNull(NewSessionBusinessException.Errors);
                Assert.AreEqual(1, NewSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.DuplicateSessionCode, NewSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual("Session-A", NewSessionBusinessException.Errors[0].Value);
            }
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        private void ShouldThrowInvalidNameErrorCode(string name)
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

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
                NewSessionBusinessException NewSessionBusinessException = ex.InnerExceptions[0] as NewSessionBusinessException;
                Assert.IsNotNull(NewSessionBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewSession() has thrown a NewSession business exception. See the Errors property for details.", NewSessionBusinessException.Message);

                // Validate the NewSession business exception contains the InvalidName error code.
                Assert.IsNotNull(NewSessionBusinessException.Errors);
                Assert.AreEqual(1, NewSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.InvalidName, NewSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(name, NewSessionBusinessException.Errors[0].Value);
            }
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidNameErrorCode_GivenNullName()
        {
            ShouldThrowInvalidNameErrorCode(null);
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidNameErrorCode_GivenEmptyName()
        {
            ShouldThrowInvalidNameErrorCode("");
        }

        /// <summary>
        /// Should throw the InvalidName error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidNameErrorCode_GivenTooLongName()
        {
            ShouldThrowInvalidNameErrorCode(new String('a', 51));
        }
    }
}
