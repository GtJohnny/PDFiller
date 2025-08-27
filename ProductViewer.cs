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

        private void FillDataGridView()
        {
            if (_dataGridView.Rows.Count > 0)
            {
                _dataGridView.Rows.Clear();
            }
            _dataGridView.RowTemplate.Height = 130;
            ProductFactory factory = ProductFactory.GetInstance();
            List<Product> allProducts = factory.GetAllProducts();
            foreach (Product product in allProducts)
            {
                _dataGridView.Rows.Add(product.Id, product.Image, product.Name);
            }
        }

   

        public ProductViewer(DataGridView dataGridView, Button searchButton, Button viewButton, TextBox searchTextBox)
        {
            _dataGridView = dataGridView;
            this.newButton = searchButton;
            this.viewButton = viewButton;
            this.searchTextBox = searchTextBox;
            FillDataGridView();
            searchTextBox.TextChanged +=SearchTextBox_TextChanged;
            searchButton.Click += SearchButton_Click;
            viewButton.Click += ViewButton_Click;
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private async Task SearchDB(string searchString)
        {
            if (string.IsNullOrWhiteSpace(searchTextBox.Text))
            {
                FillDataGridView();
                return;
            }
            string searchText = searchTextBox.Text.ToLower();
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
            await SearchDB(searchTextBox.Text);
            searchTextBox.TextChanged += SearchTextBox_TextChanged; // Resubscribe
        }   



    }
}
