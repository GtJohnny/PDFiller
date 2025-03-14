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
using System.Reflection.Emit;



namespace PDFiller
{
    public partial class Form1 : Form

    {
        string root_path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\\";
        string workdir_path;
        string zip_path;
        string excel_path;
        List<FileInfo> unzipped_list = new List<FileInfo>();


        //  string root2 = "C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\AWBFiller\\bin\\Debug\\";
        public string lorem_ipsum = "lorem ipsum dolor sit amet blasphemous E1331";

        public Form1()
        {
            InitializeComponent();
            //  root_path = root2;
            //       root_path = Environment.CurrentDirectory;
      //      root_path = "C:\\Users\\KZE PC\\Desktop\\AWB";
        }


        public List<Order> ReadExcel(FileInfo excelFile)
        {

            Excel.Application app = new Excel.Application();
            List<Order> orders = new List<Order>();
            Workbook book = app.Workbooks.Open(excelFile.FullName);
            Worksheet sheet;
            try{
                sheet = book.Worksheets[1];
                textBox1.Text += "Opened Excel file\r\n";
                int row = 2;
                const string IDCOL = "A";
                const string AWBCOL = "C";
                const string NAMECOL = "D";
                const string QNTCOL = "G";


                Order lastOrder = new Order();
                while (true)
                {
                    string id = sheet.Cells[row, IDCOL].Value2;
                   

                    if (id != null)
                    {
                        string awb = sheet.Cells[row, AWBCOL].Value2;
                        string name = sheet.Cells[row, NAMECOL].Value2;
                        name = name.Remove(name.Length - 32);
                        int qnt = (int)sheet.Cells[row, QNTCOL].Value2;


                        if (id == lastOrder.id)
                        {
                            lastOrder.toppere.Add(new Order.topper(name, qnt));
                        }
                        else
                        {
                            if(lastOrder.id!="")
                                orders.Add(lastOrder);
                            lastOrder =new Order(id,awb,name,qnt);
                        }
                    }
                    else
                    {
                        orders.Add(lastOrder);
                        break;
                    }
                    row++;
                }

                book.Close();
                app.Quit();
                textBox1.Text += "Closed Excel file.\r\n";
                return orders;
            }
            catch (Exception ex)
            {
                textBox1.Text += ex.Message;
                book.Close();
                throw ex;
            }
        }


        public class Order
        {
            public string id;
            public string awb;
            public struct topper
            {
                public string tName;
                public int tQuantity;
                public topper(string tName,int tQuantity)
                {
                    this.tName = tName;
                    this.tQuantity = tQuantity;
                }
            };
            public List<topper> toppere;

            public Order()
            {
                this.id = "";
                this.awb = "";
                this.toppere = new List<topper>();
            }

            public Order(string id, string awb, string tName,int tQuantity)
            {
                this.id = id;
                this.awb = awb;
                this.toppere = new List<topper>();
                toppere.Add(new topper(tName, tQuantity));
            }
        }


        public void WriteOnPage(List<Order.topper> toppere, PdfPage page)
     
        {

            XGraphics gfx = XGraphics.FromPdfPage(page);

            XFont font = new XFont("Times New Roman", 15);
            XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));

            XRect rect = new XRect(0, page.Height / 2 - 15, page.Width, page.Height / 2 + 15);
            gfx.DrawRectangle(XBrushes.White, rect);

