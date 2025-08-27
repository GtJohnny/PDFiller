using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class SoldProduct
    {
        private Product product;
        private int quantity;

        public string Name{ get => product.Name; }
        public Bitmap Image { get => product.Image; }
        public string Id { get => product.Id; }
        public int Quantity { get => quantity; set => quantity = value; }

        public SoldProduct() {
            product = new Product();
            quantity = 0;
        }    
        public SoldProduct(Product product, int quantity)
        {
            this.product = product;
            this.quantity = quantity;
        }

    }
}
