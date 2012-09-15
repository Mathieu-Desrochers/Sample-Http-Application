
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.ServiceFacadeComponents.Interface.Scheduling.CourseSchedules
{
    /// <summary>
    /// Represents the CourseSchedule resource.
    /// </summary>
    public class CourseScheduleResource
    {
        /// <summary>
        /// The session.
        /// </summary>
        public SessionResourceElement Session { get; set; }
        public class SessionResourceElement
        {
            /// <summary>
            /// Gets or sets the SessionCode.
            /// </summary>
            public string SessionCode { get; set; }
        }

        /// <summary>
        /// The course schedule.
        /// </summary>
        public CourseScheduleResourceElement CourseSchedule { get; set; }
        public class CourseScheduleResourceElement
        {
            /// <summary>
            /// Gets or sets the CourseScheduleCode.
            /// </summary>
            public string CourseScheduleCode { get; set; }

            /// <summary>
            /// Gets or sets the DayOfWeek.
            /// </summary>
            public DayOfWeek DayOfWeek { get; set; }

            /// <summary>
            /// Gets or sets the Time.
            /// </summary>
            public TimeSpan Time { get; set; }
        }
    }
}
