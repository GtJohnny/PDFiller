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

        private DirectoryInfo rootDir = null;
        private DirectoryInfo workDir = null;
        private FileInfo zip = null;
        private FileInfo excel = null;
        private List<FileInfo> unzippedList = null;

        private void writeOptions()
        {//for update
            StreamWriter sw = new StreamWriter(new FileStream("options.ini", FileMode.OpenOrCreate, FileAccess.Write));
            sw.WriteLine("root=" + rootDir.FullName);
            sw.WriteLine("autofill=" + autoFillCheck.Checked);
            sw.WriteLine("print=" + PrintCheck.Checked);
            sw.WriteLine("open=" + openPdfCheck.Checked);
            sw.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = null;
            if(File.Exists("options.ini"))
            {
                sr = new StreamReader(new FileStream("options.ini", FileMode.Open, FileAccess.Read));
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB";
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                rootDir = new DirectoryInfo(path);
                rootTextBox.Text = rootDir.FullName;
                writeOptions();
            }
            while (!sr.EndOfStream) {
                string[] line = sr.ReadLine().Split("=".ToCharArray(), 2);

                switch (line[0])
                {
                    case "root":
                        rootDir = new DirectoryInfo(line[1]);
                        textBox1.Text += "Root directory found at:\r\n" + rootDir.FullName + "\r\n";
                        break;
                    case "autofill":
                        bool fill = true;
                        if (Boolean.TryParse(line[1],out fill))
                        {
                            autoFillCheck.Checked = fill;
                        }
                        else
                        {
                            autoFillCheck.Checked= true;
                        }
                        break;
                    case "print":
                        bool print = true;
                        if (Boolean.TryParse(line[1], out print))
                        {
                            PrintCheck.Checked =print;
                        }
                        else
                        {
                            PrintCheck.Checked = true;
                        }
                        break;
                    case "open":
                        bool res = true;
                        if (Boolean.TryParse(line[1], out res))
                        {
                            openPdfCheck.Checked = res;
                        }
                        else
                        {
                            openPdfCheck.Checked = true;
                        }
                        break;
                    case "":
                        break;
                    default:
                        sr.Close();
                        throw new Exception("options.ini was corrupted");
                }
            }
            sr.Close();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            Menu menu = PDFiller.Menu.getInstance(this);

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
                InitialDirectory = rootDir.FullName,
                RestoreDirectory = true,
            };

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    manualSelect = false;
                    zipPathBox.Text  = ofd.FileName;
                    zip = new FileInfo(ofd.FileName);
                    textBox1.Text += "Found zip archive at:\r\n" + zip.FullName + "\r\n";
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
                InitialDirectory = rootDir.FullName,
                RestoreDirectory = true,
            };
      
            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    excelPathBox.Text = ofd.FileName;
                    excel = new FileInfo(ofd.FileName);
                    textBox1.Text += "Found excel summary at:\r\n" + excel.FullName + "\r\n";
                    break;
                default:
                    break;
            }


        }

        private bool manualSelect = false;
        private void unzippedButton_Click(object sender, EventArgs e)
        {
            Menu menu = PDFiller.Menu.getInstance();
            zip = null;
            unzippedList = new List<FileInfo>();

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                Multiselect = true,
                Title = "Please select all your unzipped files.",
                DefaultExt = ".pdf",
              //  InitialDirectory = envi.FullName,
                RestoreDirectory = true
            };

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    manualSelect = true;
                    foreach(string fname in ofd.FileNames)
                    {
                        FileInfo t = new FileInfo(fname);
                        unzippedList.Add(t);
                        textBox1.Text += "Selected " + t.Name + "\r\n";

                    }
                    zipPathBox.Text = unzippedList[0].Name;
                    zipLabel.Text = unzippedList.Count + " file";
                    if (unzippedList.Count > 1)
                    {
                        zipPathBox.Text+=" +" + (unzippedList.Count - 1) + " others";
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
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.RootFolder = Environment.SpecialFolder.MyComputer;
            ofd.Description = "This where you'd make folders daily:\r\n" +
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop)+"\\AWB\r\n" +
                "and today you'd create:" +
                DateTime.Now.Date.ToString("dd.MM.yyyy")+"\\your pdfs.";
            ofd.ShowNewFolderButton = true;

         
            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    rootDir = new DirectoryInfo(ofd.SelectedPath);
                    textBox1.Text += "New root folder set at:\r\n" + (rootDir.FullName) + "\r\n";
                    rootTextBox.Text = ofd.SelectedPath;
                    StreamWriter sw = new StreamWriter(new FileStream("options.ini", FileMode.OpenOrCreate,FileAccess.Write));
                    sw.WriteLine("root=" + ofd.SelectedPath);
                    sw.Close();
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
                    workDir = menu.FindWorkDir(ofd.SelectedPath);
                    textBox1.Text += "New work directory set at:\r\n" + (workDir.FullName) + "\r\n";
                    excel = menu.FindExcel(workDir);
                    zip = menu.FindZipsUnzipped(workDir);
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
        //TODO

        private void mergeFillButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (excel == null || !excel.Exists)
                {
                    throw new FileNotFoundException("Excel could not be found, something happened to it.");
                }
                Menu menu = null; ;
                string saveDir =null;
                if (!manualSelect)
                {
                    if(zip == null|| !zip.Exists)
                    {
                        throw new FileNotFoundException("Zip archive could not be found, something happened to it.");
                    }
                    menu = PDFiller.Menu.getInstance();
                    unzippedList = menu.UnzipArchive(zip,ref saveDir);
                    textBox1.Text += "Extracted archive: " + zip.Name + "\r\n";
                    textBox1.Text += "Extracted " + unzippedList.Count + " files.\r\n";
;               }

                foreach (FileInfo pdf in unzippedList)
                {
                    if (!pdf.Exists)
                    {
                        textBox1.Text += pdf.Name + "could not be found, something happened to it.";
                        unzippedList.Remove(pdf);
                    }
                }
                menu = PDFiller.Menu.getInstance();
                saveDir = excel.DirectoryName;
                List<Order> orders = menu.ReadExcel(excel);
                int failed = 0;
                
                string path = menu.WriteOnOrders(unzippedList, orders, saveDir,out failed);
                textBox1.Text += "Merged order PDF was saved at location:\r\n" + saveDir + "\r\n" ;
                if (openPdfCheck.Checked)
                {
                    textBox1.Text += "The pdf should open about now:\r\n";
                    Process.Start(path);

                }
            } catch (Exception ex) {
                textBox1.Text += ex.Message + "\r\n";
                return;
            }

        }

        private void AutoFill()
        {
            try
            {
                manualSelect = false;
                Menu menu = PDFiller.Menu.getInstance(this);
                workDir = menu.FindWorkDir(rootDir);
                textBox1.Text += "Found work directory at:\r\n" + workDir.FullName + "\r\n";
                excel = menu.FindExcel(workDir);
                textBox1.Text += "Found excel file at:\r\n" + excel.FullName + "\r\n";
                excelPathBox.Text = excel.FullName;
                zip = menu.FindZipsUnzipped(workDir);
                textBox1.Text += "Found zip archive at:\r\n" + zip.FullName + "\r\n";
                zipPathBox.Text = zip.FullName;
                string extractedDir = null;
                unzippedList = menu.UnzipArchive(zip,ref extractedDir);
                textBox1.Text += "Found " + unzippedList.Count + " orders.\r\n";
                List<Order>orders = menu.ReadExcel(excel);
                int failed;
                string path = menu.WriteOnOrders(unzippedList, orders,extractedDir,out failed);
                if (failed > 0)
                {
                    textBox1.Text += failed + " files failed being filled, please check them.\r\n";
                }
                else
                {
                    textBox1.Text += "All pdfs completed and merged with success.\r\n";
                }
                textBox1.Text += "Merged pdf was saved at " + path + "\r\n";
                if (openPdfCheck.Checked)
                {
                    textBox1.Text += "It should open about now.\r\n";
                    Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += ex.Message;
            }
        }

        private void autoFillBtn_Click(object sender, EventArgs e)
        {

            AutoFill();
        }



        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            writeOptions();
        }

        private void openPdfCheck_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tabControlMenu_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

