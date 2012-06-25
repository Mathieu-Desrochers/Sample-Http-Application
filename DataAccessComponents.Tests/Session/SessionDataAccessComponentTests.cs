
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
            sessionDataRow.SessionCode = "Session-Code-A";
            sessionDataRow.Name = "Name-A";
            sessionDataRow.StartDate = new DateTime(2001, 1, 1, 1, 1, 1);

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
                "Session-Code-A",
                "Name-A",
                new DateTime(2001, 1, 1, 1, 1, 1));
        }

        /// <summary>
        /// Tests the Create method.
        /// </summary>
        [TestMethod]
        public void Create_ShouldThrowException_GivenDuplicateSessionCode()
        {
            // Insert the duplicate Session data row in the database.
            int sessionID = SessionTestTable.InsertPlaceholder(sessionCode: "Session-Code-A");

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionCode = "Session-Code-A";
            sessionDataRow.Name = "Name-A";
            sessionDataRow.StartDate = new DateTime(2001, 1, 1, 1, 1, 1);

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
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

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
            Assert.AreEqual("Session-Code-A", sessionDataRow.SessionCode);
            Assert.AreEqual("Session A", sessionDataRow.Name);
            Assert.AreEqual(new DateTime(2001, 1, 1, 1, 1, 1), sessionDataRow.StartDate);
        }

        /// <summary>
        /// Tests the ReadBySessionID method.
        /// </summary>
        [TestMethod]
        public void ReadBySessionID_ShouldReturnNull()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

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
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

            // Build the database connection.
            SessionDataRow sessionDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRow = sessionDataAccessComponent.ReadBySessionCode(databaseConnection, "Session-Code-A").Result;
            }

            // Validate the Session data row.
            Assert.IsNotNull(sessionDataRow);
            Assert.AreEqual(sessionID, sessionDataRow.SessionID);
            Assert.AreEqual("Session-Code-A", sessionDataRow.SessionCode);
            Assert.AreEqual("Session A", sessionDataRow.Name);
            Assert.AreEqual(new DateTime(2001, 1, 1, 1, 1, 1), sessionDataRow.StartDate);
        }

        /// <summary>
        /// Tests the ReadBySessionCode method.
        /// </summary>
        [TestMethod]
        public void ReadBySessionCode_ShouldReturnNull()
        {
            // Insert the Session data row in the database.
            int SessionCode = SessionTestTable.InsertWithValues(
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

            // Build the database connection.
            SessionDataRow sessionDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the Session data row.
                SessionDataAccessComponent sessionDataAccessComponent = new SessionDataAccessComponent();
                sessionDataRow = sessionDataAccessComponent.ReadBySessionCode(databaseConnection, "Session-Code-B").Result;
            }

            // Validate the Session data row.
            Assert.IsNull(sessionDataRow);
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldSucceed()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "Session-Code-B";
            sessionDataRow.Name = "Session B";
            sessionDataRow.StartDate = new DateTime(2002, 2, 2, 2, 2, 2);

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
                sessionDataRow.SessionID,
                "Session-Code-B",
                "Session B",
                new DateTime(2002, 2, 2, 2, 2, 2));
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldThrowException_GivenDuplicateSessionCode()
        {
            // Insert the Session data row in the database.
            int sessionID = SessionTestTable.InsertWithValues(
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

            // Insert the duplicate Session data row in the database.
            int duplicateSessionID = SessionTestTable.InsertPlaceholder(sessionCode: "Session-Code-B");

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "Session-Code-B";
            sessionDataRow.Name = "Session B";
            sessionDataRow.StartDate = new DateTime(2002, 2, 2, 2, 2, 2);

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
                "Session-Code-A",
                "Session A",
                new DateTime(2001, 1, 1, 1, 1, 1));

            // Build the Session data row.
            SessionDataRow sessionDataRow = new SessionDataRow();
            sessionDataRow.SessionID = sessionID;
            sessionDataRow.SessionCode = "Session-Code-B";
            sessionDataRow.Name = "Session B";
            sessionDataRow.StartDate = new DateTime(2002, 2, 2, 2, 2, 2);

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
        /// Rollbacks the transaction after every test.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            this.transactionScope.Dispose();
        }
    }
}
