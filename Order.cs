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
        public List<topper> toppere;

        public struct topper
        {
            public string tName;
            public int tQuantity;
            public string tId;
            public topper(string tName, int tQuantity,string tId)
            {
                this.tName = tName;
                this.tQuantity = tQuantity;
                this.tId = tId;
            }

        };


        /// <summary>
        /// Creates a blank Order object
        /// </summary>
        public Order()
        {
            this.id = "";
            this.awb = "";
            this.toppere = new List<topper>();
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
        public Order(string id, string awb,string name, string tName, int tQuantity, string idProduct)
        {
            this.id = id;
            this.awb = awb;
            this.name = name;
            this.toppere = new List<topper>();
            toppere.Add(new topper(tName, tQuantity, idProduct));
        }
    }
}
