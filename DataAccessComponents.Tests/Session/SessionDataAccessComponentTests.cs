
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleHttpApplication.DataAccessComponents.Code;
using SampleHttpApplication.DataAccessComponents.Code.Session;
using SampleHttpApplication.DataAccessComponents.Interface.Session;
using SampleHttpApplication.DataAccessComponents.Tests.CourseSchedule;

namespace SampleHttpApplication.DataAccessComponents.Tests.Session
{
    /// <summary>
    /// Tests for the Session data access component.
    /// </summary>
    [TestClass]
    public class SessionDataAccessComponentTests
    {
        /// <summary>
        /// The transaction scope.
        /// </summary>
        private TransactionScope transactionScope;

        /// <summary>
        /// Starts a transaction before every test.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            this.transactionScope = new TransactionScope();
        }

        /// <summary>
        /// Tests the Create method.
        /// </summary>
        [TestMethod]
        public void Create_ShouldSucceed()
        {
            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionCode = "6dk61ufcuzp3f7vs";
            sessionDataRow.Name = "Session Alpha";
            sessionDataRow.StartDate = new DateTime(2001, 1, 1);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Create the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataAccessComponent.Create(databaseConnection, sessionDataRow).Wait();
            }

            // Validate the SessionID was generated.
            Assert.AreNotEqual(0, sessionDataRow.SessionID);

