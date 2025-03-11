using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Xml.Linq;


namespace PDFiller
{
    public partial class Form1 : Form
    {
        string root_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\\";
        string root2 = "C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\AWBFiller\\bin\\Debug\\";
        public Form1()
        {
            InitializeComponent();
        }


        public void WriteOnDirectory(DirectoryInfo dir, FileInfo excelFile)
        {

            Excel.Application app = new Excel.Application();



            Workbook book = app.Workbooks.Open(excelFile.FullName);
            Worksheet sheet;



            try
            {
                sheet = book.Worksheets[1];
                textBox1.Text += "Opened Excel file\r\n";
                int row = 2;
                Range cell = sheet.Cells[1][1];
                textBox1.Text += cell.Value2;




            }
            catch (Exception ex)
            {
                textBox1.Text += ex.Message;

                book.Close();
            }
        }


        public void WriteOnPage(string text, PdfPage page)
        {

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Times New Roman", 15);
            XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));

            XRect rect = new XRect(0, page.Height / 2 - 15, page.Width, page.Height / 2 + 15);
            gfx.DrawRectangle(XBrushes.White, rect);


            gfx.DrawString(text, font, brush, rect, XStringFormats.Center);

        }


        public bool FindZipsUnzipped(DirectoryInfo workdir, FileInfo excel)
        {
            FileInfo[] zips = workdir.GetFiles().Where(x => x.Extension == ".zip").ToArray();
            DirectoryInfo[] extractedZips = workdir.GetDirectories().ToArray();

            bool noneFound = true;
            foreach (FileInfo zip in zips)
            {
                bool found = false;

                foreach (DirectoryInfo dir in extractedZips)
                {
                    if (dir.Name == zip.Name.Replace(".zip", "")) //zip was not unzipped
                    {
                        found = true;
                        noneFound = false;
                        break;
                    }
                }

                if (!found)
                {
                    textBox1.Text += "One zip file was found that was not extracted.\r\n" + zip.FullName + "\r\nExtracting it now.\r\n";
                    ZipArchive archive = new ZipArchive(new FileStream(zip.FullName, FileMode.Open), ZipArchiveMode.Read);
                    string extractedDir = zip.FullName.Replace(".zip", "");
                    //     archive.ExtractToDirectory(extractedDir);

                    WriteOnDirectory(new DirectoryInfo(extractedDir), excel);



                }
            }
            if (!noneFound)
            {
                textBox1.Text += "No zip file was found that was not already extracted.\r\n";
            }
            return noneFound;
        }


        public FileInfo FindExcel(DirectoryInfo workdir)
        {

            FileInfo details;
            try
            {
                details = workdir.GetFiles().Where(x => x.Extension == ".xlsx" || x.Extension == ".xls")
                                                  .OrderByDescending(x => x.CreationTime).ToArray()[0];
            }
            catch (IndexOutOfRangeException)
            {
                textBox1.Text += "ORDERS DETAILS FILE NOT FOUND\r\n";
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            textBox1.Text += "Orders details found at: \r\n" + details.FullName + "\r\n";
            textBox1.Text += "The most recent directory is \"" + details.Name + "\" created on \"" + details.CreationTime + "\" or \r\n";

            if ((DateTime.Now - details.CreationTime).Days > 0)
            {
                textBox1.Text += (DateTime.Now - details.CreationTime).Days + "days ago. \r\n";
            }
            else
            {
                textBox1.Text += (DateTime.Now - details.CreationTime).Minutes + " minutes ago. \r\n";
            }
            return details;
        }

        public string lorem_ipsum = "lorem ipsum dolor sit amet blasphemous E1331";



        public void SeeRootDirectory()
        {
            textBox1.Text = "Root Directory found at: \r\n" + root_path + "\r\n";
            DirectoryInfo rootdir = new DirectoryInfo(root_path);
            DirectoryInfo workdir;
            try
            {
                workdir = rootdir.GetDirectories().OrderByDescending(x => x.CreationTime).ToArray()[0];
            }
            catch (IndexOutOfRangeException)
            {
                textBox1.Text += "EMPTY DIRECTORY,NOTHING TO CHECK\r\n";
                return;
            }
            catch (Exception e)
            {
                throw e;
            }
            textBox1.Text += "Work Directory found at: \r\n" + workdir.FullName + "\r\n";
            textBox1.Text += "The most recent directory is \"" + workdir.Name + "\" created on \"" + workdir.CreationTime + "\" or \r\n";

            if ((DateTime.Now - workdir.CreationTime).Days > 0)
            {
                textBox1.Text += (DateTime.Now - workdir.CreationTime).Days + " days ago. \r\n";
            }
            else
            {
                textBox1.Text += (DateTime.Now - workdir.CreationTime).Minutes + " minutes ago. \r\n";
            }


            FileInfo excel = FindExcel(workdir);
            FindZipsUnzipped(workdir, excel);

        }




        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            if (Directory.Exists(root_path))
            {
                SeeRootDirectory();
            }
            else
            {
                textBox1.Text = "Root Directory not found. Please select one, then your current work Directory";
            }
        }
    }
}

