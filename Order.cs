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
        public string id;
        public string awb;
        public string name;
        public List<Product> toppers;
        public string country;
        public string note;


        /// <summary>
        /// Creates a blank Order object
        /// </summary>
        public Order()
        {
            this.toppers = new List<Product>();
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
            this.name = order.name;
            this.toppers = new List<Product>(order.toppers);
            this.note = order.note;
        }

        /// <summary>
        /// Correctly instantiates an Order object
        /// </summary>
        /// <param name="id">Order ID</param>
        /// <param name="awb">AWB ID</param>
        /// <param name="name">The full name of the customer</param>
        /// <param name="tName">The name of the first topper</param>
        /// <param name="tQuantity">The bought quantity of the first topper.</param>
        /// <param name="idProduct">The ID of the first topper product.</param>
        /// <param name="country"> The country of the customer.</param>
        public Order(string id, string awb,string name, string tName, int tQuantity, string idProduct, string country)
        {
            this.id = id;
            this.awb = awb;
            this.name = name;
            this.toppers = new List<Product>();
            this.country = country;
            this.note = "";
            toppers.Add(new Topper(tName, tQuantity, idProduct));
        }
    }
}
