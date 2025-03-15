using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{

    internal class Order
    {
        public string id;
        public string awb;
        public struct topper
        {
            public string tName;
            public int tQuantity;
            public topper(string tName, int tQuantity)
            {
                this.tName = tName;
                this.tQuantity = tQuantity;
            }
        };
        public List<topper> toppere;

        public Order()
        {
            this.id = "";
            this.awb = "";
            this.toppere = new List<topper>();
        }

        public Order(string id, string awb, string tName, int tQuantity)
        {
            this.id = id;
            this.awb = awb;
            this.toppere = new List<topper>();
            toppere.Add(new topper(tName, tQuantity));
        }
    }
}
