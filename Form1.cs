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
using System.Windows.Forms;
using Microsoft.WindowsAPICodePack.Dialogs.Controls;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Data.Odbc;
using System.Threading;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.SqlClient;




namespace PDFiller
{
    internal partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private string DebugPath = Environment.CurrentDirectory + "\\debugTests\\";

        private DirectoryInfo rootDir = null;
        private DirectoryInfo workDir = null;
        private FileInfo zip = null;
        private FileInfo excel = null;
        private List<FileInfo> unzippedList = null;
        private Shipment shipment = new Shipment();







        private void writeOptions()
        {
            StreamWriter sw = new StreamWriter(new FileStream("options.ini", FileMode.OpenOrCreate, FileAccess.Write));
            sw.WriteLine("root=" + rootDir.FullName);
            sw.WriteLine("autofill=" + autoFillCheck.Checked);
            sw.WriteLine("open=" + openPdfCheck.Checked);
            sw.WriteLine("draw=" + drawComboBox.SelectedIndex);
            sw.Close();
        }

        private void readOptions()
        {
            StreamReader sr = new StreamReader(new FileStream("options.ini", FileMode.Open, FileAccess.Read));
            while (!sr.EndOfStream)
            {
                string[] line = sr.ReadLine().Split("=".ToCharArray(), 2);

                switch (line[0])
                {
                    case "root":
                        rootDir = new DirectoryInfo(line[1]);
                        textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nRoot directory found at:\r\n{line[1]}\r\n");
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
                    case "draw":
                        int draw = 0;
                        if (int.TryParse(line[1], out draw))
                        {
                            drawComboBox.SelectedIndex = draw;
                        }
                        else
                        {
                            drawComboBox.SelectedIndex = 2;
                        }
                        break;
                    case "":
                        break;
                    default:
                        sr.Close();
                        textBox1.AppendText("Options.ini was corrupted, rewriting it");
                        File.Delete("options.ini");
                        drawComboBox.SelectedIndex = 2;
                        string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB";
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }
                        rootDir = new DirectoryInfo(path);

                        writeOptions();

                        break;
                }
            }
            sr.Close();
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            this.versionLabel.Text = "v1.4.2";

