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
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data.Odbc;





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
        private List<Order> orders = null;
        private string mergedPath = null;

        private void writeOptions()
        {
            StreamWriter sw = new StreamWriter(new FileStream("options.ini", FileMode.OpenOrCreate, FileAccess.Write));
            sw.WriteLine("root=" + rootDir.FullName);
            sw.WriteLine("autofill=" + autoFillCheck.Checked);
            sw.WriteLine("print=" + PrintCheck.Checked);
            sw.WriteLine("open=" + openPdfCheck.Checked);
            sw.Close();
        }


        async public void SecondForm()
        {

        }
        private void Form1_Load(object sender, EventArgs e)
        {


            StreamReader sr = null;
            if (File.Exists("options.ini"))
            {
                sr = new StreamReader(new FileStream("options.ini", FileMode.Open, FileAccess.Read));
            }
            else
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB";
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                rootDir = new DirectoryInfo(path);
                rootTextBox.Text = rootDir.FullName;
                writeOptions();
                return;
            }
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split("=".ToCharArray(), 2);

                switch (line[0])
                {
                    case "root":
                        rootDir = new DirectoryInfo(line[1]);
                        textBox1.Text += "Root directory found at:\r\n" + line[1] + "\r\n";
                        rootTextBox.Text = line[1];
                        break;
                    case "autofill":
                        bool fill = true;
                        if (Boolean.TryParse(line[1], out fill))
                        {
                            autoFillCheck.Checked = fill;
                        }
                        else
                        {
                            autoFillCheck.Checked = true;
                        }
                        break;
                    case "print":
                        bool print = true;
                        if (Boolean.TryParse(line[1], out print))
                        {
                            PrintCheck.Checked = print;
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

        private void HelpMeOut()
        {
            Builder builder = Builder.getInstance();
            manualSelect = true;
            workDir = new DirectoryInfo("C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\PDFiller\\bin\\Debug\\debugTests\\");
            unzippedList = new List<FileInfo>() { new FileInfo("C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\PDFiller\\bin\\Debug\\debugTests\\\\417264331_Sameday_4EMG24107789758001.pdf") };
            excel = builder.FindExcel(workDir);
            var orders = builder.ReadExcel(excel);
            int failed;
            string resPath = builder.WriteOnOrders(unzippedList, orders, workDir.FullName, out failed, "ROBLOX_IMAGE_TEST");
            Process.Start(resPath);

        }



        private void Form1_Shown(object sender, EventArgs e)
        {

            Builder menu = PDFiller.Builder.getInstance(this);

            HelpMeOut();    







            if (autoFillCheck.Checked)
            {
                AutoFill();
            }
        }




        private void zipButton_Click(object sender, EventArgs e)
        {
            Builder menu = PDFiller.Builder.getInstance();
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
                    zipPathBox.Text = ofd.FileName;
                    zip = new FileInfo(ofd.FileName);
                    unzippedList = null ;
                    textBox1.Text += "Found zip archive at:\r\n" + zip.FullName + "\r\n";
                    zipLabel.Font = new System.Drawing.Font(zipLabel.Font, FontStyle.Regular);
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
     //   internal bool newExcel = false;
        private void excelButton_Click(object sender, EventArgs e)
        {
            Builder menu = PDFiller.Builder.getInstance();

            OpenFileDialog ofd = new OpenFileDialog()
            {
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


                    this.excel = new FileInfo(ofd.FileName);
                    textBox1.Text += "Found excel summary at:\r\n" + excel.FullName + "\r\n";
                    excelGridView.Rows.Clear();
                    summaryGridView.Rows.Clear();
                    updateTabIndex(true);
                    break;
                default:
                    break;
            }

        }


        private bool manualSelect = false;
        private void unzippedButton_Click(object sender, EventArgs e)
        {
         

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                Multiselect = true,
                Title = "Please select all your unzipped files.",
                DefaultExt = ".pdf",
                //  InitialDirectory = envi.FullName,
                RestoreDirectory = true
            };
            Builder menu = PDFiller.Builder.getInstance();
            zip = null;
            unzippedList = new List<FileInfo>();

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    manualSelect = true;
                    foreach (string fname in ofd.FileNames)
                    {
                        FileInfo t = new FileInfo(fname);
                        unzippedList.Add(t);
                        textBox1.Text += "Selected " + t.Name + "\r\n";

                    }
                    zipPathBox.Text = unzippedList[0].Name;
                    zipLabel.Text = unzippedList.Count + " file";
                    if (unzippedList.Count > 1)
                    {
                        zipPathBox.Text += " + " + (unzippedList.Count - 1) + " others";
                        zipLabel.Text += "s";

                    }
                    break;
                default:
                    break;
            }
        }


        private void emagBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://marketplace.emag.ro/order/list-xb");

        }


        private void CelBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://marketplace.cel.ro/vanzari/comenzi");

        }

        private void SamedayBtn_Click(object sender, EventArgs e)
        {
            Process.Start("https://eawb.sameday.ro/awb");

        }

        private void rootButton_Click(object sender, EventArgs e)
        {


            //CommonOpenFileDialog rootDirDialogue = new CommonOpenFileDialog();
            //rootDirDialogue.IsFolderPicker = true;
            //rootDirDialogue.Multiselect = false;
            //rootDirDialogue.DefaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            //rootDirDialogue.EnsurePathExists = true;
            //rootDirDialogue.NavigateToShortcut = true;

            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.RootFolder = Environment.SpecialFolder.MyComputer;
            ofd.Description = "This where you'd make folders daily:\r\n" +
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\r\n" +
                "and today you'd create:" +
                DateTime.Now.Date.ToString("dd.MM.yyyy") + "\\your pdfs.";
            ofd.ShowNewFolderButton = true;


            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    rootDir = new DirectoryInfo(ofd.SelectedPath);
                    textBox1.Text += "New root folder set at:\r\n" + (rootDir.FullName) + "\r\n";
                    rootTextBox.Text = ofd.SelectedPath;
                    StreamWriter sw = new StreamWriter(new FileStream("options.ini", FileMode.OpenOrCreate, FileAccess.Write));
                    sw.WriteLine("root=" + ofd.SelectedPath);
                    sw.Close();
                    break;
                default:
                    break;
            }

        }


        private void workButton_Click(object sender, EventArgs e)
        {
            Builder menu = PDFiller.Builder.getInstance();
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "This is where we will look the .zip and .excel files today!!\r\n" +
                              "Either use this or select said files manually.\r\n" +
                              "Press \"Fill&Merge\" when you're done";
            ofd.ShowNewFolderButton = true;


            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    workDir = menu.FindWorkDir(ofd.SelectedPath);
                    textBox1.Text += "New work directory set at:\r\n" + (workDir.FullName) + "\r\n";
                    try
                    {
                        excel = menu.FindExcel(workDir);
                        excelPathBox.Text = excel.FullName;

                        excelGridView.Rows.Clear();
                        summaryGridView.Rows.Clear();
                        updateTabIndex(true);


                        zip = menu.FindZipsUnzipped(workDir);
                        zipPathBox.Text = zip.FullName;
                        



                    }
                    catch (Exception ex)
                    {
                        textBox1.Text += ex.Message;
                    }
                    break;
                default:
                    break;
            }
        }



        private void mergeFillButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (excel == null || !excel.Exists)
                {
                    throw new FileNotFoundException("Excel could not be found.");
                }
                string saveDir = null;
                Builder menu = PDFiller.Builder.getInstance();
                if (zip!=null && unzippedList == null)
                {
                    if (!zip.Exists)
                    {
                        throw new FileNotFoundException("Zip archive could not be found.");
                    }
                    menu = PDFiller.Builder.getInstance();

                    unzippedList = menu.UnzipArchive(zip, ref saveDir);
                    textBox1.Text += $"Extracted archive: {zip.Name}\r\n";
                    textBox1.Text += $"Extracted {unzippedList.Count} orders.\r\n";
                }


                orders = menu.ReadExcel(excel);
                saveDir = unzippedList.First().DirectoryName;
                int failed = 0;
                string path = mergedPath = menu.WriteOnOrders(unzippedList, orders, saveDir, out failed, "CustomPDF");
                if (failed > 0)
                {
                    textBox1.Text += failed + " files failed being filled.\r\n";
                }
                else
                {
                    textBox1.Text += "All were filled succesfully.\r\n";
                }
                textBox1.Text += "Merged order PDF was saved at location:\r\n" + path + "\r\n";
                if (tabControl2.SelectedIndex == 1) updateTabIndex();

                if (openPdfCheck.Checked)
                {
                    textBox1.Text += "The pdf should open about now:\r\n";
                    Process.Start(path);
                }
            }
            catch (Exception ex)
            {
                textBox1.Text += ex.Message + "\r\n";
                return;
            }

        }

        private void AutoFill()
        {
            try
            {
                manualSelect = false;
                Builder menu = PDFiller.Builder.getInstance(this);
                workDir = menu.FindWorkDir(rootDir);
                textBox1.Text += $"Found work directory at:\r\n{workDir.FullName}\r\n";
                zip = menu.FindZipsUnzipped(workDir);
                textBox1.Text += $"Found zip archive at:\r\n{zip.FullName}\r\n";
                zipPathBox.Text = zip.FullName;
                string extractedDir = null;
                unzippedList = menu.UnzipArchive(zip, ref extractedDir);
                textBox1.Text += $"Found {unzippedList.Count} orders.\r\n";

                excel = menu.FindExcel(workDir);
                excelGridView.Rows.Clear();
                summaryGridView.Rows.Clear();


                textBox1.Text += "Found excel file at:\r\n" + excel.FullName + "\r\n";
                excelPathBox.Text = excel.FullName;
                orders = menu.ReadExcel(excel);
                updateTabIndex(false);



                int failed;
                string path = mergedPath = menu.WriteOnOrders(unzippedList, orders, extractedDir, out failed, "Merged&Filled");
                if (failed > 0)
                {
                    textBox1.Text += failed + " files failed being filled, please check them.\r\n";
                }
                else
                {
                    textBox1.Text += "All pdfs completed and merged with success.\r\n";
                }
                textBox1.Text += $"Merged pdf was saved at \r\n{path}\r\n";
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
         //   if (tabControl2.SelectedIndex == 1) updateTabIndex(); 
         //why tf did i put that there?
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {
        }


        private void updateTabIndex(bool readOrders=false)
        {
            switch (tabControl2.SelectedIndex)
            {
                case 0:
             //       if (mergedPath == null || (chromiumWebBrowser1.Address!= null && mergedPath != chromiumWebBrowser1.Address)) return;
               //     chromiumWebBrowser1.LoadUrlAsync(mergedPath);
                    break;
                case 1:
                    if(this.excel == null || excelGridView.Rows.Count > 0) return;
                    if (readOrders)
                    {
                        Builder menu = PDFiller.Builder.getInstance();
                        this.orders = menu.ReadExcel(excel);
                    }
                    var rows = excelGridView.Rows;
                    foreach (Order o in orders)
                    {
                        rows.Add(o.name, o.toppere[0].tName, o.toppere[0].tQuantity);
                        foreach (Order.topper tp in o.toppere.GetRange(1, o.toppere.Count - 1))
                        {
                            rows.Add(null, tp.tName, tp.tQuantity);
                        }
                    }
                    break;
                case 2:

                    if (this.excel == null || summaryGridView.Rows.Count > 0 ) return;
                    if (readOrders)
                    {
                        Builder menu = PDFiller.Builder.getInstance();
                        this.orders = menu.ReadExcel(excel);
                    }
                    rows = summaryGridView.Rows;

                    Dictionary<string, int> dict = new Dictionary<string, int>();

                    foreach (Order o in orders)
                    {
                        foreach (Order.topper tp in o.toppere)
                        {
                            if (dict.ContainsKey(tp.tName))
                            {
                                dict[tp.tName] += tp.tQuantity;

                            }
                            else
                            {
                                dict[tp.tName] = tp.tQuantity;
                            }
                        }
                    }
                    foreach(var pair in dict)
                    {
                        summaryGridView.Rows.Add(pair.Key, pair.Value);
                    }
                    summaryGridView.Sort(summaryGridView.Columns[1], ListSortDirection.Descending);

                    break;
                default:
                    return;
            }
        }


        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateTabIndex(true);
        }

        private void excelTab_Click(object sender, EventArgs e)
        {

        }

   

        private void groupBox8_Enter(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void rootTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void rootTextBox_DoubleClick(object sender, EventArgs e)
        {
            Process.Start(rootTextBox.Text);
        }



        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

