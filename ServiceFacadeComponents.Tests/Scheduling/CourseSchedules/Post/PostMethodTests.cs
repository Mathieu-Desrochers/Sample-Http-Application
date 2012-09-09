
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

using SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface;

namespace SampleHttpApplication.ServiceFacadeComponents.Tests.Scheduling.CourseSchedules.Post
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
            CourseSchedulesControllerTestHarness testHarness = new CourseSchedulesControllerTestHarness(true, true);

            // Mock the invocation of the NewCourseSchedule business operation.
            testHarness.MockedSchedulingBusinessLogicComponent
                .Setup(mock => mock.NewCourseSchedule(
                    It.IsAny<IDatabaseConnection>(),
                    It.Is<NewCourseScheduleBusinessRequest>(newCourseScheduleBusinessRequest =>
                    (
                        // Match the Session business request element.
                        newCourseScheduleBusinessRequest.Session.SessionCode == "6dk61ufcuzp3f7vs" &&

                        // Match the CourseSchedule business request element.
                        newCourseScheduleBusinessRequest.CourseSchedule.DayOfWeek == DayOfWeek.Monday &&
                        newCourseScheduleBusinessRequest.CourseSchedule.Time == new TimeSpan(9, 15, 0)
                    ))))
                .Returns(Task.FromResult(new NewCourseScheduleBusinessResponse()
                {
                    // Mock the CourseSchedule business response element.
                    CourseSchedule = new NewCourseScheduleBusinessResponse.CourseScheduleBusinessResponseElement()
                    {
                        CourseScheduleCode = "zzcj32kpd6huzp1n"
                    }
                }))
                .Verifiable();

            // Build the request JSON content.
            StringBuilder requestJsonContent = new StringBuilder();
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"session\":");
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"sessionCode\":\"6dk61ufcuzp3f7vs\"");
            requestJsonContent.Append("},");
            requestJsonContent.Append("\"courseSchedule\":");
            requestJsonContent.Append("{");
            requestJsonContent.Append("\"dayOfWeek\":1,");
            requestJsonContent.Append("\"time\":\"09:15:00\"");
            requestJsonContent.Append("}");
            requestJsonContent.Append("}");

            // Invoke the HTTP POST method.
            HttpRequestMessage httpRequestMessage = testHarness.BuildHttpRequest(HttpMethod.Post, "api/scheduling/course-schedules", requestJsonContent.ToString());
            HttpResponseMessage httpResponseMessage = testHarness.HttpClient.SendAsync(httpRequestMessage).Result;

            // Verify the mocked components.
            testHarness.VerifyMockedComponents();

            // Build the expected JSON content.
            StringBuilder expectedJsonContent = new StringBuilder();
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"session\":");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"sessionCode\":\"6dk61ufcuzp3f7vs\"");
            expectedJsonContent.Append("},");
            expectedJsonContent.Append("\"courseSchedule\":");
            expectedJsonContent.Append("{");
            expectedJsonContent.Append("\"courseScheduleCode\":\"zzcj32kpd6huzp1n\",");
            expectedJsonContent.Append("\"dayOfWeek\":1,");
            expectedJsonContent.Append("\"time\":\"09:15:00\"");
            expectedJsonContent.Append("}");
            expectedJsonContent.Append("}");

            // Read the HTTP response message content.
            string httpResponseMessageContent = httpResponseMessage.Content.ReadAsStringAsync().Result;

            // Validate the HTTP response message.
            Assert.AreEqual(HttpStatusCode.Created, httpResponseMessage.StatusCode);
            Assert.AreEqual(expectedJsonContent.ToString(), httpResponseMessageContent);
        }
    }
}
