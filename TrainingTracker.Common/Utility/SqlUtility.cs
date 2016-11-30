using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace TrainingTracker.Common.Utility
{
    /// <summary>
    /// Utility class for handling sql operations.
    /// </summary>
    public class SqlUtility
    {
        /// <summary>
        /// Holds the connection string to be used for the class.
        /// </summary>
        public static string ConnectionString;

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlUtility(string connectionString)
        {
            ConnectionString = connectionString;
        }

        /// <summary>
        /// Creates Sql parameter.
        /// </summary>
        /// <param name="parameter">Paramenter name.</param>
        /// <param name="paramType">Data type of the parameter.</param>
        /// <param name="paramValue">Parameter value.</param>
        /// <returns>SqlParameter object.</returns>
        public static SqlParameter CreateParameter(string parameter, SqlDbType paramType, Object paramValue)
        {
            return new SqlParameter(parameter, paramType) { Value = paramValue };
        }

        /// <summary>
        /// Executes sql commands and returns data table.
        /// </summary>
        /// <param name="commandText">Stored procedure name.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="tableName">Name of the table for output.</param>
        /// <param name="sqlParams">SQL parameters.</param>
        /// <returns>Datatable based on the executed command.</returns>
        public static DataTable ExecuteAndGetTable(string commandText, CommandType commandType, string tableName, List<SqlParameter> sqlParams)
        {
            DataTable dt;

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (var dbCommand = new SqlCommand())
                {
                    dt = new DataTable(tableName);

                    dbCommand.CommandText = commandText;
                    dbCommand.CommandType = commandType;
                    dbCommand.Connection = conn;
                   
                    if (sqlParams != null)
                    {
                        foreach (var prm in sqlParams)
                        {
                            dbCommand.Parameters.Add(prm);
                        }
                    }

                    using (var dbAdapter = new SqlDataAdapter(dbCommand))
                    {
                        dbAdapter.Fill(dt);
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// Executes a non-result set query. Returns number of rows affected.
        /// </summary>
        /// <param name="sql">Sql command.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="sqlParams">Sql parameters.</param>
        /// <returns>Number of rows affected.</returns>
        public static Int64 ExecuteNonQuery(string sql, CommandType commandType, List<SqlParameter> sqlParams)
        {
            long rowsAffected;

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var dbCommand = new SqlCommand())
                {
                    dbCommand.Connection = conn;
                    dbCommand.CommandText = sql;
                    dbCommand.CommandType = commandType;

                    foreach (var prm in sqlParams)
                    {
                        dbCommand.Parameters.Add(prm);
                    }

                    rowsAffected = dbCommand.ExecuteNonQuery();
                }
            }

            return rowsAffected;
        }

        /// <summary>
        /// Executes a non-result set query. Returns number of rows affected.
        /// </summary>
        /// <param name="sql">Sql command.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="sqlParams">Sql parameters.</param>
        /// <returns>Number of rows affected.</returns>
        public static int ExecuteScalar(string sql, CommandType commandType, List<SqlParameter> sqlParams)
        {
            int id;

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var dbCommand = new SqlCommand())
                {
                    dbCommand.Connection = conn;
                    dbCommand.CommandText = sql;
                    dbCommand.CommandType = commandType;

                    foreach (var prm in sqlParams)
                    {
                        dbCommand.Parameters.Add(prm);
                    }
                    var returnId = dbCommand.ExecuteScalar();
                     Int32.TryParse(returnId.ToString(),out id);
                }
            }

            return id;
        }

        /// <summary>
        /// Executes command to return bytes.
        /// </summary>
        /// <param name="sql">Sql command.</param>
        /// <param name="commandType">Command type.</param>
        /// <param name="sqlParams">Sql parameters.</param>
        /// <returns>Byte array of the result.</returns>
        public static byte[] ExecuteReaderForBinaryBytes(string sql, CommandType commandType, List<SqlParameter> sqlParams)
        {
            var binaryData = new byte[] { };

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var dbCommand = new SqlCommand())
                {
                    dbCommand.Connection = conn;
                    dbCommand.CommandText = sql;
                    dbCommand.CommandType = commandType;

                    foreach (var prm in sqlParams)
                    {
                        dbCommand.Parameters.Add(prm);
                    }

                    using (var sqlRead = dbCommand.ExecuteReader())
                    {
                        if (sqlRead.HasRows)
                        {
                            while (sqlRead.Read())
                            {
                                binaryData = (byte[])sqlRead[0];
                            }
                        }
                    }
                }
            }

            return binaryData;
        }

        /// <summary>
        /// Executes a sql script batch file.
        /// </summary>
        /// <param name="scriptFilePath">File path.</param>
        public static void ExecuteBatchNonQueryFromFile(string scriptFilePath)
        {
            try
            {
                var sql = File.ReadAllText(scriptFilePath);
                ExecuteBatchNonQuery(sql);
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }
        }

        /// <summary>
        /// Executes a sql script file.
        /// </summary>
        /// <param name="sqlBatchScript">Sql query string.</param>
        public static void ExecuteBatchNonQuery(string sqlBatchScript)
        {

            using (var conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = new SqlCommand(string.Empty, conn))
                {
                    var sqlBatch = string.Empty;
                    sqlBatchScript += "\nGO";   // make sure last batch is executed.

                    foreach (var line in sqlBatchScript.Split(new string[2] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        if (line.ToUpperInvariant().Trim() == "GO")
                        {
                            if (sqlBatch.Trim() != string.Empty)
                            {
                                cmd.CommandText = sqlBatch;
                                cmd.ExecuteNonQuery();
                            }
                            sqlBatch = string.Empty;
                        }
                        else
                        {
                            sqlBatch += line + "\n";
                        }
                    }
                }
            }
        }

    }
}
