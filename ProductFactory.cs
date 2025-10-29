using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class ProductFactory
    {
        private Dictionary<string, Product> products;
        private static ProductFactory instance;
        private static readonly object lockObject = new object();
        SQLManager sqlManager;


        public static ProductFactory GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new ProductFactory();
                    }
                }
            }
            return instance;
        }

        public ProductFactory()
        {
            this.sqlManager = SQLManager.GetInstance();
            this.products = new Dictionary<string, Product>();
        }


        public List<Product> GetAllProducts()
        {
            try
            {
                List<Product> allProducts = sqlManager.GetAllProducts();
                foreach (Product product in allProducts)
                {
                    if (!products.ContainsKey(product.Id))
                    {
                        products[product.Id] = product;
                    }
                }
                return allProducts;
            }
            catch (Exception ex)
            {
                throw new Exception("Error getting products from database: " + ex.Message);
            }
        }


        /// <summary>
        /// Returns the product from the Flyweight pool of <see cref="Product"/> if it exists,
        /// otherwise fetches it from the database and adds it to the pool.
        /// </summary>
        /// <param name="id">Product id</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown if product id couldn't be found in the database</exception>
        /// <exception cref="Exception">Thrown otherwise</exception>
        public Product GetProduct(string id)
        {
            if (id == null)
            {
                throw new ArgumentNullException("Product ID is null");
            }
            if (!products.ContainsKey(id))
            {
                try
                {
                    Product product = sqlManager.GetProductById(id);
                    if (product == null)
                    {
                        throw new ArgumentException("Product not found in database");
                    }
                    products[id] = product;
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Error getting product from database: " + ex.Message);

                }
            }
            return products[id];
        }


        public List<Product> GetProducts(string[] ids)
        {
            List<Product> result = null;
            try
            {
                result = sqlManager.GetProductsById(ids);
                foreach (Product product in result)
                {
                    if (!products.ContainsKey(product.Id))
                    {
                        products[product.Id] = product;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

    }
}
