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
        public void UpdateWorkDir(string selectedPath)
        {
            if (!Directory.Exists(selectedPath))
            {
                //  workDir = new DirectoryInfo(selectedPath);
                MessageBox.Show("Directory doesn't exist");
                return;
            }
            excel = FindExcel(workDir);
            zip = FindZipsUnzipped(workDir);
        }




        public void MergeFill()
        {
            if (  rootDir == null || !rootDir.Exists )
            {
                form.textBox1.Text += "Root Directory provided doesn't exist!\r\n";
            }
            form.textBox1.Text = "Root Directory found at: \r\n" + rootDir.FullName + "\r\n";

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
                form.textBox1.Text += "ZIP FILE WAS NOT FOUND HERE!\r\n";
                return null;
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

        public FileInfo[] UnzipArchive(FileInfo zip)
        {
            if(zip==null || !zip.Exists)   return null;
            string extractedZip = zip.FullName.Replace(".zip", "");
            ZipFile.ExtractToDirectory(zip.FullName, extractedZip);
            return new DirectoryInfo(extractedZip).GetFiles();
        }



        public FileInfo FindZipsUnzipped(DirectoryInfo workDir)
        {
            FileInfo[] zips = workDir.GetFiles().Where(x => x.Extension == ".zip").ToArray();
            DirectoryInfo[] extractedZips = workDir.GetDirectories().ToArray();

           // bool found = false;
            foreach (FileInfo zip in zips)
            {
                foreach (DirectoryInfo dir in extractedZips)
                {
                    if (dir.Name == zip.Name.Replace(".zip", "")) //zip was not unzipped
                    {
               //         found = true;
                        form.zipPathBox.Text = zip.FullName;
                        form.textBox1.Text += "One zip file was found that was not extracted.\r\n" + zip.FullName + "\r\n";
                        return zip;
                    }
                }

            }
            return null;
        }

        public void WriteOnPage(List<Order.topper> toppere, PdfPage page)

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
    }
}
