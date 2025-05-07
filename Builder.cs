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
using System.Runtime.InteropServices;
using System.Data;
using System.Reflection;
using System.Numerics;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using System.Threading;
using Aspose.Pdf;
using System.Text.RegularExpressions;
using Aspose.Pdf.Text;



namespace PDFiller
{
    internal class Builder
    {

        private readonly Form1 form=null;
        static Builder menu=null;
        private Builder()
        {

        }

        private Builder(Form1 form)
        {
            this.form = form;
          //  this.rootDir = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\AWB\\");
        }
   

        static public Builder GetInstance()
        {
            if (menu == null)
            {
                menu = new Builder();
            }
            return menu;
        }
        public static Builder GetInstance(Form1 form)
        {
            if (menu == null)
            {
                menu = new Builder(form);
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
                const string IDCOL       = "A";
                const string AWBCOL      = "C";
                const string TNAMECOL    = "D";
                const string IDPRODUCT   = "E";
                const string TQNTCOL     = "G";
                const string NAMECOL     = "T";


                Order lastOrder = new Order();
                Builder builder = new Builder();
                while (true)
                {
                    string id = sheet.Cells[row, IDCOL].Value2;


                    if (id != null)
                    {
                        string awb = sheet.Cells[row, AWBCOL].Value2;
                        string name = sheet.Cells[row, NAMECOL].Value2;
                        string tName = sheet.Cells[row, TNAMECOL].Value2;
                        tName = builder.ModifyName(tName);

                        string idProduct = sheet.Cells[row, IDPRODUCT].Value2;
                        // , KZE Prints, Photo Paper Glossy
                  //      tname = tname.Replace(", KZE Prints, Photo Paper Glossy", "");
                  //      tname = tname.Remove(tname.Length - 32);

                        int qnt = (int)sheet.Cells[row, TQNTCOL].Value2;


                        if (id == lastOrder.id)
                        {
                            lastOrder.toppere.Add(new Order.topper(tName, qnt, idProduct));
                        }
                        else
                        {
                            if (lastOrder.id != "")
                                orders.Add(lastOrder);
                            lastOrder = new Order(id, awb,name, tName, qnt, idProduct);

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
        public List<FileInfo> UnzipArchive(FileInfo zip,ref string extractedZip)
        {
            if (zip == null || !zip.Exists) throw new ArgumentNullException("Zip Archive doesn't exist");
            extractedZip = zip.FullName.Replace(".zip", "");

            if (Directory.Exists(extractedZip))
                Directory.Delete(extractedZip, true);

            ZipFile.ExtractToDirectory(zip.FullName, extractedZip);
            return new DirectoryInfo(extractedZip).GetFiles().ToList();
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


        public bool ReadAwbId(Aspose.Pdf.Page page, out string idAWB)
        {
            idAWB = "";

            Regex regex = new Regex(@"\b4EMG\w{11}001\b");
            TextSearchOptions textSearchOptions = new TextSearchOptions(true);
            Aspose.Pdf.Text.TextFragmentAbsorber textFragmentAbsorber = new Aspose.Pdf.Text.TextFragmentAbsorber(regex,textSearchOptions);
            page.Accept(textFragmentAbsorber);
            if(textFragmentAbsorber.TextFragments.Count == 0)
            {
                return false;
            }
            idAWB = textFragmentAbsorber.TextFragments[1].Text;
            return true;
        }



        /// <summary>
        ///  
        /// </summary>
        /// <param name="unzippedList">All AWB files selected, automatically or manually, in PDF format and represented by the FileInfo proxy.</param>
        /// <param name="orders">All orders read from the Order Summary .xlsx file.</param>
        /// <param name="saveDir">The directory path where we want to save the resulting pdf.</param>
        /// <param name="failed">Returns the number of AWBs that failed processing.</param>
        /// <param name="name">The file name for the resulting PDF.</param>
        /// <returns>The full file path of the merged PDF.</returns>
        /// <exception cref="ArgumentException">Selected files had incorrect names.</exception>
        /// <exception cref="FileNotFoundException">Selected files could not be found.</exception>

        public string WriteOnOrders(List<FileInfo> unzippedList, List<Order> orders, string saveDir,out int failed,string name)
        {
            failed = 0;
            PdfDocument doc = new PdfDocument();
            foreach (FileInfo file in unzippedList)
            {
                if (file == null || !file.Exists)
                {
                    failed++;
                    continue;
                }
                string idOrder,idAWB;
                PdfDocument pdfDocument = PdfReader.Open(file.FullName, PdfDocumentOpenMode.Import);


                Aspose.Pdf.Document pdf = new Aspose.Pdf.Document(file.FullName);
                for (int i = 0; i<pdf.Pages.Count; i++ )
                {
                    if (!ReadAwbId(pdf.Pages[i+1],out idAWB))
                    {
                        failed++;
                        continue;
                    }
                    idOrder = idAWB.Substring(0, idAWB.Length - 3);
                    try
                    {
                        Order o = orders.Find(p => p.awb == idAWB);
                        doc.AddPage(pdfDocument.Pages[i]);
                        if (WriteOnPage(doc.Pages[doc.PageCount - 1], o.toppere))
                        {
                            failed++;
                            continue;
                        }
                    }
                    catch(Exception ex)
                    {
                        failed++;
                        throw ex;
                    }

                }
                pdfDocument.Close();

            }
            if (doc.PageCount == 0)
            {
                throw new ArgumentException("None of the orders in the excel matched the pdf files.\r\n");
            }
            string returnPath = $"{saveDir}\\{name}.pdf";
            doc.Save(returnPath);
            doc.Close();
            return returnPath;
        }

        /// <summary>
        /// List of special swaps. Checks for topper name (in future it will be ID) and associates it with a new, more readable name.
        /// 
        /// </summary>

        private List<KeyValuePair<String, String>> SpecialSwaps = new List<KeyValuePair<String, String>>()
            {
                new KeyValuePair<String, String>("Set 17 figurine tort/briose Patrula Catelusilor, KZE Prints, Photo Paper Glossy", "Paw Patrol tip2 (nou)"),
                new KeyValuePair<String, String>("Set 9 figurine tort Patrula Catelusilor, KZE Prints, Photo Paper Glossy", "Paw Patrol tip1 (vechi)"),
                new KeyValuePair<String, String>("Set 9 figurine tort Albine, KZE Prints, Photo Paper Glossy", "Albinute mici"),
                new KeyValuePair<String, String>("Set 8 figurine tort Albine, Tip 2, KZE Prints, Photo Paper Glossy", "Albine + Apicultor"),
                new KeyValuePair<String, String>("Set figurine tort/briose Barbie, Tip 4, KZE Prints, Photo Paper Glossy", "Barbie tip4 (cercuri)"),
                new KeyValuePair<String, String>("Set figurine tort/briose Barbie, Tip 3, KZE Prints, Photo Paper Glossy", "Barbie tip3 (silueta cap)"),
                new KeyValuePair<String, String>("Set figurine tort/briose Barbie, Tip 2, KZE Prints, Photo Paper Glossy", "Barbie tip2 (cercuri fancy)"),
                new KeyValuePair<String, String>("Set figurine tort/briose Barbie, KZE Prints, Tip 1, Photo Paper Glossy", "Barbie tip1 (cercuri funda)"),
                new KeyValuePair<String, String>("Set 10 figurine tort/briose Baby Boss, Tip 3, KZE Prints, Photo Paper Glossy", "Baby Boss tip3 (cercuri Logo)"),
                new KeyValuePair<String, String>("Set figurine tort/briose Baby Boss, Tip 2, KZE Prints, Photo Paper Glossy", "Baby Boss tip2 (cercuri copil)"),
                new KeyValuePair<String, String>("Set 12 figurine tort Buburuza, KZE Prints, Photo Paper Glossy", "12 Buburuze"),
                new KeyValuePair<String, String>("Set 12 figurine tort Inima Roz, KZE Prints, Photo Paper Glossy", "12 Inimi Roz <3"),
                new KeyValuePair<String, String>("Set 11 figurine tort Capsune, KZE Prints, Photo Paper Glossy", "11 Capsune + Vrej"),
                new KeyValuePair<String, String>("Set 10 figurine tort/briose Baby Boss, Tip 3, KZE Prints, Photo Paper Glossy", "Baby Boss tip3 (cercuri Logo)"),
        };

        /// <summary>
        /// Given the name {and id???} of a topper, removes the unnecessary prefix
        /// OR switches the name completely with a hardcoded table.
        /// </summary>
        /// <param name="name">A more readable topper name</param>
        /// <returns>A string that contains only the name of the topper mascot, or the same string unchanged if it couldn't be found.</returns>
        private string ModifyName(string name)
        {
            foreach(var pair in SpecialSwaps) {
                if(name == pair.Key) return pair.Value;
            }

            string[] list = { "briose de ", "briose ", "tort ", };
            foreach(string s in list)
            {
                int index = name.LastIndexOf(s);
                if (index > 0)
                {
                    return name.Substring(index + s.Length).Replace(", KZE Prints, Photo Paper Glossy", "");
                }

            }
            return name;
        }



        /// <summary>
        /// For the given pdf page which represents a whole AWB, clears the lower half, 
        /// then write the order contents: topper count, name and image (if exists).
        /// </summary>
        /// <param name="page">AWB Page to be written on</param>
        /// <param name="toppere">What to write on the page (qnt + name + image)</param>
        /// <returns>True if successfull, false if not</returns>
        private bool WriteOnPage(PdfPage page, List<Order.topper> toppere)
        {
            if (page == null || toppere == null) {
                return false;
            }
            try
            {
                XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Times New Roman", 14);
                XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));

                XRect rect = new XRect(0, page.Height / 2 - 15, page.Width, page.Height / 2 + 15);
                gfx.DrawRectangle(XBrushes.White, rect);

                int i = 0;

                /*
                ///This WILL dissappear from here.
                DirectoryInfo imagesDir = new DirectoryInfo("C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\PDFiller\\bin\\Debug\\images\\");
                FileInfo[] images  = imagesDir.GetFiles("*.jpg");
                Random rng = new Random(i);
                */



                foreach (var topper in toppere)
                {
                    /*
                    if (i < 16)
                    {
                        //    img = XImage.FromFile($"{imagesPath}\\{topper.tId}.jpg");
                     //   XImage img = XImage.FromFile(images[rng.Next(8)].FullName);
                     //   gfx.DrawImage(img, page.Width - 95 * (Math.Max(toppere.Count, 16) / 4) + 95 * (i / 4)+100, page.Height / 2 + 20 + 95 * (i % 4), 90, 90);
                    }
                    */
                    gfx.DrawString($"{topper.tQuantity} buc: {topper.tName}", font, brush, 25, page.Height / 2 + 25 + 20 * i, XStringFormats.CenterLeft);
                        // gfx.DrawImage(img, page.Width - 95 * (nrImagini / 4) + 95 * (j / 4), page.Height / 2 + 20 + 95 * (j % 4), 90, 90);
                    i++;

                    if (i == 20) break;
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
