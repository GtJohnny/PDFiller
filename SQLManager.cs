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
        private string connectionString = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={Environment.CurrentDirectory}\Database1.mdf;Integrated Security=True";
        SqlConnection conn = new SqlConnection();
        private static SQLManager instance;
        private static readonly object lockObject = new object();

        Regex idRegex = new Regex(@"^[0-9]{13}$");
        //Regex nameRegex = new Regex(@"^[a-zA-Z0-9\s&]{1,100}$");

        public static SQLManager GetInstance()
        {

            lock (lockObject)
            {
                if (instance == null)
                {
                    instance = new SQLManager();
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




        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product to retrieve.</param>
        /// <returns>The <see cref="Product"/> associated with the specified identifier 
        /// or null if nothing was found</returns>
        public Product GetProductById(string id)
        {
            Product product=null;
            try
            {
                List<Product> products = GetProductsById(new string[] { id });
                if(products.Count > 0)
                {
                    product = products[0];
                }
                else
                {
                    product = null;
                }
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
        public List<Product> GetProductsById(string[] ids)
        {
            foreach (string id in ids)
            {
                if (!idRegex.IsMatch(id))
                {
                    throw new ArgumentException("Invalid ID format");
                }
            }
            SqlCommand cmd = conn.CreateCommand();

            var parameters = new List<string>();
            for (int i = 0; i < ids.Length; i++)
            {
                parameters.Add($"@id{i}");
                cmd.Parameters.AddWithValue($"@id{i}", ids[i]);
            }
            cmd.CommandText = $"select * from toppers where id in ({string.Join(",", parameters)});";



            cmd.Parameters.AddWithValue("ids", "("+string.Join(",",ids)+")");
            SqlDataReader reader = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            try
            {
                while (reader.Read())
                {
                    string id = reader["id"].ToString();
                    byte[] imgBytes = (byte[])reader["image"];
                    string name = reader["name"].ToString();

                    products.Add(new Product(id, imgBytes, name));

                }
            }catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }
            reader.Close();
            return products;
        }




        public List<Product> GetAllProducts()
        {
            SqlCommand cmd = new SqlCommand("select * from toppers;", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            List<Product> products = new List<Product>();
            try
            {
                while (reader.Read())
                {
                    string id = reader["id"].ToString();
                    byte[] imgBytes = (byte[])reader["image"];
                    string name = reader["name"].ToString();
                    products.Add(new Product(id, imgBytes, name));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error reading data: " + ex.Message);
            }
            reader.Close();
            return products;
        }


        public bool InsertProduct(Product product)
        {
            //see if product id already exists
            SqlCommand check_cmd = conn.CreateCommand();
            check_cmd.CommandText = "select count(*) from toppers where id = @id;";
            check_cmd.Parameters.AddWithValue("@id", product.Id);
            int count = (int)check_cmd.ExecuteScalar();
            if (count > 0)
            {
                //product already exists
                throw new ArgumentException("Product with this ID already exists");
            }
            try
            {

                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "insert into toppers (id, image, name) values (@id, @image, @name);";

                cmd.Parameters.AddWithValue("@id", product.Id);
                SqlParameter p1 = new SqlParameter("@image", System.Data.SqlDbType.VarBinary);
                p1.Value = product.ImageBuffer;

                cmd.Parameters.Add(p1);
                cmd.Parameters.AddWithValue("@name", product.Name);
                int rows = cmd.ExecuteNonQuery();
                if (rows == 0)
                {
                    throw new Exception("No rows inserted");
                }
                return rows == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting product: " + ex.Message);
                return false;

            }

        }
        /// <summary>
        /// Updates a product in the database.
        /// </summary>
        /// <remarks>This is meant in ProductViewForm</remarks>
        /// <param name="product">Old product</param>
        /// <param name="newId">New id</param>
        /// <param name="imageBuffer">New image, can be null for no change</param>
        /// <param name="name">New name</param>
        public bool updateProduct(Product product, string newId, byte[] imageBuffer, string name)
        {
            //check if id already exists and is not the same as the old id
            if (newId != product.Id)
            {
                SqlCommand check_cmd = conn.CreateCommand();
                check_cmd.CommandText = "select count(*) from toppers where id = @id;";
                check_cmd.Parameters.AddWithValue("@id", newId);
                int count = (int)check_cmd.ExecuteScalar();
                if (count > 0)
                {
                    //product already exists
                    throw new ArgumentException("Product with this ID already exists");
                }

            }




                SqlCommand cmd = conn.CreateCommand();
            if (imageBuffer != null)
            {
                cmd.CommandText = "update toppers set id=@newId, image = @image, name = @name where id = @oldId;";
                SqlParameter p1 = new SqlParameter("@image", System.Data.SqlDbType.VarBinary);
                p1.Value = imageBuffer;
                cmd.Parameters.Add(p1);
            }
            else
            {
                cmd.CommandText = "update toppers set id=@newId, name = @name where id = @oldId;";
            }
            cmd.Parameters.AddWithValue("@oldId", product.Id);
            cmd.Parameters.AddWithValue("@newId", newId);
            cmd.Parameters.AddWithValue("@name", name);
            try
            {
                int rows = cmd.ExecuteNonQuery();
                return rows == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating product: " + ex.Message);
                return false;
            }
        }

        public bool DeleteProduct(string id)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "delete from toppers where id = @id;";
            cmd.Parameters.AddWithValue("@id", id);
            try
            {
                int rows = cmd.ExecuteNonQuery();
                return rows == 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting product: " + ex.Message);
                return false;
            }
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
