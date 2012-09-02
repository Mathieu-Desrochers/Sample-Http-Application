
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;

namespace SampleHttpApplication.BusinessLogicComponents.Tests.Scheduling.NewCourseSchedule
{
    /// <summary>
    /// Represents the NewCourseSchedule operation tests.
    /// </summary>
    [TestClass]
    public class NewCourseScheduleOperationTests
    {
        /// <summary>
        /// Should return the CourseSchedule response element.
        /// </summary>
        [TestMethod]
        public void ShouldReturnCourseScheduleResponseElement()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadBySessionCode(It.IsAny<IDatabaseConnection>(), "6dk61ufcuzp3f7vs"))
                .Returns(Task.FromResult(new SessionDataRow()
                {
                    SessionID = 10001,
                    SessionCode = "6dk61ufcuzp3f7vs",
                    Name = "Session Alpha",
                    StartDate = new DateTime(2001, 1, 1)
                }))
                .Verifiable();

            // Mock the generation of the unique token.
            testHarness.MockedUniqueTokenGenerator
                .Setup(mock => mock.GenerateUniqueToken())
                .Returns("zzcj32kpd6huzp1n")
                .Verifiable();

            // Mock the creation of the CourseSchedule data row.
            testHarness.MockedCourseScheduleDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<CourseScheduleDataRow>(courseScheduleDataRow =>
                    (
                        courseScheduleDataRow.CourseScheduleCode == "zzcj32kpd6huzp1n" &&
                        courseScheduleDataRow.SessionID == 10001 &&
                        courseScheduleDataRow.DayOfWeek == (int)DayOfWeek.Monday &&
                        courseScheduleDataRow.Time == new TimeSpan(9, 15, 0)
                    ))))
                .Returns(Task.FromResult<object>(null))
                .Callback((IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow) =>
                {
                    courseScheduleDataRow.CourseScheduleID = 20001;
                })
                .Verifiable();

            // Build the NewCourseSchedule business request.
            NewCourseScheduleBusinessRequest newCourseScheduleBusinessRequest = new NewCourseScheduleBusinessRequest();

            // Build the Session business request element.
            NewCourseScheduleBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewCourseScheduleBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = "6dk61ufcuzp3f7vs";
            newCourseScheduleBusinessRequest.Session = sessionBusinessRequestElement;

            // Build the CourseSchedule business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement();
            courseScheduleBusinessRequestElement.DayOfWeek = DayOfWeek.Monday;
            courseScheduleBusinessRequestElement.Time = new TimeSpan(9, 15, 0);
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            // Invoke the NewCourseSchedule business operation.
            NewCourseScheduleBusinessResponse newCourseScheduleBusinessResponse = testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the CourseSchedule business response element.
            Assert.IsNotNull(newCourseScheduleBusinessResponse);
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule);
            Assert.AreEqual("zzcj32kpd6huzp1n", newCourseScheduleBusinessResponse.CourseSchedule.CourseScheduleCode);
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        private void ShouldThrowInvalidSessionCodeErrorCode(string sessionCode)
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Build the NewCourseSchedule business request.
            NewCourseScheduleBusinessRequest newCourseScheduleBusinessRequest = new NewCourseScheduleBusinessRequest();

            // Build the Session business request element.
            NewCourseScheduleBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewCourseScheduleBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = sessionCode;
            newCourseScheduleBusinessRequest.Session = sessionBusinessRequestElement;

            // Build the CourseSchedule business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement();
            courseScheduleBusinessRequestElement.DayOfWeek = DayOfWeek.Monday;
            courseScheduleBusinessRequestElement.Time = new TimeSpan(9, 15, 0);
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            try
            {
                // Invoke the NewCourseSchedule business operation.
                testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Wait();

                // Validate an exception was thrown.
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Verify the mocked components.
                testHarness.VerifyMockedComponents();

                // Validate a NewCourseSchedule business exception was thrown.
                NewCourseScheduleBusinessException NewCourseScheduleBusinessException = ex.InnerExceptions[0] as NewCourseScheduleBusinessException;
                Assert.IsNotNull(NewCourseScheduleBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewCourseSchedule() has thrown a NewCourseSchedule business exception. See the Errors property for details.", NewCourseScheduleBusinessException.Message);

                // Validate the NewCourseSchedule business exception contains the InvalidSessionCode error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidSessionCode, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(sessionCode, NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
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
        public void ShouldThrowInvalidSessionCodeErrorCode_GivenNonRegularExpressionMatchingSessionCode()
        {
            ShouldThrowInvalidSessionCodeErrorCode("x");
        }

        /// <summary>
        /// Should throw the InvalidSessionCode error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidSessionCodeErrorCode_GivenNonExistingSessionCode()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Mock the reading of the Session data row.
            testHarness.MockedSessionDataAccessComponent
                .Setup(mock => mock.ReadBySessionCode(It.IsAny<IDatabaseConnection>(), "6dk61ufcuzp3f7vs"))
                .Returns(Task.FromResult<SessionDataRow>(null))
                .Verifiable();

            // Build the NewCourseSchedule business request.
            NewCourseScheduleBusinessRequest newCourseScheduleBusinessRequest = new NewCourseScheduleBusinessRequest();

            // Build the Session business request element.
            NewCourseScheduleBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewCourseScheduleBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = "6dk61ufcuzp3f7vs";
            newCourseScheduleBusinessRequest.Session = sessionBusinessRequestElement;

            // Build the CourseSchedule business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement();
            courseScheduleBusinessRequestElement.DayOfWeek = DayOfWeek.Monday;
            courseScheduleBusinessRequestElement.Time = new TimeSpan(9, 15, 0);
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            try
            {
                // Invoke the NewCourseSchedule business operation.
                testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Wait();

                // Validate an exception was thrown.
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Verify the mocked components.
                testHarness.VerifyMockedComponents();

                // Validate a NewCourseSchedule business exception was thrown.
                NewCourseScheduleBusinessException NewCourseScheduleBusinessException = ex.InnerExceptions[0] as NewCourseScheduleBusinessException;
                Assert.IsNotNull(NewCourseScheduleBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewCourseSchedule() has thrown a NewCourseSchedule business exception. See the Errors property for details.", NewCourseScheduleBusinessException.Message);

                // Validate the NewCourseSchedule business exception contains the InvalidSessionCode error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidSessionCode, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual("6dk61ufcuzp3f7vs", NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
            }
        }

        /// <summary>
        /// Should throw the InvalidTime error code.
        /// </summary>
        private void ShouldThrowInvalidTimeErrorCode(TimeSpan time)
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Build the NewCourseSchedule business request.
            NewCourseScheduleBusinessRequest newCourseScheduleBusinessRequest = new NewCourseScheduleBusinessRequest();

            // Build the Session business request element.
            NewCourseScheduleBusinessRequest.SessionBusinessRequestElement sessionBusinessRequestElement = new NewCourseScheduleBusinessRequest.SessionBusinessRequestElement();
            sessionBusinessRequestElement.SessionCode = "6dk61ufcuzp3f7vs";
            newCourseScheduleBusinessRequest.Session = sessionBusinessRequestElement;

            // Build the CourseSchedule business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement();
            courseScheduleBusinessRequestElement.DayOfWeek = DayOfWeek.Monday;
            courseScheduleBusinessRequestElement.Time = time;
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            try
            {
                // Invoke the NewCourseSchedule business operation.
                testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Wait();

                // Validate an exception was thrown.
                Assert.Fail();
            }
            catch (AggregateException ex)
            {
                // Verify the mocked components.
                testHarness.VerifyMockedComponents();

                // Validate a NewCourseSchedule business exception was thrown.
                NewCourseScheduleBusinessException NewCourseScheduleBusinessException = ex.InnerExceptions[0] as NewCourseScheduleBusinessException;
                Assert.IsNotNull(NewCourseScheduleBusinessException);
                Assert.AreEqual("SchedulingBusinessLogicComponent.NewCourseSchedule() has thrown a NewCourseSchedule business exception. See the Errors property for details.", NewCourseScheduleBusinessException.Message);

                // Validate the NewCourseSchedule business exception contains the InvalidTime error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidTime, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(time, NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
            }
        }

        /// <summary>
        /// Should throw the InvalidTime error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidTimeErrorCode_GivenTooEarlyTime()
        {
            ShouldThrowInvalidTimeErrorCode(new TimeSpan(0, 0, -1));
        }

        /// <summary>
        /// Should throw the InvalidTime error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidTimeErrorCode_GivenTooLateTime()
        {
            ShouldThrowInvalidTimeErrorCode(new TimeSpan(24, 0, 0));
        }
    }
}
