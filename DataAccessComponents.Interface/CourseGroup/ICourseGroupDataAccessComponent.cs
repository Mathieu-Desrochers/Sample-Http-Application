
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.CourseGroup
{
    /// <summary>
    /// Represents the CourseGroup data access component.
    /// </summary>
    public interface ICourseGroupDataAccessComponent
    {
        /// <summary>
        /// Creates the specified CourseGroup data row.
        /// </summary>
        Task Create(IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow);

        /// <summary>
        /// Reads the single CourseGroup data row matching the specified CourseGroupID.
        /// </summary>
        Task<CourseGroupDataRow> ReadByCourseGroupID(IDatabaseConnection databaseConnection, int courseGroupID);

        /// <summary>
        /// Reads the single CourseGroup data row matching the specified CourseGroupCode.
        /// </summary>
        Task<CourseGroupDataRow> ReadByCourseGroupCode(IDatabaseConnection databaseConnection, string courseGroupCode);

        /// <summary>
        /// Reads the multiple CourseGroup data rows matching the specified CourseScheduleID.
        /// </summary>
        Task<CourseGroupDataRow[]> ReadByCourseScheduleID(IDatabaseConnection databaseConnection, int courseScheduleID);

        /// <summary>
        /// Updates the specified CourseGroup data row.
        /// </summary>
        Task Update(IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow);

        /// <summary>
        /// Deletes the specified CourseGroup data row.
        /// </summary>
        Task Delete(IDatabaseConnection databaseConnection, CourseGroupDataRow courseGroupDataRow);
    }
}
