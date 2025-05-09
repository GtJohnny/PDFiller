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
    


        //{
        //        new KeyValuePair<String, String>("Set 17 figurine tort/briose Patrula Catelusilor, KZE Prints, Photo Paper Glossy", "Paw Patrol tip2 (nou)"),
        //        new KeyValuePair<String, String>("Set 9 figurine tort Patrula Catelusilor, KZE Prints, Photo Paper Glossy", "Paw Patrol tip1 (vechi)"),
        //        new KeyValuePair<String, String>("albinuta", "Albinute mici"),
        //        new KeyValuePair<String, String>("apicultor", "Albine + Apicultor"),
        //        new KeyValuePair<String, String>("5941933302517", "Barbie tip4 (cercuri)"),
        //        new KeyValuePair<String, String>("5941933302524", "Barbie tip3 (silueta)"),
        //        new KeyValuePair<String, String>("5941933302531", "Barbie tip2 (fancy)"),
        //        new KeyValuePair<String, String>("5941933302548", "Barbie tip1 (funda)"),
        //        new KeyValuePair<String, String>("Set 10 figurine tort/briose Baby Boss, Tip 3, KZE Prints, Photo Paper Glossy", "Baby Boss tip3 (Logo)"),
        //        new KeyValuePair<String, String>("5941933302487", "Baby Boss tip2 (cercuri copil)"),
        //        new KeyValuePair<String, String>("Set 12 figurine tort Buburuza, KZE Prints, Photo Paper Glossy", "12 Buburuze"),
        //        new KeyValuePair<String, String>("Set 12 figurine tort Inima Roz, KZE Prints, Photo Paper Glossy", "12 Inimi Roz <3"),
        //        new KeyValuePair<String, String>("Set 11 figurine tort Capsune, KZE Prints, Photo Paper Glossy", "11 Capsune + Vrej"),
        //        new KeyValuePair<String, String>("5941933302470", "Baby Boss tip3 (Logo)"),
        //};

        private void Form2_Load(object sender, EventArgs e)
        {


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Builder builder = Builder.GetInstance();
            const string path = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\";
            string inputfPath = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\422415612_eMAG_Courier_4EMGLN113959080001.pdf";
            string excelPath = @"C:\Users\KZE PC\Desktop\DEBUGGING\Quick_Thing\orders_details_file_06-05-2025-09-48-37.xlsx";
            List<Order> orders = builder.ReadExcel(new FileInfo(excelPath));
            List<FileInfo> unzipped = new List<FileInfo>()
            {
                new FileInfo(inputfPath)
            };
            string savedPDFpath = builder.WriteOnOrders(unzipped, orders, path, "TestName");
            Process.Start(savedPDFpath);

            return;


            const string imgPath = @"C:\Users\KZE PC\Desktop\DEBUGGING\ImaginiAwb-uri";
            Regex regex = new Regex(@"\b[0-9]{13}.(png|jpeg)\b");
            DirectoryInfo directory = new DirectoryInfo(imgPath);

            // Get files asynchronously
            FileInfo[] files = directory.GetFiles().Where(f => regex.IsMatch(f.Name) && SpecialSwaps__.Any(item => item.Key == Path.GetFileNameWithoutExtension(f.Name))).ToArray();

            PdfDocument pdf = new PdfDocument();
            pdf.AddPage(new PdfPage());
            PdfPage page = pdf.Pages[0];

            XGraphics gfx = XGraphics.FromPdfPage(page);

            int i = 0;
            int perPage = 3;
         //   bool doWrite = true;

            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    //Write
                    //NoDraw
                    break;
                case 1:

                    perPage = 2;
                    //doWrite = true;
                    //Draw
                    break;
                case 2:
                    perPage = 3;
                    //noWrite
                    //Draw
                    break;
            }
      

            foreach (FileInfo file in files)
            {

                XImage image = XImage.FromFile(file.FullName);
                string name = SpecialSwaps__[Path.GetFileNameWithoutExtension(file.Name)];
                //                                                                                       (scales with images/row)+ (pageH=90 +30 space)+no out of bounds  
                gfx.DrawImage(image, (i % perPage) * (120 + 120/perPage) +(perPage==2 ? page.Width/2.2 : page.Width/6), (i / perPage) * 120 + 70 + page.Height/2 , 90, 90);
                                                                                                           //per pozition *  (pageH=90 +30 space + space with img/row) - (center text) + (even abscise per img/row (2= right column, 3=wide)
                gfx.DrawString(name, new XFont("Times New Roman", 12, XFontStyle.Regular), XBrushes.Black, (i % perPage) * (120 + 120 / perPage) + 45 - 4.5f*(name.Count()/2) + (perPage == 2 ? page.Width / 2.2 : page.Width / 6), (i / perPage) * 120 + 50 + 90 + 30 + page.Height / 2);

                i++;
            }


            string pdfPath = $"{path}\\MULTE_POZE.pdf";
            pdf.Save(pdfPath);
            pdf.Close();
            Process.Start(pdfPath);
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
