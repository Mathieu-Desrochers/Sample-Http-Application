
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface;
using SampleHttpApplication.DataAccessComponents.Interface.CourseGroup;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface.Session;

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

            // Build the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement[0];

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
        /// Should return zero CourseGroup response elements.
        /// </summary>
        [TestMethod]
        public void ShouldReturnZeroCourseGroupsResponseElements()
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

            // Build the CourseGroup business request elements.
            List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement> courseGroupBusinessRequestElements = new List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement>();

            // Set the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = courseGroupBusinessRequestElements.ToArray();

            // Invoke the NewCourseSchedule business operation.
            NewCourseScheduleBusinessResponse newCourseScheduleBusinessResponse = testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the CourseGroup business response elements count.
            Assert.IsNotNull(newCourseScheduleBusinessResponse);
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups);
            Assert.AreEqual(0, newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups.Length);
        }

        /// <summary>
        /// Should return one CourseGroup response element.
        /// </summary>
        [TestMethod]
        public void ShouldReturnOneCourseGroupsResponseElement()
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

            // Mock the generation of the unique tokens.
            Queue<string> uniqueTokens = new Queue<string>(new string[] { "zzcj32kpd6huzp1n", "5s1cgndj6e5x0uvz" });
            testHarness.MockedUniqueTokenGenerator
                .Setup(mock => mock.GenerateUniqueToken())
                .Returns(() => uniqueTokens.Dequeue())
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

            // Mock the creation of the CourseGroup data row.
            testHarness.MockedCourseGroupDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<CourseGroupDataRow>(courseGroupDataRow =>
                    (
                        courseGroupDataRow.CourseGroupCode == "5s1cgndj6e5x0uvz" &&
                        courseGroupDataRow.CourseScheduleID == 20001 &&
                        courseGroupDataRow.PlacesCount == 1
                    ))))
                .Returns(Task.FromResult<object>(null))
                .Callback((IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow) =>
                {
                    courseGroupDataRow.CourseGroupID = 30001;
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

            // Build the CourseGroup business request elements.
            List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement> courseGroupBusinessRequestElements = new List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement>();

            // Build the CourseGroup business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement courseGroupBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement();
            courseGroupBusinessRequestElement.PlacesCount = 1;
            courseGroupBusinessRequestElements.Add(courseGroupBusinessRequestElement);

            // Set the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = courseGroupBusinessRequestElements.ToArray();

            // Invoke the NewCourseSchedule business operation.
            NewCourseScheduleBusinessResponse newCourseScheduleBusinessResponse = testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the CourseGroup business response elements count.
            Assert.IsNotNull(newCourseScheduleBusinessResponse);
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups);
            Assert.AreEqual(1, newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups.Length);

            // Validate the CourseGroup business response element.
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[0]);
            Assert.AreEqual("5s1cgndj6e5x0uvz", newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[0].CourseGroupCode);
        }

        /// <summary>
        /// Should return multiple CourseGroup response elements.
        /// </summary>
        [TestMethod]
        public void ShouldReturnMultipleCourseGroupsResponseElements()
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

            // Mock the generation of the unique tokens.
            Queue<string> uniqueTokens = new Queue<string>(new string[] { "zzcj32kpd6huzp1n", "5s1cgndj6e5x0uvz", "78zcn25ynkaz50ef", "q5692qwy70qde9uv" });
            testHarness.MockedUniqueTokenGenerator
                .Setup(mock => mock.GenerateUniqueToken())
                .Returns(() => uniqueTokens.Dequeue())
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

            // Mock the creation of the first CourseGroup data row.
            testHarness.MockedCourseGroupDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<CourseGroupDataRow>(courseGroupDataRow =>
                    (
                        courseGroupDataRow.CourseGroupCode == "5s1cgndj6e5x0uvz" &&
                        courseGroupDataRow.CourseScheduleID == 20001 &&
                        courseGroupDataRow.PlacesCount == 1
                    ))))
                .Returns(Task.FromResult<object>(null))
                .Callback((IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow) =>
                {
                    courseGroupDataRow.CourseGroupID = 30001;
                })
                .Verifiable();

            // Mock the creation of the second CourseGroup data row.
            testHarness.MockedCourseGroupDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<CourseGroupDataRow>(courseGroupDataRow =>
                    (
                        courseGroupDataRow.CourseGroupCode == "78zcn25ynkaz50ef" &&
                        courseGroupDataRow.CourseScheduleID == 20001 &&
                        courseGroupDataRow.PlacesCount == 2
                    ))))
                .Returns(Task.FromResult<object>(null))
                .Callback((IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow) =>
                {
                    courseGroupDataRow.CourseGroupID = 30002;
                })
                .Verifiable();

            // Mock the creation of the third CourseGroup data row.
            testHarness.MockedCourseGroupDataAccessComponent
                .Setup(mock => mock.Create(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<CourseGroupDataRow>(courseGroupDataRow =>
                    (
                        courseGroupDataRow.CourseGroupCode == "q5692qwy70qde9uv" &&
                        courseGroupDataRow.CourseScheduleID == 20001 &&
                        courseGroupDataRow.PlacesCount == 3
                    ))))
                .Returns(Task.FromResult<object>(null))
                .Callback((IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow) =>
                {
                    courseGroupDataRow.CourseGroupID = 30003;
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

            // Build the CourseGroup business request elements.
            List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement> courseGroupBusinessRequestElements = new List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement>();

            // Build the first CourseGroup business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement firstCourseGroupBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement();
            firstCourseGroupBusinessRequestElement.PlacesCount = 1;
            courseGroupBusinessRequestElements.Add(firstCourseGroupBusinessRequestElement);

            // Build the second CourseGroup business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement secondCourseGroupBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement();
            secondCourseGroupBusinessRequestElement.PlacesCount = 2;
            courseGroupBusinessRequestElements.Add(secondCourseGroupBusinessRequestElement);

            // Build the third CourseGroup business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement thirdCourseGroupBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement();
            thirdCourseGroupBusinessRequestElement.PlacesCount = 3;
            courseGroupBusinessRequestElements.Add(thirdCourseGroupBusinessRequestElement);

            // Set the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = courseGroupBusinessRequestElements.ToArray();

            // Invoke the NewCourseSchedule business operation.
            NewCourseScheduleBusinessResponse newCourseScheduleBusinessResponse = testHarness.SchedulingBusinessLogicComponent.NewCourseSchedule(testHarness.MockedDatabaseConnection, newCourseScheduleBusinessRequest).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Validate the CourseGroup business response elements count.
            Assert.IsNotNull(newCourseScheduleBusinessResponse);
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups);
            Assert.AreEqual(3, newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups.Length);

            // Validate the first CourseGroup business response element.
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[0]);
            Assert.AreEqual("5s1cgndj6e5x0uvz", newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[0].CourseGroupCode);

            // Validate the second CourseGroup business response element.
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[1]);
            Assert.AreEqual("78zcn25ynkaz50ef", newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[1].CourseGroupCode);

            // Validate the third CourseGroup business response element.
            Assert.IsNotNull(newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[2]);
            Assert.AreEqual("q5692qwy70qde9uv", newCourseScheduleBusinessResponse.CourseSchedule.CourseGroups[2].CourseGroupCode);
        }

        /// <summary>
        /// Should throw the InvalidSession error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidSessionErrorCode()
        {
            // Build the test harness.
            SchedulingBusinessLogicComponentTestHarness testHarness = new SchedulingBusinessLogicComponentTestHarness();

            // Build the NewCourseSchedule business request.
            NewCourseScheduleBusinessRequest newCourseScheduleBusinessRequest = new NewCourseScheduleBusinessRequest();

            // Build the Session business request element.
            newCourseScheduleBusinessRequest.Session = null;

            // Build the CourseSchedule business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement courseScheduleBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement();
            courseScheduleBusinessRequestElement.DayOfWeek = DayOfWeek.Monday;
            courseScheduleBusinessRequestElement.Time = new TimeSpan(9, 15, 0);
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            // Build the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement[0];

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

                // Validate the NewCourseSchedule business exception contains the InvalidSession error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidSession, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(null, NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
            }
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

            // Build the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement[0];

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

            // Build the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement[0];

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
        /// Should throw the InvalidCourseSchedule error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidCourseScheduleErrorCode()
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
            newCourseScheduleBusinessRequest.CourseSchedule = null;

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

                // Validate the NewCourseSchedule business exception contains the InvalidCourseSchedule error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidCourseSchedule, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(null, NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
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

            // Build the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement[0];

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

        /// <summary>
        /// Should throw the InvalidCourseGroups error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidCourseGroupsErrorCode()
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
            courseScheduleBusinessRequestElement.Time = new TimeSpan(9, 15, 0);
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            // Build the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = null;

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

                // Validate the NewCourseSchedule business exception contains the InvalidCourseGroups error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidCourseGroups, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(null, NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
            }
        }

        /// <summary>
        /// Should throw the InvalidPlacesCount error code.
        /// </summary>
        private void ShouldThrowInvalidPlacesCountErrorCode(int placesCount)
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
            courseScheduleBusinessRequestElement.Time = new TimeSpan(9, 15, 0);
            newCourseScheduleBusinessRequest.CourseSchedule = courseScheduleBusinessRequestElement;

            // Build the CourseGroup business request elements.
            List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement> courseGroupBusinessRequestElements = new List<NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement>();

            // Build the CourseGroup business request element.
            NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement courseGroupBusinessRequestElement = new NewCourseScheduleBusinessRequest.CourseScheduleBusinessRequestElement.CourseGroupBusinessRequestElement();
            courseGroupBusinessRequestElement.PlacesCount = placesCount;
            courseGroupBusinessRequestElements.Add(courseGroupBusinessRequestElement);

            // Set the CourseGroup business request elements.
            courseScheduleBusinessRequestElement.CourseGroups = courseGroupBusinessRequestElements.ToArray();

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

                // Validate the NewCourseSchedule business exception contains the InvalidPlacesCount error code.
                Assert.IsNotNull(NewCourseScheduleBusinessException.Errors);
                Assert.AreEqual(1, NewCourseScheduleBusinessException.Errors.Length);
                Assert.AreEqual(NewCourseScheduleBusinessException.ErrorCodes.InvalidPlacesCount, NewCourseScheduleBusinessException.Errors[0].ErrorCode);
                Assert.AreEqual(placesCount, NewCourseScheduleBusinessException.Errors[0].ErroneousValue);
            }
        }

        /// <summary>
        /// Should throw the InvalidPlacesCount error code.
        /// </summary>
        [TestMethod]
        public void ShouldThrowInvalidPlacesCountErrorCode_GivenNegativePlacesCount()
        {
            ShouldThrowInvalidPlacesCountErrorCode(-1);
        }
    }
}
