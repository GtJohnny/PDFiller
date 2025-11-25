using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFiller
{
    internal partial class ProductViewForm : Form
    {

        private Product product;
        byte[] tempBuffer;
        bool newProduct = false;
        public ProductViewForm(string productId)
        {
            InitializeComponent();

            if (productId == "None")
            {
                newProduct = true;
                saveBtn.Text = "Create";
                product = new Product("None", null, "Choose a name");
                return;
            }

            ProductFactory factory = ProductFactory.GetInstance();
            this.product = factory.GetProduct(productId);
            if (this.product == null)
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void ProductViewForm_Load(object sender, EventArgs e)
        {
            productPictureBox.Image = product.Image;
            productPnTextBox.Text = product.Id;
            productNameTextBox.Text = product.Name;
            productTypeComboBox.Text = "N/A";
        }

        private void loadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".png files()|*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileInfo file = new FileInfo(ofd.FileName);
                if (file.Exists == false)
                {
                    MessageBox.Show("File does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (file.Length > 2 * 1024 * 1024)
                {
                    MessageBox.Show("File size exceeds 2MB.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                tempBuffer = File.ReadAllBytes(ofd.FileName);
                productPictureBox.Image = new Bitmap(new MemoryStream(tempBuffer));
            }
        }


        private void tryAddNew(string id, string name, string type)
        {
            //type not used yet.
            Product product = new Product(id, tempBuffer, name);
            SQLManager man = SQLManager.GetInstance();

            try
            {
                bool success = man.InsertProduct(product);
                if (success)
                {
                    MessageBox.Show("Product created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to create product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error create product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void tryUpdate(string id, string name, string type)
        {
            SQLManager man = SQLManager.GetInstance();

            try
            {
                bool success = man.updateProduct(product, id, tempBuffer, name);
                if (success)
                {
                    MessageBox.Show("Product updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {

            if (newProduct)
            {
                if (DialogResult.Yes != MessageBox.Show("Create product", "Are you certain the values are correct?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    return;
                }
            }
            else if (DialogResult.Yes != MessageBox.Show("Save product", "Are you certain the values are correct?", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            {
                return;
            }



            string id = productPnTextBox.Text;
            if (id.Length != 13)
            {
                MessageBox.Show("Product Number must be exactly 13 characters long.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string name = productNameTextBox.Text;
            if (name.Length > 50)
            {
                MessageBox.Show("Product Name cannot exceed 50 characters.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string type = productTypeComboBox.Text;
            if (id == product.Id && name == product.Name && tempBuffer == null)
            {
                MessageBox.Show("No changes detected.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (newProduct)
            {
                tryAddNew(id, name, type);
            }
            else
            {
                tryUpdate(id, name, type);
            }

        }


        private void resetBtn_Click(object sender, EventArgs e)
        {
            productPictureBox.Image = product.Image;
            productPnTextBox.Text = product.Id;
            productNameTextBox.Text = product.Name;
            productTypeComboBox.SelectedIndex = 0;
            tempBuffer = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!newProduct)
            {
                if (DialogResult.Yes != MessageBox.Show("Delete product", "Are you certain you want to delete this product?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning))
                {
                    return;
                }
                SQLManager man = SQLManager.GetInstance();
                if (man.DeleteProduct(product.Id))
                {
                    MessageBox.Show("Product deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to delete product.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
