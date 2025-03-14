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
        DirectoryInfo rootDir=null ;
        DirectoryInfo workDir=null;
        FileInfo zip=null;
        FileInfo excel = null;
        List<FileInfo> unzipped_list = null;
        Form1 form=null;
        Menu menu=null;



        private Menu()
        {

        }

        private Menu(Form1 form)
        {
            this.form = form;
            this.rootDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\\");
        }




        public Menu getInstance()
        {
            if (menu == null)
            {
                menu = new Menu();
            }
            return menu;
        }
        public Menu getInstance(Form1 form)
        {
            if (menu == null)
            {
                menu = new Menu(form);
            }
            return menu;
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

            if ((DateTime.Now - workdir.CreationTime).Days > 0)
            {
                textBox1.Text += (DateTime.Now - workdir.CreationTime).Days + " days ago. \r\n";
            }
            else
            {
                textBox1.Text += (DateTime.Now - workdir.CreationTime).Minutes + " minutes ago. \r\n";
            }


            FileInfo excel = FindExcel();
            FindZipsUnzipped(workdir, excel);
            excelPathBox.Text = excel.FullName;


        }
        public List<Order> ReadExcel()
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
                int x = Form11.
                textBox1.Text += ex.Message;
                book.Close();
                throw ex;
            }
        }

        public bool FindZipsUnzipped(DirectoryInfo workdir, FileInfo excel)
        {
            FileInfo[] zips = workdir.GetFiles().Where(x => x.Extension == ".zip").ToArray();
            DirectoryInfo[] extractedZips = workdir.GetDirectories().ToArray();

            bool found = false;
            foreach (FileInfo zip in zips)
            {
                foreach (DirectoryInfo dir in extractedZips)
                {
                    if (dir.Name == zip.Name.Replace(".zip", "")) //zip was not unzipped
                    {
                        found = true;
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
            return found;
        }
        public int failed = 0;

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
