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
using Microsoft.Office.Interop;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.IO.Compression;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;




namespace PDFiller
{
   internal partial class Form1 : Form
    {
       
        public Form1()
        {
            InitializeComponent();
        }




   

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
        //    Menu menu = PDFiller.Menu.getInstance(this);
            if (autoFillCheck.Checked)
            {
                AutoFill();
            }
        }



        private void zipButton_Click(object sender, EventArgs e)
        {
            Menu menu = PDFiller.Menu.getInstance();
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Zip files (*.zip)|*.zip|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Please select order zip archive.",
                DefaultExt = ".zip",
                InitialDirectory = menu.workDir.FullName,
                RestoreDirectory = true,
            };

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    zipPathBox.Text  = ofd.FileName;
                    menu.zip = new FileInfo(ofd.FileName);
                    textBox1.Text += "Found zip archive at:\r\n" + menu.zip.FullName + "\r\n";
                    zipLabel.Font = new System.Drawing.Font(zipLabel.Font, FontStyle.Regular);
                //    f.Visible = false;
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
            Menu menu = PDFiller.Menu.getInstance();

            OpenFileDialog ofd = new OpenFileDialog() {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Please select order summary excel file.",
                DefaultExt = ".xlsx",
                InitialDirectory = menu.workDir.FullName,
                RestoreDirectory = true,
            };
      
            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    excelPathBox.Text = ofd.FileName;
                    menu.excel = new FileInfo(ofd.FileName);
                    textBox1.Text += "Found excel summary at:\r\n" + menu.excel.FullName + "\r\n";
                    break;
                default:
                    break;
            }


        }

        private void unzippedButton_Click(object sender, EventArgs e)
        {
            Menu menu = PDFiller.Menu.getInstance();
            List<FileInfo> unzippedLists = menu.unzippedList;

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                Multiselect = true,
                Title = "Please select all your unzipped files.",
                DefaultExt = ".pdf",
                InitialDirectory = menu.workDir.FullName,
                RestoreDirectory = true
            };

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    zipLabel.Font = new System.Drawing.Font(zipLabel.Font, FontStyle.Strikeout);
                    menu.unzippedList.Clear();
                    foreach(string fname in ofd.FileNames)
                    {
                        FileInfo t = new FileInfo(fname);
                        menu.unzippedList.Add(t);
                        textBox1.Text += "Selected " + t.Name + "\r\n";

                    }
                    zipPathBox.Text = menu.unzippedList[0].Name;
                    zipLabel.Text = unzippedLists.Count + "file";
                    if (unzippedLists.Count > 1)
                    {
                        zipPathBox.Text+=" +" + (unzippedLists.Count - 1) + " others";
                        zipLabel.Text +="s";

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
            Menu menu = PDFiller.Menu.getInstance();
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.RootFolder = Environment.SpecialFolder.MyComputer;
            ofd.Description = "This where you'd make folders daily:\r\n" +
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\AWB\r\n" +
                "and today you'd create:" +
                DateTime.Now.Date.ToString("dd.MM.yyyy")+"\\your pdfs.";
            ofd.ShowNewFolderButton = true;

                //   string root2 = "C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\AWBFiller\\bin\\Debug\\";

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    menu.rootDir = new DirectoryInfo(ofd.SelectedPath);
                    textBox1.Text += "New root folder set at:\r\n" + (menu.rootDir.FullName) + "\r\n";

                    break;
                default:
                    break;
            }

        }

        private void rootButton_MouseHover(object sender, EventArgs e)
        {
    

        }

        private void workButton_Click(object sender, EventArgs e)
        {
            Menu menu = PDFiller.Menu.getInstance();
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "This is where we will look the .zip and .excel files today!!\r\n"+
                              "Either use this or select said files manually.\r\n"+
                              "Press \"Fill&Merge\" when you're done";
            ofd.ShowNewFolderButton = true;


            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    menu.UpdateWorkDir(ofd.SelectedPath);
                    textBox1.Text += "New root folder set at:\r\n" + (menu.workDir.FullName) + "\r\n";
                    break;
                default:
                    break;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            Menu menu = PDFiller.Menu.getInstance();
            //apply conditions for paths
         //   menu.MergeFill();
        }


        private void mergeFillButton_Click(object sender, EventArgs e)
        {
            Menu menu = PDFiller.Menu.getInstance();
            FileInfo pdf = menu.excel;
            if (pdf == null || !pdf.Exists) {
                throw new FileNotFoundException("PDF could not be found, something happened to it",pdf.FullName);
            }
            List<FileInfo> orders = menu.unzippedList;
            if(orders == null ||  orders.Count == 0 )
            {
               // orders = menu.
             //   throw new Exception("There are no orders to be processed");
            }
        }

        private void AutoFill()
        {
            try
            {
                Menu menu = PDFiller.Menu.getInstance(this);
                DirectoryInfo workDir = menu.UpdateWorkDir();
                FileInfo excel = menu.FindExcel(workDir);
                FileInfo zip = menu.FindZipsUnzipped(workDir);
                string extractedDir = null;
                List<FileInfo> unzippedList = menu.UnzipArchive(zip,ref extractedDir);
                List<Order> orders = menu.ReadExcel(excel);
                string path = menu.WriteOnOrders(unzippedList, orders,extractedDir);
                if (openPdfCheck.Checked) Process.Start(path);
            }
            catch (Exception ex)
            {
                textBox1.Text += ex.Message;
            }
        }

        private void autoFillBtn_Click(object sender, EventArgs e)
        {

            AutoFill();
        //    if(autoFillCheck.Checked) //print 
        }
    }
}

