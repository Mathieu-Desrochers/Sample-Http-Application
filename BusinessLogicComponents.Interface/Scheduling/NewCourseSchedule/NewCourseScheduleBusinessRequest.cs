
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.Infrastructure.Code.DataAnnotations.ValidationAttributes;
using SampleHttpApplication.Infrastructure.Interface.UniqueToken;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule
{
    /// <summary>
    /// Represents the NewCourseSchedule business request.
    /// </summary>
    public class NewCourseScheduleBusinessRequest
    {
        /// <summary>
        /// The session.
        /// </summary>
        public SessionBusinessRequestElement Session;
        public class SessionBusinessRequestElement
        {
            /// <summary>
            /// Gets or sets the SessionCode.
            /// </summary>
            public string SessionCode { get; set; }
        }

        /// <summary>
        /// The new course schedule.
        /// </summary>
        public CourseScheduleBusinessRequestElement CourseSchedule;
        public class CourseScheduleBusinessRequestElement
        {
            /// <summary>
            /// Gets or sets the DayOfWeek.
            /// </summary>
            public DayOfWeek DayOfWeek { get; set; }

            /// <summary>
            /// Gets or sets the Time.
            /// </summary>
            [TimeSpanRange("00:00:00", "23:59:59")]
            public TimeSpan Time { get; set; }
        }
    }
}
