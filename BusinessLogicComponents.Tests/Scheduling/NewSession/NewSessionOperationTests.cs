
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

            // Mock the generation of the unique token.
            testHarness.MockedUniqueTokenGenerator
                .Setup(mock => mock.GenerateUniqueToken())
                .Returns("6dk61ufcuzp3f7vs")
                .Verifiable();

            // Mock the creation of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<SessionDataRow>(sessionDataRow =>
                    (
                        sessionDataRow.SessionCode == "6dk61ufcuzp3f7vs" &&
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
            Assert.AreEqual("6dk61ufcuzp3f7vs", newSessionBusinessResponse.Session.SessionCode);
        }

        /// <summary>
        /// Should throw the InvalidSession error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidSessionErrorCode()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Build the NewSession business request.
            NewSessionBusinessRequest newSessionBusinessRequest = new NewSessionBusinessRequest();

            // Build the Session business request element.
            newSessionBusinessRequest.Session = null;

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

                // Validate the NewSession business exception contains the InvalidSession error code.
                Assert.IsNotNull(NewSessionBusinessException.Errors);
                Assert.AreEqual(1, NewSessionBusinessException.Errors.Length);
                Assert.AreEqual(NewSessionBusinessException.ErrorCodes.InvalidSession, NewSessionBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(null, NewSessionBusinessException.Errors[0].ErroneousValue);
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
                Assert.AreEqual(name, NewSessionBusinessException.Errors[0].ErroneousValue);
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
