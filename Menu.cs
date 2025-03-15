using Microsoft.Office.Interop.Excel;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO.Compression;
using PdfSharpCore.Pdf.IO;
using System.Diagnostics;



namespace PDFiller
{
    internal class Menu
    {
        internal DirectoryInfo rootDir=null ;
        internal DirectoryInfo workDir =null;
        internal FileInfo zip =null;
        internal FileInfo excel = null;
        internal List<FileInfo> unzippedList = null;
        Form1 form=null;
        static Menu menu=null;



        private Menu()
        {

        }

        private Menu(Form1 form)
        {
            this.form = form;
            this.rootDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\\");
        }


        static public Menu getInstance()
        {
            if (menu == null)
            {
                menu = new Menu();
            }
            return menu;
        }
        public static Menu getInstance(Form1 form)
        {
            if (menu == null)
            {
                menu = new Menu(form);
            }
            return menu;
        }


        //        menu.UpdateWorkDir(ofd.SelectedPath);

        public void UpdateRootDir(string selectedPath)
        {
            if (!Directory.Exists(selectedPath))
            {
                MessageBox.Show("Directory doesn't exist");
                return;
            }
            this.rootDir = new DirectoryInfo(selectedPath);
            DirectoryInfo workDir = rootDir.GetDirectories().OrderByDescending(x => x.CreationTime).First();
            UpdateWorkDir(workDir);
        }

        public DirectoryInfo UpdateWorkDir()
        {
            if (rootDir == null || !rootDir.Exists)
            {
                throw new Exception("Root no longer exists!\r\n");
            }
            DirectoryInfo[] dirs = rootDir.GetDirectories();
            if (dirs.Length == 0) throw new Exception("Root Directory contains no subdirectories.\r\n");

            dirs = dirs.OrderByDescending(d=>d.CreationTime).ToArray();
            return this.workDir = dirs.First();

            form.textBox1.Text += "Root directory found at " + rootDir.FullName + "\r\n";
        }
        public void UpdateWorkDir(string selectedPath)
        {
            if (!Directory.Exists(selectedPath))
            {
                //  workDir = new DirectoryInfo(selectedPath);
                MessageBox.Show("Directory doesn't exist");
                return;
            }
            workDir = new DirectoryInfo(selectedPath);
            excel = FindExcel(workDir);
            zip = FindZipsUnzipped(workDir);
        }

        public void UpdateWorkDir(DirectoryInfo workDir)
        {
            if (!workDir.Exists)
            {
                //  workDir = new DirectoryInfo(selectedPath);
                MessageBox.Show("Directory doesn't exist");
                return;
            }
            this.workDir = workDir;
            excel = FindExcel(workDir);
            zip = FindZipsUnzipped(workDir);
        }


        /*

        public void MergeFill()
        {
            if (  rootDir == null || !rootDir.Exists )
            {
                form.textBox1.Text += "Root Directory provided doesn't exist!\r\n";
            }
            form.textBox1.Text = "Root Directory found at: \r\n" + rootDir.FullName + "\r\n";
            .textBox1.Text = "Root Directory found at: \r\n" + rootDir.FullName + "\r\n";


            try
            {
                workDir = rootDir.GetDirectories().OrderByDescending(x => x.CreationTime).ToArray()[0];
            }
            catch (IndexOutOfRangeException)
            {
                form.textBox1.Text += "EMPTY DIRECTORY, NOTHING TO CHECK\r\n";

                return;
            }
            catch (Exception e)
            {
                throw e;
            }
            form.textBox1.Text += "Work Directory found at: \r\n" + workDir.FullName + "\r\n";
            form.textBox1.Text += "The most recent directory is \"" + workDir.Name + "\" created on \"" + workDir.CreationTime + "\" or \r\n";

            if ((DateTime.Now - workDir.CreationTime).Days > 0)
            {
                form.textBox1.Text += (DateTime.Now - workDir.CreationTime).Days + " days ago. \r\n";
            }
            else
            {
                form.textBox1.Text += (DateTime.Now - workDir.CreationTime).Minutes + " minutes ago. \r\n";
            }


            excel = FindExcel(workDir);
            zip = FindZipsUnzipped(workDir);
            form.excelPathBox.Text = excel.FullName;


        }
        */
        public FileInfo FindExcel(DirectoryInfo workDir)
        {
            FileInfo excel = null;
            if (workDir == null || !workDir.Exists) return null;
            try
            {
                excel = workDir.GetFiles().Where(o => o.Extension == ".xlsx").OrderByDescending(o => o.CreationTime).ToArray().First();
            }
            catch(IndexOutOfRangeException)
            {
                throw new Exception("NO EXCEL FILE WAS FOUND HERE!\r\n");
            }
            return excel;
        }


