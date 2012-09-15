
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using SampleHttpApplication.DataAccessComponents.Code;
using SampleHttpApplication.DataAccessComponents.Code.CourseGroup;
using SampleHttpApplication.DataAccessComponents.Interface.CourseGroup;
using SampleHttpApplication.DataAccessComponents.Tests.CourseSchedule;

namespace SampleHttpApplication.DataAccessComponents.Tests.CourseGroup
{
    /// <summary>
    /// Tests for the CourseGroup data access component.
    /// </summary>
    [TestClass]
    public class CourseGroupDataAccessComponentTests
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
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupCode = "5s1cgndj6e5x0uvz";
            courseGroupDataRow.CourseScheduleID = courseScheduleID;
            courseGroupDataRow.PlacesCount = 1;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Create the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataAccessComponent.Create(databaseConnection, courseGroupDataRow).Wait();
            }

            // Validate the CourseGroupID was generated.
            Assert.AreNotEqual(0, courseGroupDataRow.CourseGroupID);

            // Validate the CourseGroup data row was inserted in the database.
            CourseGroupTestTable.AssertPresence(
                courseGroupDataRow.CourseGroupID,
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);
        }

        /// <summary>
        /// Tests the Create method.
        /// </summary>
        [TestMethod]
        public void Create_ShouldThrowException_GivenInvalidCourseScheduleID()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupCode = "5s1cgndj6e5x0uvz";
            courseGroupDataRow.CourseScheduleID = -1;
            courseGroupDataRow.PlacesCount = 1;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Create the CourseGroup data row.
                    CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                    courseGroupDataAccessComponent.Create(databaseConnection, courseGroupDataRow).Wait();

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
        public void Create_ShouldThrowException_GivenDuplicateCourseGroupCode()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the duplicate CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertPlaceholder(courseGroupCode: "5s1cgndj6e5x0uvz");

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupCode = "5s1cgndj6e5x0uvz";
            courseGroupDataRow.CourseScheduleID = courseScheduleID;
            courseGroupDataRow.PlacesCount = 1;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Create the CourseGroup data row.
                    CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                    courseGroupDataAccessComponent.Create(databaseConnection, courseGroupDataRow).Wait();

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
        /// Tests the ReadByCourseGroupID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseGroupID_ShouldReturnDataRow()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);

            // Build the database connection.
            CourseGroupDataRow courseGroupDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRow = courseGroupDataAccessComponent.ReadByCourseGroupID(databaseConnection, courseGroupID).Result;
            }

            // Validate the CourseGroup data row.
            Assert.IsNotNull(courseGroupDataRow);
            Assert.AreEqual(courseGroupID, courseGroupDataRow.CourseGroupID);
            Assert.AreEqual("5s1cgndj6e5x0uvz", courseGroupDataRow.CourseGroupCode);
            Assert.AreEqual(courseScheduleID, courseGroupDataRow.CourseScheduleID);
            Assert.AreEqual(1, courseGroupDataRow.PlacesCount);
        }

        /// <summary>
        /// Tests the ReadByCourseGroupID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseGroupID_ShouldReturnNull()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);

            // Build the database connection.
            CourseGroupDataRow courseGroupDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRow = courseGroupDataAccessComponent.ReadByCourseGroupID(databaseConnection, -1).Result;
            }

            // Validate the CourseGroup data row.
            Assert.IsNull(courseGroupDataRow);
        }

        /// <summary>
        /// Tests the ReadByCourseGroupCode method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseGroupCode_ShouldReturnDataRow()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);

            // Build the database connection.
            CourseGroupDataRow courseGroupDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRow = courseGroupDataAccessComponent.ReadByCourseGroupCode(databaseConnection, "5s1cgndj6e5x0uvz").Result;
            }

            // Validate the CourseGroup data row.
            Assert.IsNotNull(courseGroupDataRow);
            Assert.AreEqual(courseGroupID, courseGroupDataRow.CourseGroupID);
            Assert.AreEqual("5s1cgndj6e5x0uvz", courseGroupDataRow.CourseGroupCode);
            Assert.AreEqual(courseScheduleID, courseGroupDataRow.CourseScheduleID);
            Assert.AreEqual(1, courseGroupDataRow.PlacesCount);
        }

        /// <summary>
        /// Tests the ReadByCourseGroupCode method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseGroupCode_ShouldReturnNull()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);

            // Build the database connection.
            CourseGroupDataRow courseGroupDataRow = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRow = courseGroupDataAccessComponent.ReadByCourseGroupCode(databaseConnection, "").Result;
            }

            // Validate the CourseGroup data row.
            Assert.IsNull(courseGroupDataRow);
        }

        /// <summary>
        /// Tests the ReadByCourseScheduleID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleID_ShouldReturnZeroDataRows()
        {
            // Insert the first master data rows in the database.
            int firstCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the first CourseGroup data row in the database.
            int firstCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                firstCourseScheduleID,
                1);

            // Insert the second master data rows in the database.
            int secondCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the second CourseGroup data row in the database.
            int secondCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "78zcn25ynkaz50ef",
                secondCourseScheduleID,
                2);

            // Insert the third master data rows in the database.
            int thirdCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the third CourseGroup data row in the database.
            int thirdCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "q5692qwy70qde9uv",
                thirdCourseScheduleID,
                3);

            // Build the database connection.
            CourseGroupDataRow[] courseGroupDataRows = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data rows.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRows = courseGroupDataAccessComponent.ReadByCourseScheduleID(databaseConnection, -1).Result;
            }

            // Validate the CourseGroup data rows.
            Assert.IsNotNull(courseGroupDataRows);
            Assert.AreEqual(0, courseGroupDataRows.Length);
        }

        /// <summary>
        /// Tests the ReadByCourseScheduleID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleID_ShouldReturnOneDataRow()
        {
            // Insert the first master data rows in the database.
            int firstCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the first CourseGroup data row in the database.
            int firstCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                firstCourseScheduleID,
                1);

            // Insert the second master data rows in the database.
            int secondCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the second CourseGroup data row in the database.
            int secondCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "78zcn25ynkaz50ef",
                secondCourseScheduleID,
                2);

            // Insert the third master data rows in the database.
            int thirdCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the third CourseGroup data row in the database.
            int thirdCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "q5692qwy70qde9uv",
                thirdCourseScheduleID,
                3);

            // Build the database connection.
            CourseGroupDataRow[] courseGroupDataRows = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data rows.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRows = courseGroupDataAccessComponent.ReadByCourseScheduleID(databaseConnection, firstCourseScheduleID).Result;
            }

            // Validate the CourseGroup data rows.
            Assert.IsNotNull(courseGroupDataRows);
            Assert.AreEqual(1, courseGroupDataRows.Length);

            // Validate the first CourseGroup data row.
            Assert.AreEqual(firstCourseGroupID, courseGroupDataRows[0].CourseGroupID);
            Assert.AreEqual("5s1cgndj6e5x0uvz", courseGroupDataRows[0].CourseGroupCode);
            Assert.AreEqual(firstCourseScheduleID, courseGroupDataRows[0].CourseScheduleID);
            Assert.AreEqual(1, courseGroupDataRows[0].PlacesCount);
        }

        /// <summary>
        /// Tests the ReadByCourseScheduleID method.
        /// </summary>
        [TestMethod]
        public void ReadByCourseScheduleID_ShouldReturnMultipleDataRows()
        {
            // Insert the master data rows in the database.
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the first CourseGroup data row in the database.
            int firstCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);

            // Insert the second CourseGroup data row in the database.
            int secondCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "78zcn25ynkaz50ef",
                courseScheduleID,
                2);

            // Insert the third CourseGroup data row in the database.
            int thirdCourseGroupID = CourseGroupTestTable.InsertWithValues(
                "q5692qwy70qde9uv",
                courseScheduleID,
                3);

            // Build the database connection.
            CourseGroupDataRow[] courseGroupDataRows = null;
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Read the CourseGroup data rows.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataRows = courseGroupDataAccessComponent.ReadByCourseScheduleID(databaseConnection, courseScheduleID).Result;
            }

            // Validate the CourseGroup data rows.
            Assert.IsNotNull(courseGroupDataRows);
            Assert.AreEqual(3, courseGroupDataRows.Length);

            // Validate the first CourseGroup data row.
            Assert.AreEqual(firstCourseGroupID, courseGroupDataRows[0].CourseGroupID);
            Assert.AreEqual("5s1cgndj6e5x0uvz", courseGroupDataRows[0].CourseGroupCode);
            Assert.AreEqual(courseScheduleID, courseGroupDataRows[0].CourseScheduleID);
            Assert.AreEqual(1, courseGroupDataRows[0].PlacesCount);

            // Validate the second CourseGroup data row.
            Assert.AreEqual(secondCourseGroupID, courseGroupDataRows[1].CourseGroupID);
            Assert.AreEqual("78zcn25ynkaz50ef", courseGroupDataRows[1].CourseGroupCode);
            Assert.AreEqual(courseScheduleID, courseGroupDataRows[1].CourseScheduleID);
            Assert.AreEqual(2, courseGroupDataRows[1].PlacesCount);

            // Validate the third CourseGroup data row.
            Assert.AreEqual(thirdCourseGroupID, courseGroupDataRows[2].CourseGroupID);
            Assert.AreEqual("q5692qwy70qde9uv", courseGroupDataRows[2].CourseGroupCode);
            Assert.AreEqual(courseScheduleID, courseGroupDataRows[2].CourseScheduleID);
            Assert.AreEqual(3, courseGroupDataRows[2].PlacesCount);
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldSucceed()
        {
            // Insert the first master data rows in the database.
            int firstCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the second master data rows in the database.
            int secondCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                firstCourseScheduleID,
                1);

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupID = courseGroupID;
            courseGroupDataRow.CourseGroupCode = "78zcn25ynkaz50ef";
            courseGroupDataRow.CourseScheduleID = secondCourseScheduleID;
            courseGroupDataRow.PlacesCount = 2;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Update the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataAccessComponent.Update(databaseConnection, courseGroupDataRow).Wait();
            }

            // Validate the CourseGroup data row was updated in the database.
            CourseGroupTestTable.AssertPresence(
                courseGroupID,
                "78zcn25ynkaz50ef",
                secondCourseScheduleID,
                2);
        }

        /// <summary>
        /// Tests the Update method.
        /// </summary>
        [TestMethod]
        public void Update_ShouldThrowException_GivenInvalidCourseScheduleID()
        {
            // Insert the first master data rows in the database.
            int firstCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the second master data rows in the database.
            int secondCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                firstCourseScheduleID,
                1);

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupID = courseGroupID;
            courseGroupDataRow.CourseGroupCode = "78zcn25ynkaz50ef";
            courseGroupDataRow.CourseScheduleID = -1;
            courseGroupDataRow.PlacesCount = 2;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Update the CourseGroup data row.
                    CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                    courseGroupDataAccessComponent.Update(databaseConnection, courseGroupDataRow).Wait();
                    
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
        public void Update_ShouldThrowException_GivenDuplicateCourseGroupCode()
        {
            // Insert the first master data rows in the database.
            int firstCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the second master data rows in the database.
            int secondCourseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                firstCourseScheduleID,
                1);

            // Insert the duplicate CourseGroup data row in the database.
            int duplicateCourseGroupID = CourseGroupTestTable.InsertPlaceholder(courseGroupCode: "78zcn25ynkaz50ef");

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupID = courseGroupID;
            courseGroupDataRow.CourseGroupCode = "78zcn25ynkaz50ef";
            courseGroupDataRow.CourseScheduleID = secondCourseScheduleID;
            courseGroupDataRow.PlacesCount = 2;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                try
                {
                    // Update the CourseGroup data row.
                    CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                    courseGroupDataAccessComponent.Update(databaseConnection, courseGroupDataRow).Wait();
                    
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
            int courseScheduleID = CourseScheduleTestTable.InsertPlaceholder();

            // Insert the CourseGroup data row in the database.
            int courseGroupID = CourseGroupTestTable.InsertWithValues(
                "5s1cgndj6e5x0uvz",
                courseScheduleID,
                1);

            // Build the CourseGroup data row.
            CourseGroupDataRow courseGroupDataRow = new CourseGroupDataRow();
            courseGroupDataRow.CourseGroupID = courseGroupID;
            courseGroupDataRow.CourseGroupCode = "5s1cgndj6e5x0uvz";
            courseGroupDataRow.CourseScheduleID = courseScheduleID;
            courseGroupDataRow.PlacesCount = 1;

            // Build the database connection.
            using (DatabaseConnection databaseConnection = new DatabaseConnection(TestDatabase.ConnectionString))
            {
                // Open the database connection.
                databaseConnection.Open().Wait();

                // Delete the CourseGroup data row.
                CourseGroupDataAccessComponent courseGroupDataAccessComponent = new CourseGroupDataAccessComponent();
                courseGroupDataAccessComponent.Delete(databaseConnection, courseGroupDataRow).Wait();
            }

            // Validate the CourseGroup data row was deleted in the database.
            CourseGroupTestTable.AssertAbsence(courseGroupID);
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
