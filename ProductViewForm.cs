using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFiller
{
    internal partial class ProductViewForm : Form
    {

        private Product product;
        public ProductViewForm(Product product)
        {
            this.product = product;
            InitializeComponent();
        }

        private void ProductViewForm_Load(object sender, EventArgs e)
        {

        }
    }
}
