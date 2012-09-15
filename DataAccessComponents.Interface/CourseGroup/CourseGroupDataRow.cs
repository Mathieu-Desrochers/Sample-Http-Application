
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.CourseGroup
{
    /// <summary>
    /// Represents the CourseGroup data row.
    /// </summary>
    public class CourseGroupDataRow
    {
        /// <summary>
        /// Gets or sets the CourseGroupID.
        /// </summary>
        public int CourseGroupID;

        /// <summary>
        /// Gets or sets the CourseGroupCode.
        /// </summary>
        public string CourseGroupCode;

        /// <summary>
        /// Gets or sets the CourseScheduleID.
        /// </summary>
        public int CourseScheduleID;

        /// <summary>
        /// Gets or sets the PlacesCount.
        /// </summary>
        public int PlacesCount;
    }
}
