
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleHttpApplication.DataAccessComponents.Code;
using SampleHttpApplication.DataAccessComponents.Code.CourseSchedule;
using SampleHttpApplication.DataAccessComponents.Interface.CourseSchedule;
using SampleHttpApplication.DataAccessComponents.Tests.Session;

namespace SampleHttpApplication.DataAccessComponents.Tests.CourseSchedule
{
    /// <summary>
    /// Tests for the CourseSchedule data access component.
    /// </summary>
    [TestClass]
    public class CourseScheduleDataAccessComponentTests
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
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleCode = "zzcj32kpd6huzp1n";
            courseScheduleDataRow.SessionID = sessionID;
            courseScheduleDataRow.DayOfWeek = 1;
            courseScheduleDataRow.Time = new TimeSpan(9, 15, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Create the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataAccessComponent.Create(databaseConnection, courseScheduleDataRow).Wait();
            }

            // Validate the CourseScheduleID was generated.
            Assert.AreNotEqual(0, courseScheduleDataRow.CourseScheduleID);

            // Validate the CourseSchedule data row was inserted in the database.
            CourseScheduleTestTable.AssertPresence(
                courseScheduleDataRow.CourseScheduleID,
                "zzcj32kpd6huzp1n",
                sessionID,
                1,
                new TimeSpan(9, 15, 0));
        }

        /// <summary>
        /// Tests the Create method.
        /// </summary>
        [TestMethod]
        public void Create_ShouldThrowException_GivenInvalidSessionID()
        {
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleCode = "zzcj32kpd6huzp1n";
            courseScheduleDataRow.SessionID = -1;
            courseScheduleDataRow.DayOfWeek = 1;
            courseScheduleDataRow.Time = new TimeSpan(9, 15, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Create the CourseSchedule data row.
                    CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                    courseScheduleDataAccessComponent.Create(databaseConnection, courseScheduleDataRow).Wait();

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
        /// Tests the Create method.
        /// </summary>
        [TestMethod]
        public void Create_ShouldThrowException_GivenDuplicateCourseScheduleCode()
        {
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Insert the duplicate CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder(courseScheduleCode: "zzcj32kpd6huzp1n");

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleCode = "zzcj32kpd6huzp1n";
            courseScheduleDataRow.SessionID = sessionID;
            courseScheduleDataRow.DayOfWeek = 1;
            courseScheduleDataRow.Time = new TimeSpan(9, 15, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Create the CourseSchedule data row.
                    CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                    courseScheduleDataAccessComponent.Create(databaseConnection, courseScheduleDataRow).Wait();

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
        /// Tests the ReadByCourseScheduleID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleID_ShouldReturnDataRow()
        {
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                sessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the database connection.
            CourseScheduleDataRow courseScheduleDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataRow = courseScheduleDataAccessComponent.ReadByCourseScheduleID(databaseConnection, courseScheduleID).Result;
            }

            // Validate the CourseSchedule data row.
            Assert.IsNotNull(courseScheduleDataRow);
            Assert.AreEqual(courseScheduleID, courseScheduleDataRow.CourseScheduleID);
            Assert.AreEqual("zzcj32kpd6huzp1n", courseScheduleDataRow.CourseScheduleCode);
            Assert.AreEqual(sessionID, courseScheduleDataRow.SessionID);
            Assert.AreEqual(1, courseScheduleDataRow.DayOfWeek);
            Assert.AreEqual(new TimeSpan(9, 15, 0), courseScheduleDataRow.Time);
        }

        /// <summary>
        /// Tests the ReadByCourseScheduleID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleID_ShouldReturnNull()
        {
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                sessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the database connection.
            CourseScheduleDataRow courseScheduleDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataRow = courseScheduleDataAccessComponent.ReadByCourseScheduleID(databaseConnection, -1).Result;
            }

            // Validate the CourseSchedule data row.
            Assert.IsNull(courseScheduleDataRow);
        }

        /// <summary>
        /// Tests the ReadByCourseScheduleCode method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleCode_ShouldReturnDataRow()
        {
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                sessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the database connection.
            CourseScheduleDataRow courseScheduleDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataRow = courseScheduleDataAccessComponent.ReadByCourseScheduleCode(databaseConnection, "zzcj32kpd6huzp1n").Result;
            }

            // Validate the CourseSchedule data row.
            Assert.IsNotNull(courseScheduleDataRow);
            Assert.AreEqual(courseScheduleID, courseScheduleDataRow.CourseScheduleID);
            Assert.AreEqual("zzcj32kpd6huzp1n", courseScheduleDataRow.CourseScheduleCode);
            Assert.AreEqual(sessionID, courseScheduleDataRow.SessionID);
            Assert.AreEqual(1, courseScheduleDataRow.DayOfWeek);
            Assert.AreEqual(new TimeSpan(9, 15, 0), courseScheduleDataRow.Time);
        }

        /// <summary>
        /// Tests the ReadByCourseScheduleCode method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleCode_ShouldReturnNull()
        {
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                sessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the database connection.
            CourseScheduleDataRow courseScheduleDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataRow = courseScheduleDataAccessComponent.ReadByCourseScheduleCode(databaseConnection, "").Result;
            }

            // Validate the CourseSchedule data row.
            Assert.IsNull(courseScheduleDataRow);
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldSucceed()
        {
            // Insert the first master data rows in the database.
            int firstSessionID = SessionTestTable.InsertPlaceholder();

            // Insert the second master data rows in the database.
            int secondSessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                firstSessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleID = courseScheduleID;
            courseScheduleDataRow.CourseScheduleCode = "8zu96quwvk70ng1e";
            courseScheduleDataRow.SessionID = secondSessionID;
            courseScheduleDataRow.DayOfWeek = 2;
            courseScheduleDataRow.Time = new TimeSpan(10, 30, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Update the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataAccessComponent.Update(databaseConnection, courseScheduleDataRow).Wait();
            }

            // Validate the CourseSchedule data row was updated in the database.
            CourseScheduleTestTable.AssertPresence(
                courseScheduleID,
                "8zu96quwvk70ng1e",
                secondSessionID,
                2,
                new TimeSpan(10, 30, 0));
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldThrowException_GivenInvalidSessionID()
        {
            // Insert the first master data rows in the database.
            int firstSessionID = SessionTestTable.InsertPlaceholder();

            // Insert the second master data rows in the database.
            int secondSessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                firstSessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleID = courseScheduleID;
            courseScheduleDataRow.CourseScheduleCode = "8zu96quwvk70ng1e";
            courseScheduleDataRow.SessionID = -1;
            courseScheduleDataRow.DayOfWeek = 2;
            courseScheduleDataRow.Time = new TimeSpan(10, 30, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Update the CourseSchedule data row.
                    CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                    courseScheduleDataAccessComponent.Update(databaseConnection, courseScheduleDataRow).Wait();
                    
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
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldThrowException_GivenDuplicateCourseScheduleCode()
        {
            // Insert the first master data rows in the database.
            int firstSessionID = SessionTestTable.InsertPlaceholder();

            // Insert the second master data rows in the database.
            int secondSessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                firstSessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Insert the duplicate CourseSchedule data row in the database.
            int duplicateCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder(courseScheduleCode: "8zu96quwvk70ng1e");

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleID = courseScheduleID;
            courseScheduleDataRow.CourseScheduleCode = "8zu96quwvk70ng1e";
            courseScheduleDataRow.SessionID = secondSessionID;
            courseScheduleDataRow.DayOfWeek = 2;
            courseScheduleDataRow.Time = new TimeSpan(10, 30, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Update the CourseSchedule data row.
                    CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                    courseScheduleDataAccessComponent.Update(databaseConnection, courseScheduleDataRow).Wait();
                    
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
            // Insert the master data rows in the database.
            int sessionID = SessionTestTable.InsertPlaceholder();

            // Insert the CourseSchedule data row in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertWithValues(
                "zzcj32kpd6huzp1n",
                sessionID,
                1,
                new TimeSpan(9, 15, 0));

            // Build the CourseSchedule data row.
            CourseScheduleDataRow courseScheduleDataRow = new CourseScheduleDataRow();
            courseScheduleDataRow.CourseScheduleID = courseScheduleID;
            courseScheduleDataRow.CourseScheduleCode = "zzcj32kpd6huzp1n";
            courseScheduleDataRow.SessionID = sessionID;
            courseScheduleDataRow.DayOfWeek = 1;
            courseScheduleDataRow.Time = new TimeSpan(9, 15, 0);

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Delete the CourseSchedule data row.
                CourseScheduleDataAccessComponent courseScheduleDataAccessComponent = new CourseScheduleDataAccessComponent();
                courseScheduleDataAccessComponent.Delete(databaseConnection, courseScheduleDataRow).Wait();
            }

            // Validate the CourseSchedule data row was deleted in the database.
            CourseScheduleTestTable.AssertAbsence(courseScheduleID);
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
