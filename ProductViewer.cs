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
        private Button searchButton;
        private Button viewButton;
        private TextBox searchTextBox;
        private Form viewForm;
        private SqlConnection conn;
        private DispatchWrapper _dispatchWrapper;

        private void FillDataGridView()
        {
            if (_dataGridView.Rows.Count > 0)
            {
                _dataGridView.Rows.Clear();
            }
            SqlCommand cmd = new SqlCommand("SELECT * FROM TOPPERS;", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            _dataGridView.RowTemplate.Height = 130;


            while (reader.Read())
            {
                string id = reader["ID"].ToString();
                byte[] image = (byte[])reader["IMAGE"];
                string name = reader["NAME"].ToString();

                Bitmap bmp = new Bitmap(new MemoryStream(image));
                _dataGridView.Rows.Add(id, image, name);
            }
            reader.Close();
        }

   

        public ProductViewer(DataGridView dataGridView, Button searchButton, Button viewButton, TextBox searchTextBox, string connectionString)
        {
            _dataGridView = dataGridView;
            this.searchButton = searchButton;
            this.viewButton = viewButton;
            this.searchTextBox = searchTextBox;
            this.conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Error connecting to database: " + ex.Message);
            }
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
            SqlCommand cmd = new SqlCommand("SELECT * FROM TOPPERS WHERE LOWER(NAME) LIKE @searchText OR ID LIKE @searchText;", conn);
            cmd.Parameters.AddWithValue("searchText", $"%{searchText}%");
            SqlDataReader reader = null;



            try
            {
                reader = cmd.ExecuteReader();
                if (_dataGridView.Rows.Count > 0)
                {
                    _dataGridView.Rows.Clear();
                }
                while (reader.Read())
                {
                    string id = reader["ID"].ToString();
                    byte[] image = (byte[])reader["IMAGE"];
                    string name = reader["NAME"].ToString();
                    Bitmap bmp = new Bitmap(new MemoryStream(image));
                    _dataGridView.Rows.Add(id, image, name);
                }
                reader.Close();
            }
            catch (InvalidOperationException ex)
            {
                reader.Close();
                conn.Close();
            }
        }



        private async void SearchTextBox_TextChanged(object sender, EventArgs e)
        {
            searchTextBox.TextChanged -= SearchTextBox_TextChanged; // Unsubscribe to prevent multiple calls
            await Task.Delay(1000); // Debounce delay
            await SearchDB(searchTextBox.Text);
            searchTextBox.TextChanged += SearchTextBox_TextChanged; // Resubscribe
        }   


        class TOPPERDBO
        {
            public int ID { get; set; }
            public byte[] IMAGE { get; set; }
            public string NAME { get; set; }
        }
    }
}
