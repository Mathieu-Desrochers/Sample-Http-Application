
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;

namespace SampleHttpApplication.BusinessLogicComponents.Code.Scheduling
{
    /// <summary>
    /// Represents the NewCourseSchedule operation data.
    /// </summary>
    public class NewCourseScheduleOperationData
    {
        /// <summary>
        /// The data rows.
        /// </summary>
        public SessionDataRow SessionDataRow;
        public CourseScheduleDataRow CourseScheduleDataRow;
    }
}