            // Validate the Session data row was inserted in the database.
            SessionTestTable.AssertPresence(
                sessionDataRow.SessionID,
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));
        }

        /// <summary>
        /// Tests the Create method.
        /// </summary>
        [TestMethod]
        public void Create_ShouldThrowException_GivenDuplicateSessionCode()
        {
            // Insert the duplicate Session data row in the database.
            int sessionID = SessionTestTable.InsertPlaceholder(sessionCode: "6dk61ufcuzp3f7vs");

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionCode = "6dk61ufcuzp3f7vs";
            sessionDataRow.Name = "Session Alpha";
            sessionDataRow.StartDate = new DateTime(2001, 1, 1);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Create the Session data row.
                    SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                    sessionDataAccessComponent.Create(databaseConnection, sessionDataRow).Wait();

                    // Validate an exception was thrown.
                    Assert.Fail();
                }
                catch (AggregateException ex)
                {
                    // Validate an SQL exception was thrown.
                    Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(SqlException));
                }
            }
        }

        /// <summary>
        /// Tests the ReadBySessionID method.
        /// </summary>
        [TestMethod]
        public void ReadBySessionID_ShouldReturnDataRow()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the database connection.
            SessionDataRow sessionDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRow = sessionDataAccessComponent.ReadBySessionID(databaseConnection, sessionID).Result;
            }

            // Validate the Session data row.
            Assert.IsNotNull(sessionDataRow);
            Assert.AreEqual(sessionID, sessionDataRow.SessionID);
            Assert.AreEqual("6dk61ufcuzp3f7vs", sessionDataRow.SessionCode);
            Assert.AreEqual("Session Alpha", sessionDataRow.Name);
            Assert.AreEqual(new DateTime(2001, 1, 1), sessionDataRow.StartDate);
        }

        /// <summary>
        /// Tests the ReadBySessionID method.
        /// </summary>
        [TestMethod]
        public void ReadBySessionID_ShouldReturnNull()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the database connection.
            SessionDataRow sessionDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRow = sessionDataAccessComponent.ReadBySessionID(databaseConnection, -1).Result;
            }

            // Validate the Session data row.
            Assert.IsNull(sessionDataRow);
        }

        /// <summary>
        /// Tests the ReadBySessionCode method.
        /// </summary>
        [TestMethod]
        public void ReadBySessionCode_ShouldReturnDataRow()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the database connection.
            SessionDataRow sessionDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRow = sessionDataAccessComponent.ReadBySessionCode(databaseConnection, "6dk61ufcuzp3f7vs").Result;
            }

            // Validate the Session data row.
            Assert.IsNotNull(sessionDataRow);
            Assert.AreEqual(sessionID, sessionDataRow.SessionID);
            Assert.AreEqual("6dk61ufcuzp3f7vs", sessionDataRow.SessionCode);
            Assert.AreEqual("Session Alpha", sessionDataRow.Name);
            Assert.AreEqual(new DateTime(2001, 1, 1), sessionDataRow.StartDate);
        }

        /// <summary>
        /// Tests the ReadBySessionCode method.
        /// </summary>
        [TestMethod]
        public void ReadBySessionCode_ShouldReturnNull()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the database connection.
            SessionDataRow sessionDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRow = sessionDataAccessComponent.ReadBySessionCode(databaseConnection, "").Result;
            }

            // Validate the Session data row.
            Assert.IsNull(sessionDataRow);
        }

        /// <summary>
        /// Tests the ReadAll method.
        /// </summary>
        [TestMethod]
        public void ReadAll_ShouldReturnZeroDataRows()
        {
            // Build the database connection.
            SessionDataRow[] sessionDataRows = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data rows.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRows = sessionDataAccessComponent.ReadAll(databaseConnection).Result;
            }

            // Validate the Session data rows.
            Assert.IsNotNull(sessionDataRows);
            Assert.AreEqual(0, sessionDataRows.Length);
        }

        /// <summary>
        /// Tests the ReadAll method.
        /// </summary>
        [TestMethod]
        public void ReadAll_ShouldReturnOneDataRow()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the database connection.
            SessionDataRow[] sessionDataRows = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data rows.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRows = sessionDataAccessComponent.ReadAll(databaseConnection).Result;
            }

            // Validate the Session data rows.
            Assert.IsNotNull(sessionDataRows);
            Assert.AreEqual(1, sessionDataRows.Length);

            // Validate the first Session data row.
            Assert.AreEqual(sessionID, sessionDataRows[0].SessionID);
            Assert.AreEqual("6dk61ufcuzp3f7vs", sessionDataRows[0].SessionCode);
            Assert.AreEqual("Session Alpha", sessionDataRows[0].Name);
            Assert.AreEqual(new DateTime(2001, 1, 1), sessionDataRows[0].StartDate);
        }

        /// <summary>
        /// Tests the ReadAll method.
        /// </summary>
        [TestMethod]
        public void ReadAll_ShouldReturnMultipleDataRows()
        {
            // Insert the first Session data row in the database.
            int firstSessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Insert the second Session data row in the database.
            int secondSessionID = SessionTestTable.InsertWithValues(
                "n3p4y556gt9f17hw",
                "Session Bravo",
                new DateTime(2002, 2, 2));

            // Insert the third Session data row in the database.
            int thirdSessionID = SessionTestTable.InsertWithValues(
                "x36s2tccz8yxp1hq",
                "Session Charlie",
                new DateTime(2003, 3, 3));

            // Build the database connection.
            SessionDataRow[] sessionDataRows = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data rows.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRows = sessionDataAccessComponent.ReadAll(databaseConnection).Result;
            }

            // Validate the Session data rows.
            Assert.IsNotNull(sessionDataRows);
            Assert.AreEqual(3, sessionDataRows.Length);

            // Validate the first Session data row.
            Assert.AreEqual(firstSessionID, sessionDataRows[0].SessionID);
            Assert.AreEqual("6dk61ufcuzp3f7vs", sessionDataRows[0].SessionCode);
            Assert.AreEqual("Session Alpha", sessionDataRows[0].Name);
            Assert.AreEqual(new DateTime(2001, 1, 1), sessionDataRows[0].StartDate);

            // Validate the second Session data row.
            Assert.AreEqual(secondSessionID, sessionDataRows[1].SessionID);
            Assert.AreEqual("n3p4y556gt9f17hw", sessionDataRows[1].SessionCode);
            Assert.AreEqual("Session Bravo", sessionDataRows[1].Name);
            Assert.AreEqual(new DateTime(2002, 2, 2), sessionDataRows[1].StartDate);

            // Validate the third Session data row.
            Assert.AreEqual(thirdSessionID, sessionDataRows[2].SessionID);
            Assert.AreEqual("x36s2tccz8yxp1hq", sessionDataRows[2].SessionCode);
            Assert.AreEqual("Session Charlie", sessionDataRows[2].Name);
            Assert.AreEqual(new DateTime(2003, 3, 3), sessionDataRows[2].StartDate);
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldSucceed()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "n3p4y556gt9f17hw";
            sessionDataRow.Name = "Session Bravo";
            sessionDataRow.StartDate = new DateTime(2002, 2, 2);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Update the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataAccessComponent.Update(databaseConnection, sessionDataRow).Wait();
            }

            // Validate the Session data row was updated in the database.
            SessionTestTable.AssertPresence(
                sessionID,
                "n3p4y556gt9f17hw",
                "Session Bravo",
                new DateTime(2002, 2, 2));
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldThrowException_GivenDuplicateSessionCode()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Insert the duplicate Session data row in the database.
            int duplicateSessionID = SessionTestTable.InsertPlaceholder(sessionCode: "n3p4y556gt9f17hw");

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "n3p4y556gt9f17hw";
            sessionDataRow.Name = "Session Bravo";
            sessionDataRow.StartDate = new DateTime(2002, 2, 2);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Update the Session data row.
                    SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                    sessionDataAccessComponent.Update(databaseConnection, sessionDataRow).Wait();
                    
                    // Validate an exception was thrown.
                    Assert.Fail();
                }
                catch (AggregateException ex)
                {
                    // Validate an SQL exception was thrown.
                    Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(SqlException));
                }
            }
        }

        /// <summary>
        /// Tests the Delete method.
        /// </summary>
        [TestMethod]
        public void Delete_ShouldSucceed()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "6dk61ufcuzp3f7vs";
            sessionDataRow.Name = "Session Alpha";
            sessionDataRow.StartDate = new DateTime(2001, 1, 1);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Delete the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataAccessComponent.Delete(databaseConnection, sessionDataRow).Wait();
            }

            // Validate the Session data row was deleted in the database.
            SessionTestTable.AssertAbsence(sessionID);
        }

        /// <summary>
        /// Tests the Delete method.
        /// </summary>
        [TestMethod]
        public void Delete_ShouldThrowException_GivenCourseScheduleDetails()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "6dk61ufcuzp3f7vs",
                "Session Alpha",
                new DateTime(2001, 1, 1));

            // Insert the details CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder(sessionID: sessionID);

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "6dk61ufcuzp3f7vs";
            sessionDataRow.Name = "Session Alpha";
            sessionDataRow.StartDate = new DateTime(2001, 1, 1);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Delete the Session data row.
                    SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                    sessionDataAccessComponent.Delete(databaseConnection, sessionDataRow).Wait();

                    // Validate an exception was thrown.
                    Assert.Fail();
                }
                catch (AggregateException ex)
                {
                    // Validate an SQL exception was thrown.
                    Assert.IsInstanceOfType(ex.InnerExceptions[0], typeof(SqlException));
                }
            }
        }

        /// <summary>
        /// Rollbacks the transaction after every test.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }
    }
}
