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
        public ProductViewForm(string productId)
        {
            ProductFactory factory = ProductFactory.GetInstance();
            this.product = factory.GetProduct(productId);
            if (this.product == null)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
            InitializeComponent();
        }

        private void ProductViewForm_Load(object sender, EventArgs e)
        {

        }
    }
}
