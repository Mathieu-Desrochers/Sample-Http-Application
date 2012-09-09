﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.BusinessLogicComponents.Interface.Scheduling.NewCourseSchedule
{
    /// <summary>
    /// Represents the NewCourseSchedule business response.
    /// </summary>
    public class NewCourseScheduleBusinessResponse
    {
        /// <summary>
        /// The new course schedule.
        /// </summary>
        public CourseScheduleBusinessResponseElement CourseSchedule { get; set; }
        public class CourseScheduleBusinessResponseElement
        {
            /// <summary>
            /// Gets or sets the CourseScheduleCode.
            /// </summary>
            public string CourseScheduleCode { get; set; }
        }
    }
}