            this.shipment.Subscribe(new SummaryObserver(summaryGridView));
            this.shipment.Subscribe(new PreviewObserver(previewGridView));
            this.shipment.Subscribe(new ImagesObserver(imagePanel));
            if (File.Exists("options.ini"))
            {
                readOptions();
                return;
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
                if (drawComboBox.SelectedIndex == -1) drawComboBox.SelectedIndex = 2;
                writeOptions();
                return;
            }



        }


        private void Form1_Shown(object sender, EventArgs e)
        {
            textBox1.AppendText("Please select a zip archive, an excel file," +
               " or a handful of awbs to begin!\r\n" +
               "You can select them automatically, manually, Drag & Drop " +
               "or Copy & Paste them directly into this textbox!\r\n");

            if (autoFillCheck.Checked)
            {

                AutoFill();
            }
           

        }




        private void zipButton_Click(object sender, EventArgs e)
        {
            Builder menu = PDFiller.Builder.GetInstance();
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
                    //manualSelect = false;
                    zipPathBox.Text = ofd.FileName;
                    this.zip = new FileInfo(ofd.FileName);
                    unzippedList = null;
                    textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nFound zip archive at:\r\n {zip.FullName} \r\n");
                    zipLabel.Text = "Zip File:";
                    break;
                default:
                    break;
            }
        }

        private void zipPathBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.zip != null && this.zip.Exists)
            {
                Process.Start(this.zip.DirectoryName);
            }
        }

        private void excelPathBox_DoubleClick(object sender, EventArgs e)
        {
            if (this.excel != null && this.excel.Exists)
            {
                Process.Start(this.excel.FullName);
            }
        }
        //   internal bool newExcel = false;
        private void excelButton_Click(object sender, EventArgs e)
        {

            Builder menu = PDFiller.Builder.GetInstance();

            OpenFileDialog ofd = new OpenFileDialog()
            {
                Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*",
                Multiselect = false,
                Title = "Please select order summary excel file.",
                DefaultExt = ".xlsx",
                //      InitialDirectory = rootDir.FullName,
                RestoreDirectory = true,
            };

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    excelPathBox.Text = ofd.FileName;


                    this.excel = new FileInfo(ofd.FileName);
                    textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nFound.xlsx order summary at:\r\n" + excel.FullName + "\r\n");
                    if (this.tabControl2.SelectedIndex > 0 && this.tabControl2.SelectedIndex < 4)
                    {
                        this.shipment.Update(new Shipment(menu.ReadExcel(excel), unzippedList, this.shipment.MergedPDF));
                    }
                    else
                    {
                        this.shipment.NotifyCompleted();
                    }





                    //this.shipment.Update(new Shipment(menu.ReadExcel(excel), unzippedList, this.shipment.MergedPDF));

                    break;
                default:
                    break;
            }

        }


        // private bool manualSelect = false;
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
            Builder menu = PDFiller.Builder.GetInstance();
            zip = null;
            unzippedList = new List<FileInfo>();

            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    //   manualSelect = true;
                    textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nSelected {ofd.FileNames.Count()} unzipped files:\r\n");
                    foreach (string fname in ofd.FileNames)
                    {
                        FileInfo t = new FileInfo(fname);
                        unzippedList.Add(t);
                        textBox1.AppendText($"Selected {t.Name}\r\n");

                    }
                    zipPathBox.Text = unzippedList[0].Name;
                    zipLabel.Text = unzippedList.Count + " file";
                    if (unzippedList.Count > 1)
                    {
                        zipPathBox.AppendText($" + {unzippedList.Count - 1} others");
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
                    textBox1.AppendText($"New root folder set at:\r\n {rootDir.FullName} \r\n");
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
            Builder menu = PDFiller.Builder.GetInstance();
            FolderBrowserDialog ofd = new FolderBrowserDialog();
            ofd.Description = "This is where we will look the .zip and .excel files today!!\r\n" +
                              "Either use this or select said files manually.\r\n" +
                              "Press \"Fill&Merge\" when you're done";
            ofd.ShowNewFolderButton = true;


            switch (ofd.ShowDialog())
            {
                case DialogResult.OK:
                    workDir = menu.FindWorkDir(ofd.SelectedPath);
                    textBox1.AppendText($"New work directory set at:\r\n{workDir.FullName}r\n");
                    try
                    {
                        excel = menu.FindExcel(workDir);
                        excelPathBox.Text = excel.FullName;

                        previewGridView.Rows.Clear();
                        summaryGridView.Rows.Clear();
                        //updateTabIndex();


                        zip = menu.FindZipsUnzipped(workDir);
                        zipPathBox.Text = zip.FullName;


                    }
                    catch (Exception ex)
                    {
                        textBox1.AppendText(ex.Message);
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
                Cursor.Current = Cursors.WaitCursor;

                string saveDir = null;
                Builder menu = PDFiller.Builder.GetInstance();
                if (zip != null && unzippedList == null)
                {
                    if (!zip.Exists)
                    {
                        throw new FileNotFoundException("Zip archive could not be found.");
                    }
                    unzippedList = menu.UnzipArchive(zip, out saveDir);
                    textBox1.AppendText($"Extracted archive: {zip.Name}\r\n");
                    textBox1.AppendText($"Extracted {unzippedList.Count} orders.\r\n");
                }

                if (unzippedList != null)
                {
                    saveDir = unzippedList.First().DirectoryName;
                }
                else
                {
                    throw new Exception("No zip archive or pdf files were selected. Nothing to fill.");
                }

                if (excel == null || !excel.Exists)
                {
                    throw new FileNotFoundException("Excel could not be found.");
                }

                if (shipment.Orders == null || shipment.Orders.Count == 0)
                {
                    textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nReading the excel file.\r\n");
                    this.shipment.Update(menu.WriteOnOrders(unzippedList, menu.ReadExcel(excel), saveDir, "CustomPDF"));
                }
                else
                {
                    this.shipment.Update(menu.WriteOnOrders(unzippedList, shipment.Orders, saveDir, "CustomPDF"));
                }
                //if (failed > 0)
                //{
                //    textBox1.AppendText( $"{failed} files failed being filled.\r\n";
                //}
                //else
                //{
                //    textBox1.AppendText( "All were filled succesfully.\r\n";
                //}
                textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nMerged order PDF was saved at location:\r\n{this.shipment.MergedPDF}\r\n");
                //if (tabControl2.SelectedIndex == 1) updateTabIndex();

                if (openPdfCheck.Checked)
                {
                    textBox1.AppendText("The pdf should open about now:\r\n");
                    Process.Start(this.shipment.MergedPDF);
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText(ex.Message + "\r\n");
                return;
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }




        }

        private void AutoFill()
        {
            try
            {
                textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]" +
                    "Autofilling...\r\n");
                Cursor.Current = Cursors.WaitCursor;
                Builder menu = PDFiller.Builder.GetInstance(this);
                workDir = menu.FindWorkDir(rootDir);
                textBox1.AppendText($"Found work directory at:\r\n{workDir.FullName}\r\n");
                zip = menu.FindZipsUnzipped(workDir);
                textBox1.AppendText($"Found zip archive at:\r\n{zip.FullName}\r\n");
                zipPathBox.Text = zip.FullName;
                string extractedDir = null;
                unzippedList = menu.UnzipArchive(zip, out extractedDir);
                textBox1.AppendText($"Found {unzippedList.Count} orders.\r\n");



                excel = menu.FindExcel(workDir);

                if (this.tabControl2.SelectedIndex > 0 && this.tabControl2.SelectedIndex < 4)
                {
                    this.shipment.Update(new Shipment(menu.ReadExcel(excel), unzippedList, this.shipment.MergedPDF));
                }
                else
                {
                    this.shipment.NotifyCompleted();
                }


                textBox1.AppendText($"Found excel file at:\r\n{excel.FullName}\r\n");
                excelPathBox.Text = excel.FullName;
                List<Order> orders = menu.ReadExcel(excel);
                //updateTabIndex();


                //   int failed = 0;
                this.shipment.Update(menu.WriteOnOrders(unzippedList, orders, extractedDir, "Merged&Filled"));
                //Shipment shipment = new Shipment(orders, unzippedList, extractedDir + "\\Merged&Filled");

                string mergedPath = shipment.MergedPDF;
                //string path = mergedPath = menu.WriteOnOrders(unzippedList, orders, extractedDir, "Merged&Filled");
                //if (failed > 0)
                //{
                //    textBox1.AppendText( failed + " files failed being filled, please check them.\r\n";
                //}
                //else
                //{
                //    textBox1.AppendText( "All pdfs completed and merged with success.\r\n";
                //}
                textBox1.AppendText($"Merged pdf was saved at \r\n{mergedPath}\r\n");
                if (openPdfCheck.Checked)
                {
                    textBox1.AppendText("It should open about now.\r\n");
                    Process.Start(mergedPath);
                }
            }
            catch (Exception ex)
            {
                textBox1.AppendText(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
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


        //private void updateTabIndex()
        //{
        //    switch (tabControl2.SelectedIndex)
        //    {
        //        case 0:
        //            //       if (mergedPath == null || (chromiumWebBrowser1.Address!= null && mergedPath != chromiumWebBrowser1.Address)) return;
        //            //     chromiumWebBrowser1.LoadUrlAsync(mergedPath);
        //            break;
        //        case 1: //Excel Preview
        //            //if (this.excel == null) return;
        //            if (this.newExcel || (this.excel != null && this.previewExcel != this.excel))
        //            {
        //                Builder menu = PDFiller.Builder.GetInstance();
        //                this.orders = menu.ReadExcel(excel);
        //                this.previewExcel = this.excel;
        //                this.newExcel = false;
        //            }
        //            else
        //            {
        //                return;
        //            }

        //                //if (this.previewExcel != null || this.excel == this.previewExcel) return;
        //                var rows = previewGridView.Rows;
        //            foreach (Order o in orders)
        //            {
        //                rows.Add(o.name, o.toppers[0].name, o.toppers[0].quantity);
        //                foreach (Order.topper tp in o.toppers.GetRange(1, o.toppers.Count - 1))
        //                {
        //                    rows.Add(null, tp.name, tp.quantity);
        //                }
        //            }
        //            break;
        //        case 2://ExcelSummary

        //            if (this.excel == null) return;
        //            if (this.summaryExcel != null || this.excel == this.summaryExcel) return;
        //            if (newExcel)
        //            {
        //                Builder menu = PDFiller.Builder.GetInstance();
        //                this.orders = menu.ReadExcel(excel);
        //                this.newExcel = false;
        //                this.summaryExcel = this.excel;
        //            }

        //            summaryGridView.Rows.Clear();

        //            //Dictionary<KeyValuePair<string,string>,int> dict = new Dictionary<KeyValuePair<string, string>, int>();
        //            Dictionary<string, int> dict = new Dictionary<string, int>();



        //            foreach (Order o in orders)
        //            {
        //                foreach (Order.topper tp in o.toppers)
        //                {
        //                    //KeyValuePair<string, string> key = new KeyValuePair<string, string>(tp.tName, tp.tId);
        //                    string key = tp.name;

        //                    if (dict.ContainsKey(key))
        //                    {
        //                        dict[key] += tp.quantity;
        //                    }
        //                    else
        //                    {
        //                        dict[key] = tp.quantity;
        //                    }
        //                }
        //            }
        //            foreach (var pair in dict)
        //            {
        //                //Bitmap img = Bitmap.FromFile(Environment.CurrentDirectory + "\\images\\" + pair.Key.Value + ".png") as Bitmap;
        //                //DataGridViewRow row = new DataGridViewRow();
        //                //row.CreateCells(summaryGridView, pair.Value, pair.Key.Key, img);
        //                //row.SetValues(pair.Value, pair.Key.Key, img);                        //pair.Value, pair.Key.Key, img,
        //                //row.Height = 100;
        //                summaryGridView.Rows.Add(pair.Value, pair.Key);

        //            }
        //            summaryGridView.Sort(summaryGridView.Columns[0], ListSortDirection.Descending);

        //            break;
        //        case 3://Imagini




        //        default:
        //            return;
        //    }
        //}



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

        private void button2_Click_1(object sender, EventArgs e)
        {


            //   drawComboBox.SelectedIndex = 1;
            Builder builder = Builder.GetInstance();
            builder.ZStartTest();


        }

        private void drawComboBox_DropDownClosed(object sender, EventArgs e)
        {
            this.ActiveControl = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.shipment == null) return;
            string mergedPath = this.shipment.MergedPDF;
            if (mergedPath != null && File.Exists(mergedPath))
                Process.Start(mergedPath);
        }

        private void drawComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }




        private void TestButtonClick(object sender, EventArgs e)
        {



            //Builder menu = PDFiller.Builder.GetInstance();
            //List<Order> orders = menu.ReadExcel(this.excel);
            //Dictionary<string, string> dict = new Dictionary<string, string>();



            SqlConnection conn = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\ionut\\source\\repos\\PDFiller\\Database1.mdf;Integrated Security=True");
            conn.Open();
            SqlCommand cmd = new SqlCommand("UPDATE TOPPERS SET NAME = @NAME WHERE ID = @ID;", conn);


            //cmd.CommandText = "UPDATE TOPPERS SET NAME = 'test' WHERE ID = '5941933302128';";
            //cmd.ExecuteNonQuery();

            Builder builder = Builder.GetInstance();

            Dictionary<string, string> dict = builder.GetDictionary();

            SqlParameter nameParam = new SqlParameter("NAME", SqlDbType.VarChar);
            SqlParameter idParam = new SqlParameter("ID", SqlDbType.Char);

            foreach (var pair in dict)
            {
                string key = pair.Key;
                string val = pair.Value;


                nameParam.Value = val;
                idParam.Value = key;
                cmd.Parameters.Add(nameParam);
                cmd.Parameters.Add(idParam);
                try
                {
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                    textBox1.AppendText(key + " -> " + val);
                }
                catch (Exception ex)
                {
                    textBox1.AppendText(ex.Message);
                    conn.Close();
                    return;
                }
            }
            conn.Close();
        }

        private void UpdateTabIndex()
        {

            switch (tabControl2.SelectedIndex)
            {
                case 0:
                    return;
                case 1:
                    if (previewGridView.Rows.Count == 0)
                    {
                        if (this.excel == null)
                        {
                            previewGridView.Rows.Add(null, null, "No orders to preview.");
                        }
                        else
                        {
                            Builder builder = Builder.GetInstance();
                            List<Order> orders = builder.ReadExcel(this.excel);
                            this.shipment.Update(new Shipment(orders, unzippedList, this.shipment.MergedPDF));
                        }
                    }
                    break;

                case 2:
                    if (summaryGridView.Rows.Count == 0)
                    {
                        if (this.excel == null)
                        {
                            summaryGridView.Rows.Add(null, "No orders to summarize.");
                        }
                        else
                        {
                            Builder builder = Builder.GetInstance();
                            List<Order> orders = builder.ReadExcel(this.excel);
                            this.shipment.Update(new Shipment(orders, unzippedList, this.shipment.MergedPDF));
                        }
                    }
                    break;

            }

            //if (tabControl2.SelectedIndex == 0) return;
            //if (this.shipment == null) return;
            //if (this.excel == null) return;
            //if (this.shipment.Orders == null) return;

            //Builder builder = Builder.GetInstance();
            //List<Order> orders = builder.ReadExcel(this.excel);
            //this.shipment.Update(new Shipment(orders, unzippedList, this.shipment.MergedPDF));

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start(Environment.CurrentDirectory);
        }



        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateTabIndex();
        }



        private void tabControl2_DragLeave(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.Default;
            textBox1.BackColor = SystemColors.Window;
        }



     
        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
                textBox1.BackColor = Color.LightGreen;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void textBox1_DragLeave(object sender, EventArgs e)
        {
            textBox1.BackColor = SystemColors.Window;
            Cursor = Cursors.Default;

        }

        /// <summary>
        /// Given a list of file paths, reads them and sets the zip, excel and unzippedList variables accordingly.
        /// Used as alternative to autofill or file dialogs.
        /// </summary>
        /// <param name="paths">Paths to all the input files given</param>
        private void ReadGivenFiles(string[] paths)
        {
            bool foundExcel = false;
            bool foundZip = false;
            bool foundPDF = false;

            byte failed = 0;
            textBox1.AppendText($"[{DateTime.Now.ToString("HH:mm:ss")}]\r\n");


            foreach (string path in paths)
            {
                FileInfo file = new FileInfo(path);

                if (file.Exists)
                {
                    switch (file.Extension)
                    {
                        case ".zip":
                            if (foundZip) //We will only process one zip file.
                            {
                                textBox1.AppendText($"Multiple zip files found, only the first will be used.\r\n");
                                continue;
                            }
                            zip = file;
                            zipLabel.Text = "Zip File:";
                            zipPathBox.Text = zip.FullName;
                            textBox1.AppendText($"Zip archive set to:\"{zip.Name}\"\r\n");
                            foundZip = true;
                            this.unzippedList = null;

                            break;
                        case ".xlsx":
                            if (foundExcel) //Same as zip file, only one
                            {
                                textBox1.AppendText($"Multiple excel files found, only the first will be used.\r\n");
                                continue;
                            }
                            this.excel = file;
                            excelPathBox.Text = excel.FullName;
                            textBox1.AppendText($"Excel file set to:\"{excel.Name}\"\r\n");
                            ///Remember to update statistics tabs if needed.
                            if (this.tabControl2.SelectedIndex > 0 && this.tabControl2.SelectedIndex < 4)
                            {
                                this.shipment.Update(new Shipment(Builder.GetInstance().ReadExcel(excel), unzippedList, this.shipment.MergedPDF));
                            }
                            else
                            {
                                this.shipment.NotifyCompleted();
                            }
                            
                                foundExcel = true;
                            break;
                        case ".pdf":

                            if (foundZip)
                            {
                                foundPDF = true; //We cannot have both zip files and pdf files.
                                failed++;
                                continue; //We either have a zip file or pdf files, not both.
                            }

                            if (!foundPDF) //We can have plenty of those, but only if we don't have a zip file.
                            {
                                this.unzippedList = new List<FileInfo>();
                                foundPDF = true;
                            }

                            this.unzippedList.Add(file);
                            textBox1.AppendText($"AWB Loaded: \"{file.Name}\"\r\n");
                            break;

                        default:
                            textBox1.AppendText($"File \"{file.Name}\" is not a valid file.\r\n");
                            failed++;
                            break;
                    }
                }
                else
                {
                    textBox1.AppendText($"File \"{file.Name}\" is either a directory or it doesn't exist.\r\n");
                    failed++;
                }

            }

            if (foundPDF)
            {
                if (!foundZip)
                {
                    zipLabel.Text = unzippedList.Count + " file" + (unzippedList.Count > 1 ? "s" : "");
                    zipPathBox.Text = unzippedList[0].Name + (unzippedList.Count > 1 ? $" + {unzippedList.Count - 1} others" : "");
                }
                else
                {
                    textBox1.AppendText($"Both zip files and pdf files were selected, all pdfs were discarded in favor of the zip archive.\r\n");
                }
            }

            textBox1.AppendText($"Processed {paths.Length - failed} out of {paths.Length} files.\r\n");
            if (failed == paths.Length)
            {
                textBox1.AppendText($"No valid files were selected.\r\n" +
                    $"Please select either a zip file, excel file, or some pdf files.");
            }
        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {
            string[] paths = (string[])e.Data.GetData(DataFormats.FileDrop);
            textBox1.BackColor = SystemColors.Window;

            ReadGivenFiles(paths);

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {

            tabControl2.SelectedIndex = 0;
            textBox1.BackColor = Color.LightPink;
        }


        private void Form1_DragLeave(object sender, EventArgs e)
        {
            textBox1.BackColor = SystemColors.Window;

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {

            if(e.Control && e.KeyCode == Keys.V)
            {
                string[] paths = Clipboard.GetFileDropList().Cast<string>().ToArray();
                if (paths.Length>0)
                {
                    ReadGivenFiles(paths);
                }
            }
        }

        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}

