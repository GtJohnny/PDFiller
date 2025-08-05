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
        public List<topper> toppers;
        public string country;

        public struct topper
        {
            public string name;
            public int quantity;
            public string id;
            public topper(string tName, int tQuantity,string tId)
            {
                this.name = tName;
                this.quantity = tQuantity;
                this.id = tId;
            }

        };


        /// <summary>
        /// Creates a blank Order object
        /// </summary>
        public Order()
        {
            this.id = "";
            this.name = "";
            this.awb = "";
            this.country = "";
            this.toppers = new List<topper>();
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
        public Order(string id, string awb,string name, string tName, int tQuantity, string idProduct, string country)
        {
            this.id = id;
            this.awb = awb;
            this.name = name;
            this.toppers = new List<topper>();
            this.country = country;
            toppers.Add(new topper(tName, tQuantity, idProduct));
        }
    }
}
