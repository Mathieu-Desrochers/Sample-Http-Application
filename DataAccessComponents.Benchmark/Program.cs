
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SampleHttpApplication.DataAccessComponents.Benchmark
{
    public class Program
    {
        /// <summary>
        /// Performs a sequence of blocking database operations.
        /// </summary>
        private static void BlockingDatabaseOperations()
        {
            // Build the database connection.
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleHttpApplication"].ConnectionString))
            {
                // Open the database connection.
                sqlConnection.Open();

                // Insert the database row.
                SqlCommand insertSqlCommand = new SqlCommand("INSERT INTO [Session] VALUES('" + Guid.NewGuid() + "', 'Session Alpha', '2012-06-27 10:05:45'); SELECT CAST(SCOPE_IDENTITY() AS INT);", sqlConnection);
                int sessionID = (int)insertSqlCommand.ExecuteScalar();

                // Select the database row.
                SqlCommand selectSqlCommand = new SqlCommand("SELECT * FROM [Session] WHERE [SessionID] = " + sessionID, sqlConnection);
                SqlDataReader sqlDataReader = selectSqlCommand.ExecuteReader();
                sqlDataReader.Read();
                sqlDataReader.Close();

                // Update the database row.
                SqlCommand updateSqlCommand = new SqlCommand("UPDATE [Session] SET [SessionCode] = '" + Guid.NewGuid() + "', [Name] = 'Session Beta', [StartDate] = '2013-07-28 11:06:46' WHERE [SessionID] = " + sessionID, sqlConnection);
                updateSqlCommand.ExecuteNonQuery();

                // Delete the database row.
                SqlCommand deleteSqlCommand = new SqlCommand("DELETE FROM [Session] WHERE [SessionID] = " + sessionID, sqlConnection);
                updateSqlCommand.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Performs a sequence of non blocking database operations.
        /// </summary>
        private async static Task NonBlockingDatabaseOperations()
        {
            // Build the database connection.
            using (SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["SampleHttpApplication"].ConnectionString))
            {
                // Open the database connection.
                await sqlConnection.OpenAsync();

                // Insert the database row.
                SqlCommand insertSqlCommand = new SqlCommand("INSERT INTO [Session] VALUES('" + Guid.NewGuid() + "', 'Session Alpha', '2012-06-27 10:05:45'); SELECT CAST(SCOPE_IDENTITY() AS INT);", sqlConnection);
                int sessionID = (int)await insertSqlCommand.ExecuteScalarAsync();

                // Select the database row.
                SqlCommand selectSqlCommand = new SqlCommand("SELECT * FROM [Session] WHERE [SessionID] = " + sessionID, sqlConnection);
                SqlDataReader sqlDataReader = await selectSqlCommand.ExecuteReaderAsync();
                await sqlDataReader.ReadAsync();
                sqlDataReader.Close();

                // Update the database row.
                SqlCommand updateSqlCommand = new SqlCommand("UPDATE [Session] SET [SessionCode] = '" + Guid.NewGuid() + "', [Name] = 'Session Beta', [StartDate] = '2013-07-28 11:06:46' WHERE [SessionID] = " + sessionID, sqlConnection);
                await updateSqlCommand.ExecuteNonQueryAsync();

                // Delete the database row.
                SqlCommand deleteSqlCommand = new SqlCommand("DELETE FROM [Session] WHERE [SessionID] = " + sessionID, sqlConnection);
                await updateSqlCommand.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// Performs the benchmark on the specified database operations.
        /// </summary>
        private static void BenchmarkDatabaseOperations(string strategyName, Task[] databaseOperations)
        {
            // Start timing the database operations.
            Stopwatch stopwatch = Stopwatch.StartNew();
            TimeSpan initialTotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;

            // Wait for the database operations to complete.
            Task.WaitAll(databaseOperations);

            // Stop timing the database operations.
            TimeSpan finalTotalProcessorTime = Process.GetCurrentProcess().TotalProcessorTime;
            stopwatch.Stop();

            // Calculate the CPU utilization.
            double duration = stopwatch.Elapsed.TotalMilliseconds;
            double totalProcessorTime = finalTotalProcessorTime.TotalMilliseconds - initialTotalProcessorTime.TotalMilliseconds;
            double cpuUtilization = ((totalProcessorTime / duration) * 100) / Environment.ProcessorCount;

            // Display the benchmark results.
            Console.WriteLine(strategyName);
            Console.WriteLine("Duration: " + (int)duration + " milliseconds");
            Console.WriteLine("CPU utilization: " + (int)cpuUtilization + "%");
        }

        /// <summary>
        /// Runs when the program is started.
        /// </summary>
        public static void Main(string[] args)
        {
            // Prepare the blocking database operations.
            Task[] blockingDatabaseOperations = new Task[1000];
            for (int i = 0; i < 1000; i++)
            {
                blockingDatabaseOperations[i] = Task.Run(() => { BlockingDatabaseOperations(); });
            }

            // Benchmark the blocking database operations.
            BenchmarkDatabaseOperations("Hang on to the current thread", blockingDatabaseOperations);
            Console.WriteLine();

            // Prepare the non blocking database operations.
            Task[] nonBlockingDatabaseOperations = new Task[1000];
            for (int i = 0; i < 1000; i++)
            {
                nonBlockingDatabaseOperations[i] = NonBlockingDatabaseOperations();
            }

            // Benchmark the non blocking database operations.
            BenchmarkDatabaseOperations("Resume on another thread", nonBlockingDatabaseOperations);
            Console.ReadLine();
        }
    }
}
