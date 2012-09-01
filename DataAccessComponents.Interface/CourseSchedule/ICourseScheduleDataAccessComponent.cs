
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule
{
    /// <summary>
    /// Represents the CourseSchedule data access component.
    /// </summary>
    public interface ICourseScheduleDataAccessComponent
    {
        /// <summary>
        /// Creates the specified CourseSchedule data row.
        /// </summary>
        Task Create(IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow);

        /// <summary>
        /// Reads the single CourseSchedule data row matching the specified CourseScheduleID.
        /// </summary>
        Task<CourseScheduleDataRow> ReadByCourseScheduleID(IDatabaseConnection databaseConnection, int courseScheduleID);

        /// <summary>
        /// Reads the single CourseSchedule data row matching the specified CourseScheduleCode.
        /// </summary>
        Task<CourseScheduleDataRow> ReadByCourseScheduleCode(IDatabaseConnection databaseConnection, string courseScheduleCode);

        /// <summary>
        /// Updates the specified CourseSchedule data row.
        /// </summary>
        Task Update(IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow);

        /// <summary>
        /// Deletes the specified CourseSchedule data row.
        /// </summary>
        Task Delete(IDatabaseConnection databaseConnection, CourseScheduleDataRow courseScheduleDataRow);
    }
}
