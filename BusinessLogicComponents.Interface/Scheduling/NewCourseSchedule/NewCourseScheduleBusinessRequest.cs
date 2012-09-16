
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.Infrastructure.Code.DataAnnotations.ValidationAttributes;

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
        [Required]
        public SessionBusinessRequestElement Session { get; set; }
        public class SessionBusinessRequestElement
        {
            /// <summary>
            /// Gets or sets the SessionCode.
            /// </summary>
            [Required]
            [RegularExpression(@"[abcdefghjkmnpqrstuvwxyz1234567890]{16}")]
            public string SessionCode { get; set; }
        }

        /// <summary>
        /// The new course schedule.
        /// </summary>
        [Required]
        public CourseScheduleBusinessRequestElement CourseSchedule { get; set; }
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

            /// <summary>
            /// The new course groups.
            /// </summary>
            [Required]
            public CourseGroupBusinessRequestElement[] CourseGroups { get; set; }
            public class CourseGroupBusinessRequestElement
            {
                /// <summary>
                /// Gets or sets the PlacesCount.
                /// </summary>
                [Range(0, Int32.MaxValue)]
                public int PlacesCount { get; set; }
            }
        }
    }
}
