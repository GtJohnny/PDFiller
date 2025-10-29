using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{

    internal class Order
    {
        /// <summary>
        /// Order id (13 digit number)
        /// </summary>
        public string id;

        /// <summary>
        /// Order AWB code (13 digit number)
        /// </summary>
        public string awb;

        /// <summary>
        /// Customer full name
        /// </summary>
        public string customerName;

        /// <summary>
        /// List of all products in the order + quantity of each product
        /// </summary>
        public List<SoldProduct> products;

        /// <summary>
        /// Shipping address country
        /// </summary>
        public string country;

        /// <summary>
        /// Extra note for the order
        /// </summary>
        public string note;


        /// <summary>
        /// Creates a blank Order object
        /// </summary>
        public Order()
        {
            this.products = new List<SoldProduct>();
        }

        public Order(string Note)
        {
            this.note = Note;
        }



        /// <summary>
        /// Copy all contents from another Order object
        /// </summary>
        /// <param name="order"></param>
        public Order(Order order)
        {
            this.country = order.country;
            this.id = order.id;
            this.awb = order.awb;
            this.customerName = order.customerName;
            this.products = order.products;
            this.note = order.note;
        }

        /// <summary>
        /// Correctly instantiates an Order object
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="awb">AWB ID</param>
        /// <param name="customerName">The full name of the customer</param>
        /// <param name="product"> A product in the order</param>
        /// <param name="country"> The country of the customer.</param>
        public Order(string id, string awb, string customerName, SoldProduct product, string country)
        {
            this.id = id;
            this.awb = awb;
            this.customerName = customerName;
            this.products = new List<SoldProduct>();
            this.country = country;
            this.note = null;
            this.products.Add(product);
        }
        /// <summary>
        /// Correctly instantiates an Order object
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="awb">AWB ID</param>
        /// <param name="customerName">The full name of the customer</param>
        /// <param name="product"> A product in the order</param>
        /// <param name="country"> The country of the customer.</param>
        /// <param name="note"> Extra note for the order</param>
        public Order(string id, string awb, string customerName, SoldProduct product, string country, string note)
        {
            this.id = id;
            this.awb = awb;
            this.customerName = customerName;
            this.products = new List<SoldProduct>();
            this.country = country;
            this.note = note;
            this.products.Add(product);
        }



        /// <summary>
        /// Adds a product to the order, default quantity is 1
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(Product product)
        {
            SoldProduct soldProduct = new SoldProduct(product, 1);
            this.products.Add(soldProduct);
        }

        /// <summary>
        /// Adds a product to the order
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        public void AddProduct(Product product, int quantity)
        {
            SoldProduct soldProduct = new SoldProduct(product, quantity);
            this.products.Add(soldProduct);
        }

        /// <summary>
        /// Adds a <see cref="SoldProduct"/> to the order
        /// </summary>
        /// <param name="product"></param>
        public void AddProduct(SoldProduct product)
        {
            if (product != null)
            {
                this.products.Add(product);
            }
        }
    }
}
