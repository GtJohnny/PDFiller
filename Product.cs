using PdfSharp.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class Product
    {
        private int id;
        private string pnk;
        private byte[] buff;
        private string name;
        private int id_category;

        public Product()
        {
            id = 0;
            pnk = "";
            buff = null;
            name = "";
            id_category = 1;
        }
        public Product(int id, string pnk, byte[] buff, string name, int id_category)
        {
            this.id = id;
            this.pnk = pnk;
            this.buff = buff;
            this.name = name;
            this.id_category = id_category;
        }
        public int Id { get => id; }
        public string Pnk { get => pnk; }
        public byte[] ImageBuffer { get => buff; }
        public string Name { get => name; }

        public int Id_category { get => id_category; }
        public Bitmap Image
        {
            get
            {
                if (buff == null)
                {
                    return null;
                }
                using (var ms = new MemoryStream(buff))
                {
                    return new Bitmap(ms);
                }
            }
        }

    }

}
