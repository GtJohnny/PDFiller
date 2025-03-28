﻿using Microsoft.Office.Interop.Excel;
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
using System.Runtime.InteropServices;
using System.Data;



namespace PDFiller
{
    internal class Menu
    {

        Form1 form=null;
        static Menu menu=null;
        private Menu()
        {

        }

        private Menu(Form1 form)
        {
            this.form = form;
          //  this.rootDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\\");
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

        //public void UpdateRootDir(string selectedPath)
        //{
        //    if (!Directory.Exists(selectedPath))
        //    {
        //        MessageBox.Show("Directory doesn't exist");
        //        return;
        //    }
        //    rootDir = new DirectoryInfo(selectedPath);
        //    form.rootTextBox.Text = selectedPath;
        //    workDir = rootDir.GetDirectories().OrderByDescending(x => x.CreationTime).First();
        //    form.textBox1.Text += "Work directory found at:\r\n" +
        //                  workDir.FullName + "\r\n";
        //    excel = FindExcel(workDir);
        //    zip = FindZipsUnzipped(workDir);
        //}

        /// <summary>
        /// Given the root directory, find the work directory.
        /// </summary>
        /// <param name="rootDir">The root directory that would contain all workdirectories.</param>
        /// <returns>A reference to the newest created subdirectory</returns>
        /// <exception cref="ArgumentNullException">Is thrown if the root directory is null or doesn't exist.</exception>
        /// <exception cref="DirectoryNotFoundException">Is thrown if the root directory contains no subdirectories.</exception>
        public DirectoryInfo FindWorkDir(DirectoryInfo rootDir)
        {
            if (rootDir == null || !rootDir.Exists)
            {
                throw new ArgumentNullException("Root no longer exists!\r\n");
            }
      //      form.textBox1.Text += "Root directory found at:\r\n " + rootDir.FullName + "\r\n";
      //      form.rootTextBox.Text = rootDir.FullName;
            DirectoryInfo[] dirs = rootDir.GetDirectories();
            if (dirs.Length == 0) throw new DirectoryNotFoundException("Root Directory contains no subdirectories.\r\n");
            DirectoryInfo workDir;
            workDir = dirs.OrderByDescending(d=>d.CreationTime).ToArray().First();
      //      form.textBox1.Text+="Work directory found at:\r\n"+
       //         workDir.FullName + "\r\n";
            return workDir;
        }



        /// <summary>
        /// Given a path, find a given directory.
        /// </summary>
        /// <returns>A reference to the workdirectory specified by user.</returns>
        /// <param name="selectedPath">Selected path from directory dialogue"</param>
        /// <exception cref="ArgumentNullException">No path was given.</exception>
        /// <exception cref="DirectoryNotFoundException">Directory somehow doesn't exist.</exception>
        public DirectoryInfo FindWorkDir(string selectedPath)
        {
            if(selectedPath == null) throw new ArgumentNullException("No path was provided."); 
            if(!Directory.Exists(selectedPath)) throw new DirectoryNotFoundException("Work directory doesn't exist");
            return new DirectoryInfo(selectedPath); 
        }

        //public void UpdateWorkDir(DirectoryInfo workDir)
        //{
        //    if (!workDir.Exists)
        //    {
        //        //  workDir = new DirectoryInfo(selectedPath);
        //        MessageBox.Show("Directory doesn't exist");
        //        return;
        //    }
        //    form.textBox1.Text += "Work directory found at:\r\n" +
        //       workDir.FullName + "\r\n";
        //    excel = FindExcel(workDir);
        //    zip = FindZipsUnzipped(workDir);
        //}

        /// <summary>
        /// Given the workDirectory, find the excel file.
        /// </summary>
        /// <param name="workDir">The work directory</param>
        /// <returns>A reference to the newest created excel file in the work directory.</returns>
        /// <exception cref="ArgumentNullException">Work directory doesn't exist.</exception>
        /// <exception cref="FileNotFoundException">No excel file exists in work directory.</exception>
        public FileInfo FindExcel(DirectoryInfo workDir)
        {
            if (workDir == null || !workDir.Exists) throw new ArgumentNullException("Work directory doesn't exist.");
            FileInfo excel;
            try
            {
                excel = workDir.GetFiles().Where(o => o.Extension == ".xlsx").OrderByDescending(o => o.CreationTime).ToArray().First();
            }
            catch(Exception)
            {
                throw new FileNotFoundException("No excel file was found within the work directory!\r\n");
            }
            //      form.excelPathBox.Text = excel.FullName;
//form.newExcel = true;
            return excel;
        }

        /// <summary>
        /// Read the excel file, and return a list of orders.
        /// </summary>
        /// <param name="excel">The excel file to be read</param>
        /// <returns>A list of `Order` objects</returns>
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
                const string NAMECOL = "T";
                const string TNAMECOL = "D";
                const string TQNTCOL = "G";


                Order lastOrder = new Order();
                while (true)
                {
                    string id = sheet.Cells[row, IDCOL].Value2;


                    if (id != null)
                    {
                        string awb = sheet.Cells[row, AWBCOL].Value2;
                        string name = sheet.Cells[row, NAMECOL].Value2;
                        string tname = sheet.Cells[row, TNAMECOL].Value2;

                        tname = tname.Remove(tname.Length - 32);
                        int qnt = (int)sheet.Cells[row, TQNTCOL].Value2;


                        if (id == lastOrder.id)
                        {
                            lastOrder.toppere.Add(new Order.topper(tname, qnt));
                        }
                        else
                        {
                            if (lastOrder.id != "")
                                orders.Add(lastOrder);
                            lastOrder = new Order(id, awb,name, tname, qnt);

                            //form.textBox1.Text += ++i + ". " + awb + ":\r\n" +
                            //    "-> " + qnt + ". " + name + "\r\n";


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
                app.Quit();
                throw ex;
            }
        }






        internal void ReadExcelTable(List<Order> orders, DataGridViewRowCollection rows)
        {
            //    System.Data.DataTable dt = new System.Data.DataTable("Order Preview");
            rows.Clear();
            foreach (Order o in orders)
            {
                rows.Add( o.name, o.toppere[0].tName, o.toppere[0].tQuantity );
                foreach (Order.topper tp in o.toppere.GetRange(1, o.toppere.Count - 1))
                {
                    rows.Add( null, tp.tName, tp.tQuantity);
                }
            }
        }








        /// <summary>
        /// Extracts the archive per se.
        /// </summary>
        /// <param name="zip">The zip file</param>
        /// <param name="extractedZip">A refference to the folder that was extracted, for convenience</param>
        /// <returns>A list containing all the pdf files extracted.</returns>
        /// <exception cref="Exception"></exception>
        internal List<FileInfo> UnzipArchive(FileInfo zip,ref string extractedZip)
        {
            if (zip == null || !zip.Exists) throw new ArgumentNullException("Zip Archive doesn't exist");
            extractedZip = zip.FullName.Replace(".zip", "");
            if (Directory.Exists(extractedZip))Directory.Delete(extractedZip, true);
            ZipFile.ExtractToDirectory(zip.FullName, extractedZip);
            List<FileInfo> fileInfos = new DirectoryInfo(extractedZip).GetFiles().ToList();

            return fileInfos;
        }


        /// <summary>
        /// Looks to find the zip files that do not have a matching unzipped folder.
        /// Then proceeds to extract it.
        /// </summary>
        /// <param name="workDir">Work directory.</param>
        /// <returns>The referece that represents the newest zip file found, or null if not found.</returns>
        /// <exception cref="ArgumentNullException">Work directory doesn't exist.</exception>
        public FileInfo FindZipsUnzipped(DirectoryInfo workDir)
        {
            if (workDir == null || !workDir.Exists) {
                throw new ArgumentNullException("Work Directory no longer exists!\r\n");
            }
            FileInfo[] zips;
            DirectoryInfo[] extractedZips;
            try
            {
                zips = workDir.GetFiles().Where(x => x.Extension == ".zip").ToArray();
                extractedZips = workDir.GetDirectories().ToArray();
            }
            catch (IndexOutOfRangeException)
            {
                throw (new FileNotFoundException("No zip files exist"));
            }
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
                if (!found)
                {
                    form.zipLabel.Text = "Zip File:\r\n";
                    form.zipPathBox.Text = zip.FullName;
                    return zip;
                }
            }
            throw new FileNotFoundException("All zip files already extracted.\r\n");
        }

        /// <summary>
        /// Filles all the unzipped pdfs with the coresponding orders and saves the merged pdf.
        /// If Open pdf in browser is checked, it also opens it.
        /// Returns a refference to the pdf file.
        /// </summary>
        /// <param name="unzippedList">The list of all unzipped pdf files.</param>
        /// <param name="orders">The list of all orders coresponding to those pdf files, taken from an excel sheet.</param>
        /// <param name="saveDir">The directory where we save the merged pdf to.</param>
        /// <returns>Path to the merged pdf</returns>
        public string WriteOnOrders(List<FileInfo> unzippedList, List<Order> orders, string saveDir,out int failed,string name)
        {
            failed = 0;
            PdfDocument doc = new PdfDocument();

            foreach (FileInfo file in unzippedList)
            {
                Order o = null;
                try
                {
                    o = orders.Find(p => p.id == file.Name.Substring(0, 9));
                    if (o == null)
                    {
                        failed++;
                        continue;
                    }
                    PdfDocument pdf = PdfReader.Open(file.FullName, PdfDocumentOpenMode.Import);
                    PdfPage page = pdf.Pages[0];
                    doc.AddPage(page);
                }
                catch (Exception ex)
                {
                    failed++;
                    throw ex;
                }
                if (WriteOnPage(doc.Pages[doc.PageCount - 1], o.toppere))
                {
                    failed++;
                }

            }
            if (doc.PageCount == 0)
            {
                throw new ArgumentException("None of the orders in the excel matched the pdf files.\r\n");
            }
            doc.Save(saveDir + "\\" + name + ".pdf");
            doc.Close();
            return saveDir + "\\" + name + ".pdf";
        }



        /// <summary>
        /// Given the name of a topper, removes the unnecessary prefix
        /// </summary>
        /// <param name="name">Topper name</param>
        /// <returns>A topper that contains only the name of the topper mascot, or the same string unchanged if it couldn't be found.</returns>

        private string modifyName(string name)
        {
            string[] list = { "briose de", "briose", "tort", };
            foreach(string s in list)
            {
                int index = name.LastIndexOf(s);
                if (index > 0)
                {
                    return name.Substring(index + s.Length);
                }

            }
            return name;

        }





        /// <summary>
        /// Writes topper count and name on an individual page.
        /// </summary>
        /// <param name="page">Page to be written on</param>
        /// <param name="toppere">What to write on the page (qnt + name)</param>
        /// <returns>True if successfull, false if not</returns>
        private bool WriteOnPage(PdfPage page, List<Order.topper> toppere)
        {
            if (page == null || toppere == null) {
                return false;
            }
            try
            {
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Times New Roman", 12);
                XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));

                XRect rect = new XRect(0, page.Height / 2 - 15, page.Width, page.Height / 2 + 15);
                gfx.DrawRectangle(XBrushes.White, rect);

                int i = 0;


                foreach (var topper in toppere)
                {
                    gfx.DrawString(topper.tQuantity + " buc: " + modifyName(topper.tName), font, brush, 50, page.Height / 2 + 150 + 15 * (i++), XStringFormats.CenterLeft);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }
    }
}
