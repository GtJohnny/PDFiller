using PdfSharpCore.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class Product
    {
        private string id;
        private Bitmap bmp;
        private string name;
        public Product(string id, Bitmap bmp, string name)
        {
            this.id = id;
            this.bmp = bmp;
            this.name = name;
        }

        public string Id { get => id; }
        public Bitmap Image { get => bmp; }
        public string Name { get => name; }
    };

}
