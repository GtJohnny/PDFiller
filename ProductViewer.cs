using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFiller
{
    internal class ProductViewer
    {
        private DataGridView _dataGridView;
        private Button newButton;
        private Button viewButton;
        private TextBox searchTextBox;


   

        public ProductViewer(DataGridView dataGridView, Button newButton, Button viewButton, TextBox searchTextBox)
        {
            _dataGridView = dataGridView;
            this.newButton = newButton;
            this.viewButton = viewButton;
            this.searchTextBox = searchTextBox;
            searchTextBox.TextChanged +=SearchTextBox_TextChanged;
            viewButton.Click += ViewButton_Click;
            newButton.Click += NewButton_Click;
            _dataGridView.Paint += DataGridView_Paint;
        }

        private void DataGridView_Paint(object sender, PaintEventArgs e)
        {
            if(String.IsNullOrEmpty(this.searchTextBox.Text) && _dataGridView.Rows.Count == 0)
                FillDataGridView();
        }
        private void FillDataGridView()
        {

            _dataGridView.Rows.Clear();

            _dataGridView.RowTemplate.Height = 130;
            ProductFactory factory = ProductFactory.GetInstance();
            List<Product> allProducts = factory.GetAllProducts();
            foreach (Product product in allProducts)
            {
                _dataGridView.Rows.Add(product.Id, product.Image, product.Name);
            }
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            int rowId = this._dataGridView.CurrentCell.RowIndex;
            string selectedId = this._dataGridView.Rows[rowId].Cells[0].Value.ToString();
            Form viewForm = new ProductViewForm(selectedId);
            if(viewForm.ShowDialog() == DialogResult.OK)
            {
                //refresh grid
                SearchDB(this.searchTextBox.Text);
            }
            //viewForm.ShowDialog();
        }

        private void NewButton_Click(object sender, EventArgs e)
        {
            Form viewForm = new ProductViewForm("None");
            if (viewForm.ShowDialog() == DialogResult.OK)
            {
                this.FillDataGridView();
            }
        }

        private void SearchDB(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                FillDataGridView();
                return;
            }
            string searchText = searchTextBox.Text.ToLower().Trim();
            ProductFactory factory = ProductFactory.GetInstance();
            List<Product> allProducts = factory.GetAllProducts();

            try
            {
                _dataGridView.Rows.Clear();
                foreach (Product product in allProducts.Where(p => p.Name.ToLower().Contains(searchText) || p.Id.ToLower().Contains(searchText)))
                {
                    _dataGridView.Rows.Add(product.Id, product.Image, product.Name);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error searching products: " + ex.Message);
            }
        }



        private async void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            searchTextBox.TextChanged -= SearchTextBox_TextChanged; // Unsubscribe to prevent multiple calls
            await Task.Delay(1000); // Debounce delay
            SearchDB(searchTextBox.Text);
            searchTextBox.TextChanged += SearchTextBox_TextChanged; // Resubscribe
        }   



    }
}
