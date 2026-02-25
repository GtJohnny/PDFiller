using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
namespace PDFiller
{

    internal class OracleManager
    {

        private static OracleManager instance;
        private static readonly object lockObject = new object();
        private static string connectionString;

        public static OracleManager GetInstance()
        {

            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new OracleManager();
                }
            }

            return instance;
        }

        private OracleManager()
        {
            StreamReader sr = new StreamReader(".env");
            connectionString = Encoding.ASCII.GetString(Convert.FromBase64String(sr.ReadLine()));

            OracleConfiguration.TnsAdmin = @"Wallet_PDFiller";
            OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The <see cref="Product"/> associated with the specified identifier 
        /// or null if nothing was found</returns>
        public Product GetProductById(string id)
        {
            Product product = null;
            try
            {
                List<Product> products = GetProductsById(new string[] { id });
                if (products.Count > 0)
                {
                    product = products[0];
                }
                else
                {
                    product = null;
                }
            }
            catch (ArgumentException ex)
            {
                throw ex;
            }

            return product;
        }



        /// <summary>
        /// Runs a select query on the database 
        /// and returns an array of Products that match every id in the ids array.
        /// </summary>
        /// <param name="pnks">An array of all products pnks</param>
        /// <returns>An array of all products from the database</returns>
        /// <exception cref="ArgumentException">Error reading from database</exception>
        public List<Product> GetProductsById(string[] pnks)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleCommand cmd = conn.CreateCommand();

                try {
                    conn.Open();
                } catch (Exception ex)
                {
                    Console.WriteLine("Error connecting to database: " + ex.Message);
                    throw new ArgumentException("Error connecting to database: " + ex.Message);
                }
                var parameters = new List<string>();
                for (int i = 0; i < pnks.Length; i++)
                {
                    parameters.Add($"@id{i}");
                    OracleParameter p = new OracleParameter($"@id{i}", OracleDbType.Varchar2);
                    p.Value = pnks[i];
                    cmd.Parameters.Add(p);
                }
                cmd.CommandText = $"select * from Produs where id in ({string.Join(",", parameters)});";



                //cmd.Parameters.AddWithValue("ids", "(" + string.Join(",", pnks) + ")");
                OracleDataReader reader = cmd.ExecuteReader();
                List<Product> products = new List<Product>();
                try
                {
                    while (reader.Read())
                    {
                        int id = (int)reader["id_produs"];
                        byte[] imgBytes = (byte[])reader["imagine"];
                        string name = reader["name"].ToString();
                        string pnk = reader["pnk"].ToString();
                        int id_category = (int)reader["id_category"];

                        products.Add(new Product(id, pnk, imgBytes, name, id_category);

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading data: " + ex.Message);
                }
                reader.Close();
                conn.Close();
                return products;
            }
        }
    }
}
