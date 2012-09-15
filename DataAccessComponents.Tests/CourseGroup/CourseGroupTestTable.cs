
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleHttpApplication.DataAccessComponents.Tests.CourseSchedule;

namespace SampleHttpApplication.DataAccessComponents.Tests.CourseGroup
{
    /// <summary>
    /// Represents the CourseGroup test table.
    /// </summary>
    public static class CourseGroupTestTable
    {
        /// <summary>
        /// Asserts the presence of the specified row. 
        /// </summary>
        public static void AssertPresence(int courseGroupID, string courseGroupCode, int courseScheduleID, int placesCount)
        {
            // Open the data reader.
            string commandText = String.Format("SELECT * FROM [CourseGroup] WHERE [CourseGroupID] = {0};", courseGroupID);
            using (SqlDataReader sqlDataReader = TestDatabase.ExecuteReader(commandText))
            {
                // Assert the presence of the row.
                Assert.IsTrue(sqlDataReader.Read());
                Assert.AreEqual(courseGroupID, (int)sqlDataReader["CourseGroupID"]);
                Assert.AreEqual(courseGroupCode, (string)sqlDataReader["CourseGroupCode"]);
                Assert.AreEqual(courseScheduleID, (int)sqlDataReader["CourseScheduleID"]);
                Assert.AreEqual(placesCount, (int)sqlDataReader["PlacesCount"]);

                // Assert there is only one row.
                Assert.IsFalse(sqlDataReader.Read());
            }
        }

        /// <summary>
        /// Asserts the absence of the specified row.
        /// </summary>
        public static void AssertAbsence(int courseGroupID)
        {
            // Open the data reader.
            string commandText = String.Format("SELECT * FROM [CourseGroup] WHERE [CourseGroupID] = {0};", courseGroupID);
            using (SqlDataReader sqlDataReader = TestDatabase.ExecuteReader(commandText))
            {
                // Assert the absence of the row.
                Assert.IsFalse(sqlDataReader.Read());
            }
        }

        /// <summary>
        /// Inserts the specified row.
        /// </summary>
        public static int InsertWithValues(string courseGroupCode, int courseScheduleID, int placesCount)
        {
            // Insert the row.
            string commandText = String.Format("INSERT INTO [CourseGroup] VALUES('{0}', {1}, {2}); SELECT CAST(SCOPE_IDENTITY() AS INT);", courseGroupCode, courseScheduleID, placesCount);
            int courseGroupID = TestDatabase.ExecuteScalar<int>(commandText);

            // Return the generated ID.
            return courseGroupID;
        }

        /// <summary>
        /// Inserts a placeholder row.
        /// </summary>
        public static int InsertPlaceholder(string courseGroupCode = default(string), int courseScheduleID = default(int), int placesCount = default(int))
        {
            // Provide a value for all the columns.
            if (courseGroupCode == default(string)) { courseGroupCode = Guid.NewGuid().ToString(); }
            if (courseScheduleID == default(int)) { courseScheduleID = CourseScheduleTestTable.InsertPlaceholder(); }
            if (placesCount == default(int)) { placesCount = 0; }

            // Insert the row.
            int courseGroupID = InsertWithValues(courseGroupCode, courseScheduleID, placesCount);

            // Return the generated ID.
            return courseGroupID;
        }
    }
}
