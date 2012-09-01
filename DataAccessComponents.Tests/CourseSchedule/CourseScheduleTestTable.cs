
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using SampleHttpApplication.DataAccessComponents.Tests.Session;

namespace SampleHttpApplication.DataAccessComponents.Tests.CourseSchedule
{
    /// <summary>
    /// Represents the CourseSchedule test table.
    /// </summary>
    public static class CourseScheduleTestTable
    {
        /// <summary>
        /// Asserts the presence of the specified row. 
        /// </summary>
        public static void AssertPresence(int courseScheduleID, string courseScheduleCode, int sessionID, int dayOfWeek, TimeSpan time)
        {
            // Open the data reader.
            string commandText = String.Format("SELECT * FROM [CourseSchedule] WHERE [CourseScheduleID] = {0};", courseScheduleID);
            using (SqlDataReader sqlDataReader = TestDatabase.ExecuteReader(commandText))
            {
                // Assert the presence of the row.
                Assert.IsTrue(sqlDataReader.Read());
                Assert.AreEqual(courseScheduleID, (int)sqlDataReader["CourseScheduleID"]);
                Assert.AreEqual(courseScheduleCode, (string)sqlDataReader["CourseScheduleCode"]);
                Assert.AreEqual(sessionID, (int)sqlDataReader["SessionID"]);
                Assert.AreEqual(dayOfWeek, (int)sqlDataReader["DayOfWeek"]);
                Assert.AreEqual(time, (TimeSpan)sqlDataReader["Time"]);

                // Assert there is only one row.
                Assert.IsFalse(sqlDataReader.Read());
            }
        }

        /// <summary>
        /// Asserts the absence of the specified row.
        /// </summary>
        public static void AssertAbsence(int courseScheduleID)
        {
            // Open the data reader.
            string commandText = String.Format("SELECT * FROM [CourseSchedule] WHERE [CourseScheduleID] = {0};", courseScheduleID);
            using (SqlDataReader sqlDataReader = TestDatabase.ExecuteReader(commandText))
            {
                // Assert the absence of the row.
                Assert.IsFalse(sqlDataReader.Read());
            }
        }

        /// <summary>
        /// Inserts the specified row.
        /// </summary>
        public static int InsertWithValues(string courseScheduleCode, int sessionID, int dayOfWeek, TimeSpan time)
        {
            // Insert the row.
            string commandText = String.Format("INSERT INTO [CourseSchedule] VALUES('{0}', {1}, {2}, '{3:hh\\:mm\\:ss}'); SELECT CAST(SCOPE_IDENTITY() AS INT);", courseScheduleCode, sessionID, dayOfWeek, time);
            int courseScheduleID = TestDatabase.ExecuteScalar<int>(commandText);

            // Return the generated ID.
            return courseScheduleID;
        }

        /// <summary>
        /// Inserts a placeholder row.
        /// </summary>
        public static int InsertPlaceholder(string courseScheduleCode = default(string), int sessionID = default(int), int dayOfWeek = default(int), TimeSpan time = default(TimeSpan))
        {
            // Provide a value for all the columns.
            if (courseScheduleCode == default(string)) { courseScheduleCode = Guid.NewGuid().ToString(); }
            if (sessionID == default(int)) { sessionID = SessionTestTable.InsertPlaceholder(); }
            if (dayOfWeek == default(int)) { dayOfWeek = 0; }
            if (time == default(TimeSpan)) { time = new TimeSpan(10, 1, 1); }

            // Insert the row.
            int courseScheduleID = InsertWithValues(courseScheduleCode, sessionID, dayOfWeek, time);

            // Return the generated ID.
            return courseScheduleID;
        }
    }
}
