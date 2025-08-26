using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class SQLManager
    {
        private string connectionString = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename{Environment.CurrentDirectory}\\Database1.mdf;Integrated Security=True";
        SqlConnection conn = new SqlConnection();
        private static SQLManager instance;
        private static readonly object lockObject = new object();

        Regex idRegex = new Regex(@"^[0-9]{13}$");
        Regex nameRegex = new Regex(@"^[a-zA-Z0-9\s&]{1,100}$");

        public static SQLManager GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new SQLManager();
                    }
                }
            }
            return instance;
        }


        private SQLManager()
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

        public Product[] SelectProductsByIdOrName(string str)
        {
            if(str == null)
            {
                throw new ArgumentNullException("Product search query string is null");
            }

            if (nameRegex.IsMatch(str))
            {
                SqlCommand cmd = new SqlCommand("select * from toppers where id like @name or name like @name;", conn);
                cmd.Parameters.AddWithValue("@name", "%" + str + "%");
                SqlDataReader reader = cmd.ExecuteReader();
                List<Product> products = new List<Product>();
                try
                {
                    while (reader.Read())
                    {
                        string id = reader["id"].ToString();
                        byte[] imgBytes = (byte[])reader["image"];
                        string name = reader["name"].ToString();
                        using (var ms = new System.IO.MemoryStream(imgBytes))
                        {
                            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
                            products.Add(new Product(id, bmp, name));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error reading data: " + ex.Message);
                }
                reader.Close();
                return products.ToArray();
            }
            else
            {
                throw new ArgumentException("Invalid search query format");
            }
        }




        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The <see cref="Product"/> associated with the specified identifier 
        /// or <see cref="null"/></returns>
        public Product GetProductById(string id)
        {
            Product product=null;
            try
            {
                Product[] products = GetProductsById(new string[] { id });
                product = products[0];
            }
            catch(ArgumentException ex)
            {
                throw ex;
            }

            return product;
        }



        /// <summary>
        /// Runs a select query on the database 
        /// and returns an array of Products that match every id in the ids array.
        /// </summary>
        /// <param name="ids">An array of all products ids</param>
        /// <returns>An array of all products from the database</returns>
        /// <exception cref="ArgumentException">Error reading from database</exception>
        public Product[] GetProductsById(string[] ids)
        {
            SqlCommand cmd = new SqlCommand("select * from toppers where id in(@ids);",conn);
            foreach (string id in ids)
            {
                if (!idRegex.IsMatch(id))
                {
                    throw new ArgumentException("Invalid ID format");
                }
            }

            cmd.Parameters.AddWithValue("@ids", string.Join(",", ids));
            SqlDataReader reader = cmd.ExecuteReader();
            Product[] products = new Product[ids.Length];
            int index = 0;
            try
            {
                while (reader.Read())
                {
                    string id = reader["id"].ToString();
                    byte[] imgBytes = (byte[])reader["image"];
                    string name = reader["name"].ToString();
                    using (var ms = new System.IO.MemoryStream(imgBytes))
                    {
                        System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(ms);
                        products[index++] = new Product(id, bmp, name);
                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }
            reader.Close();
            return products;
        }

        /// <summary>
        /// Closes the database connection if it is open.
        /// </summary>
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
