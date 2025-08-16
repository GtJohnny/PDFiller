using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class SQLController
    {
        private string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename{Environment.CurrentDirectory}\\Database1.mdf;Integrated Security=True";
        SqlConnection conn = new SqlConnection();
        private static SQLController instance;
        private static readonly object lockObject = new object();

        public static SQLController GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SQLController();
                    }
                }
            }
            return instance;
        }


        private SQLController()
        {
            conn.ConnectionString = connectionString;
            try
            {
                conn.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error connecting to database: " + ex.Message);
            }
        }

        public int ExecuteQuery(string query, params SqlParameter[] sqlParameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                return -1; // Return -1 to indicate an error
            }
        }

        public SqlDataReader ExecuteReader(string query, params SqlParameter[] sqlParameters)
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    if (sqlParameters != null)
                    {
                        cmd.Parameters.AddRange(sqlParameters);
                    }
                    return cmd.ExecuteReader();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error executing reader: " + ex.Message);
                return null;
            }
        }

        public void CloseConnection()
        {
            lock (lockObject)
            {
                if (conn != null && conn.State == System.Data.ConnectionState.Open)
                {
                    try
                    {
                        conn.Close();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error closing connection: " + ex.Message);
                    }
                }
            }

        }
    }
}
