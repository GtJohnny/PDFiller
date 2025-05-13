using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Web;
using System.Net;


namespace PDFiller
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private readonly Dictionary<String, String> SpecialSwaps__ = new Dictionary<String, String>()
        {
     
            { "5941933302517", "Barbie tip4 (cercuri)" },
            { "5941933302524", "Barbie tip3 (silueta cap)" },
            { "5941933302531", "Barbie tip2 (cercuri fancy)" },
            { "5941933302548", "Barbie tip1 (cercuri funda)" },
            { "5941933302470", "Baby Boss tip3 (cercuri Logo)" },
            { "5941933302487", "Baby Boss tip2 (cercuri copil)" }
        };


        private void Form2_Load(object sender, EventArgs e)
        {
            //Builder builder = Builder.GetInstance();
            //const string path = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\";
            //string inputfPath = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\422415612_eMAG_Courier_4EMGLN113959080001.pdf";
            //string excelPath = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\orders_details_file_06-05-2025-09-48-37.xlsx";
            //List<Order> orders = builder.ReadExcel(new FileInfo(excelPath));
            //List<FileInfo> unzipped = new List<FileInfo>()
            //{
            //    new FileInfo(inputfPath)
            //};
            //string savedPDFpath = builder.WriteOnOrders(unzipped, orders, path, "TestName");
            //Process.Start(savedPDFpath);

            //return;



        }

        private void button2_Click(object sender, EventArgs e)
        {
            var cli = new WebClient();
            var img = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\google.png";
            cli.DownloadFile("https://www.google.com/images/branding/googlelogo/2x/googlelogo_light_color_92x30dp.png", img);
            pictureBox1.Image = Image.FromFile(img);




        }
        private void Form2_Load_1(object sender, EventArgs e)
        {

            
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
       //     this.ActiveControl = null;
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            //this.ActiveControl = null;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
         //   this.ActiveControl = null;
        }

        private void comboBox1_DropDownClosed(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }


        //public async Task<FileInfo> doWork()
        //{

        //    ProgressBar bar = new ProgressBar();

        //    for(int i = 0; i < 10; i++)
        //    {
        //        bar.PerformStep();
        //        MessageBox.Show($"{bar.Value}");
        //    }
        //    return new FileInfo("");
        //}






    }
}