        public List<Order> ReadExcel(FileInfo excel)
        {

            Excel.Application app = new Excel.Application();
            List<Order> orders = new List<Order>();
            Workbook book = app.Workbooks.Open(excel.FullName);
            Worksheet sheet;
            try
            {
                sheet = book.Worksheets[1];
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
                            if (lastOrder.id != "")
                                orders.Add(lastOrder);
                            lastOrder = new Order(id, awb, name, qnt);
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
                return orders;
            }
            catch (Exception ex)
            {
                form.textBox1.Text += ex.Message;
                book.Close();
                throw ex;
            }
        }

        public List<FileInfo> UnzipArchive(FileInfo zip,ref string extractedZip)
        {
            if (zip == null || !zip.Exists) throw new Exception("Zip Archive doesn't exist");
            extractedZip = zip.FullName.Replace(".zip", "");
            ZipFile.ExtractToDirectory(zip.FullName, extractedZip);
            return new DirectoryInfo(extractedZip).GetFiles().ToList();
        }



        public FileInfo FindZipsUnzipped(DirectoryInfo workDir)
        {
            if (workDir == null || !workDir.Exists) {
                throw new Exception("Work Directory no longer exists!\r\n");
            }
            FileInfo[] zips = workDir.GetFiles().Where(x => x.Extension == ".zip").ToArray();
            DirectoryInfo[] extractedZips = workDir.GetDirectories().ToArray();

            bool found = false;
            foreach (FileInfo zip in zips)
            {
                foreach (DirectoryInfo dir in extractedZips)
                {
                    if (dir.Name == zip.Name.Replace(".zip", "")) //zip was unzipped
                    {
                        found = true;
                        break;
                    }
                }
                if (!found) return zip;
            }
            return null;
        }

        /// <summary>
        /// Filles all the unzipped pdfs with the coresponding orders and saves the merged pdf.
        /// If Open pdf in browser is checked, it also opens it.
        /// Returns a refference to the pdf file.
        /// </summary>
        /// <param name="unzippedList">The list of all unzipped pdf files.</param>
        /// <param name="orders">The list of all orders coresponding to those pdf files, taken from an excel sheet.</param>
        /// <param name="saveDir">The directory where we save the merged pdf to.</param>
        /// <returns></returns>
        public string WriteOnOrders(List<FileInfo> unzippedList, List<Order> orders, string saveDir)
        {
            int failed = 0;
            PdfDocument doc = new PdfDocument();
            
            foreach(Order o in orders)
            {
                FileInfo file = unzippedList.Find(p => p.Name.StartsWith(o.id) && p.Name.EndsWith(o.awb + "001.pdf"));
                unzippedList.Remove(file);
                PdfDocument pdf = PdfReader.Open(file.FullName, PdfDocumentOpenMode.Import);
                PdfPage page = pdf.Pages[0];
                doc.AddPage(page);
                if(WriteOnPage(doc.Pages[doc.PageCount - 1], o.toppere))
                {
                    failed++;
                }
            }
            doc.Save(saveDir + "\\Merged&Filled.pdf");
            doc.Close();
            if (failed > 0)
            {
                form.textBox1.Text = failed + " files failed being filled, please check them.\r\n";
            }
            else
            {
                form.textBox1.Text = "All files filled sucessfully.\r\n";
            }
            return saveDir + "\\Merged&Filled.pdf";
        }

        public bool WriteOnPage(PdfPage page, List<Order.topper> toppere)
        {
            if (page == null || toppere == null) {
                return false;
            }
            try
            {
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Times New Roman", 15);
                XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));

                XRect rect = new XRect(0, page.Height / 2 - 15, page.Width, page.Height / 2 + 15);
                gfx.DrawRectangle(XBrushes.White, rect);

                int i = 0;
                foreach (var topper in toppere)
                {
                    gfx.DrawString(topper.tQuantity + " buc: " + topper.tName, font, brush, 50, page.Height / 2 + 150 + 15 * (i++), XStringFormats.CenterLeft);
                }
            }
            catch (Exception ex)
            {
                form.textBox1.Text+=ex.Message+"\r\n";
                return true;
            }
            return false;
        }
    }
}
