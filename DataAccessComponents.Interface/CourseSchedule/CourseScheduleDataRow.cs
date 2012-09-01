
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule
{
    /// <summary>
    /// Represents the CourseSchedule data row.
    /// </summary>
    public class CourseScheduleDataRow
    {
        /// <summary>
        /// Gets or sets the CourseScheduleID.
        /// </summary>
        public int CourseScheduleID;

        /// <summary>
        /// Gets or sets the CourseScheduleCode.
        /// </summary>
        public string CourseScheduleCode;

        /// <summary>
        /// Gets or sets the SessionID.
        /// </summary>
        public int SessionID;

        /// <summary>
        /// Gets or sets the DayOfWeek.
        /// </summary>
        public int DayOfWeek;

        /// <summary>
        /// Gets or sets the Time.
        /// </summary>
        public TimeSpan Time;
    }
}
