using System;
using System.Collections.Generic;
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

        public Order(string id, string awb,string name, string tName, int tQuantity)
        {
            this.id = id;
            this.awb = awb;
            this.name = name;
            this.toppere = new List<topper>();
            toppere.Add(new topper(tName, tQuantity));
        }
    }
}
