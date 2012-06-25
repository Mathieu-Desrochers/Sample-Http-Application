
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SampleHttpApplication.DataAccessComponents.Tests.Session
{
    /// <summary>
    /// Represents the Session test table.
    /// </summary>
    public static class SessionTestTable
    {
        /// <summary>
        /// Asserts the presence of the specified row. 
        /// </summary>
        public static void AssertPresence(int sessionID, string sessionCode, string name, DateTime startDate)
        {
            // Open the data reader.
            string commandText = String.Format("SELECT * FROM [Session] WHERE [SessionID] = {0};", sessionID);
            using (SqlDataReader sqlDataReader = TestDatabase.ExecuteReader(commandText))
            {
                // Assert the presence of the row.
                Assert.IsTrue(sqlDataReader.Read());
                Assert.AreEqual(sessionID, (int)sqlDataReader["SessionID"]);
                Assert.AreEqual(sessionCode, (string)sqlDataReader["SessionCode"]);
                Assert.AreEqual(name, (string)sqlDataReader["Name"]);
                Assert.AreEqual(startDate, (DateTime)sqlDataReader["StartDate"]);

                // Assert there is only one row.
                Assert.IsFalse(sqlDataReader.Read());
            }
        }

        /// <summary>
        /// Asserts the absence of the specified row.
        /// </summary>
        public static void AssertAbsence(int sessionID)
        {
            // Open the data reader.
            string commandText = String.Format("SELECT * FROM [Session] WHERE [SessionID] = {0};", sessionID);
            using (SqlDataReader sqlDataReader = TestDatabase.ExecuteReader(commandText))
            {
                // Assert the absence of the row.
                Assert.IsFalse(sqlDataReader.Read());
            }
        }

        /// <summary>
        /// Inserts the specified row.
        /// </summary>
        public static int InsertWithValues(string sessionCode, string name, DateTime startDate)
        {
            // Insert the row.
            string commandText = String.Format("INSERT INTO [Session] VALUES('{0}', '{1}', '{2:yyyy-MM-dd hh:mm:ss}'); SELECT CAST(SCOPE_IDENTITY() AS INT);", sessionCode, name, startDate);
            int sessionID = TestDatabase.ExecuteScalar<int>(commandText);

            // Return the generated ID.
            return sessionID;
        }

        /// <summary>
        /// Inserts a placeholder row.
        /// </summary>
        public static int InsertPlaceholder(string sessionCode = default(string), string name = default(string), DateTime startDate = default(DateTime))
        {
            // Provide a value for all the columns.
            if (sessionCode == default(string)) { sessionCode = Guid.NewGuid().ToString(); }
            if (name == default(string)) { name = String.Empty; }
            if (startDate == default(DateTime)) { startDate = new DateTime(2000, 1, 1); }

            // Insert the row.
            int sessionID = InsertWithValues(sessionCode, name, startDate);

            // Return the generated ID.
            return sessionID;
        }
    }
}