            int i = 0;
            foreach(var topper in toppere)
            {
                gfx.DrawString(topper.tQuantity+" buc: "+topper.tName, font, brush,50,page.Height/2+150+15*(i++), XStringFormats.CenterLeft);

            }


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
                        zipPathBox.Text = zip.FullName;
                        break;
                    }
                }


                if (!found)
                {
                    textBox1.Text += "One zip file was found that was not extracted.\r\n" + zip.FullName + "\r\nExtracting it now.\r\n";
                    ZipArchive archive = new ZipArchive(new FileStream(zip.FullName, FileMode.Open), ZipArchiveMode.Read);
                    string extractedDir = zip.FullName.Replace(".zip", "");
                    archive.ExtractToDirectory(extractedDir);

                    List<Order> orders = ReadExcel(excel);
                    WriteOnDirectory(new DirectoryInfo(extractedDir), orders);
                }
            }
            if (noneFound)
            {
                textBox1.Text += "No zip file was found that was not already extracted.\r\n";
            }
            return noneFound;
        }
        public int failed = 0;

        private void WriteOnDirectory(DirectoryInfo pdfdir, List<Order> orders)
        {
            FileInfo[] files = pdfdir.GetFiles().Where(p => p.Extension == ".pdf").ToArray();
            textBox1.Text += files.Count() + " files found\r\n";

            PdfDocument doc = new PdfDocument();


            foreach(Order o in orders)
            {
                FileInfo pdfFile = null;
                try
                {
                     pdfFile = files.Where(p => p.Name.StartsWith(o.id) && p.Name.EndsWith(o.awb + "001.pdf")).ToArray()[0];
                }
                catch (IndexOutOfRangeException)
                {
                    failed++;
                    textBox1.Text += "Order \"" + o.id + "\" not found in pdfs.\r\n";
                }catch(Exception ex)
                {
                    textBox1.Text += ex.Message+"\r\n";
                }

                PdfDocument ipdf = PdfReader.Open(pdfFile.FullName, PdfDocumentOpenMode.Import);
                doc.AddPage(ipdf.Pages[0]);
                PdfPage page = doc.Pages[doc.PageCount-1];
                WriteOnPage(o.toppere, page);
            }
            doc.Save(pdfdir.FullName + "\\Merged&Filled.pdf");
            doc.Close();
            Process.Start(pdfdir.FullName + "\\Merged&Filled.pdf");

            if (failed > 0)
            {
                textBox1.Text += failed + " PDF files have failed, please look into them.\r\n";
            }
            else
            {
                textBox1.Text += "All PDF files were filled, the merged PDF should open.\r\n";
            }


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
            textBox1.Text += "Most recent excel file found at: \r\n" + details.FullName + "\r\n";


            textBox1.Text += "Created on: \r\n" + details.CreationTime + "\" or \r\n";
            TimeSpan span = DateTime.Now - details.CreationTime;
            if (span.TotalDays > 1)
            {
                textBox1.Text +=Math.Round(span.TotalDays) + "days ago. \r\n";
            }
            else
            {
                if(span.Hours>0)
                {
                    textBox1.Text += Math.Round(span.TotalHours) + " hours and ";
                }
                textBox1.Text += span.Minutes + " minutes ago. \r\n";

            }
            return details;
        }




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
                textBox1.Text += "EMPTY DIRECTORY, NOTHING TO CHECK\r\n";

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
            excelPathBox.Text = excel.FullName;

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



        private void zipButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Please select order zip archive.",
                DefaultExt = ".zip",
                InitialDirectory = workdir_path,
                RestoreDirectory = true,
            };
            DialogResult res = ofd.ShowDialog();

            switch (res)
            {
                case DialogResult.OK:
                    zipPathBox.Text = zip_path = ofd.FileName;
                    textBox1.Text += "Found zip archive at:\r\n" + zip_path + "\r\n";
                    zipLabel.Font = new System.Drawing.Font(zipLabel.Font, FontStyle.Regular);
                    fileCountLabel.Visible = false;


                    break;
                default:
                    break;
            }

        }

        private void zipPathBox_DoubleClick(object sender, EventArgs e)
        {
            tabControlMenu.SelectedTab = filePage;
            zipButton.PerformClick();
        }

        private void excelPathBox_DoubleClick(object sender, EventArgs e)
        {
            tabControlMenu.SelectedTab = filePage;
            excelButton.PerformClick();
        }

        private void excelButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog() {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Please select order summary excel file.",
                DefaultExt = ".xlsx",
                InitialDirectory = workdir_path,
                RestoreDirectory = true,
            };
            DialogResult res = ofd.ShowDialog();

            switch (res)
            {
                case DialogResult.OK:
                    excelPathBox.Text = excel_path = ofd.FileName;
                    textBox1.Text += "Found excel summary at:\r\n" + excel_path + "\r\n";
                    break;
                default:
                    break;
            }


        }

        private void unzippedButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                Multiselect = true,
                Title = "Please select all your unzipped files.",
                DefaultExt = ".pdf",
                InitialDirectory = workdir_path,
                RestoreDirectory = true
            };
            DialogResult res = ofd.ShowDialog();

            switch (res)
            {
                case DialogResult.OK:
                    zipLabel.Font = new System.Drawing.Font(zipLabel.Font, FontStyle.Strikeout);
                    unzipped_list.Clear();
                    fileCountLabel.Visible = true;
                    foreach(string fname in ofd.FileNames)
                    {
                        FileInfo t = new FileInfo(fname);
                        unzipped_list.Add(t);
                        textBox1.Text += "Selected " + t.Name + "\r\n";



                    }
                    zipPathBox.Text = unzipped_list[0].Name;
                    fileCountLabel.Text = unzipped_list.Count + "file";
                    if (unzipped_list.Count > 1)
                    {
                        zipPathBox.Text+=" +" + (unzipped_list.Count - 1) + " others";
                        fileCountLabel.Text +="s";

                    }
                    break;
                default:
                    break;
            }
        }


        private void emagBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://emag.ro");

        }


        private void CelBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://cel.ro");

        }

        private void SamedayBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://eawb.sameday.ro");

        }

        private void rootButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Please select the new root directory.",
                DefaultExt = ".xlsx",
                InitialDirectory = workdir_path,
                RestoreDirectory = true,
            };
            DialogResult res = ofd.ShowDialog();

            switch (res)
            {
                case DialogResult.OK:
                    excelPathBox.Text = excel_path = ofd.FileName;
                    textBox1.Text += "Found excel summary at:\r\n" + excel_path + "\r\n";
                    break;
                default:
                    break;
            }

        }
    }
}

