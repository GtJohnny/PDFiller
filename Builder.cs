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
using System.Text.RegularExpressions;
using System.Web;
using System.Net;



namespace PDFiller
{
    internal class Builder
    {
        static Builder menu = null;

        private readonly Form1 form = null;
        private readonly Regex regex = new Regex(@"4(EMG|ONB)\w{11}00[0-9]");
        private readonly string imagesDir = Environment.CurrentDirectory+@"\images";

        
        private readonly Dictionary<String, String> SpecialSwaps = new Dictionary<String, String>()
{
            { "5941933302517", "Barbie tip4 (cercuri)" },
            { "5941933302524", "Barbie tip3 (silueta)" },
            { "5941933302531", "Barbie tip2 (fancy)" },
            { "5941933302548", "Barbie tip1 (funda)" },
            { "5941933302470", "Baby Boss tip3 (Logo)" },
            { "5941933302487", "Baby Boss tip2 (baby)" },
            { "5941933307475", "Paw Patrol tip2" },
            { "5941933302135", "Paw Patrol tip1" },
            { "5941933302197", "Minnie Mouse" },
            { "5941933302333", "Albinute mici" },
            { "5941933302326", "Albine mari" },
            { "Set 312 figurine tort Buburuza, KZE Prints, Photo Paper Glossy", "12 Buburuze" },
            { "Set 12 figurine tort Inima Roz,  Prints, Photo Paper Glossy", "12 Inimi Roz <3" },
            { "18", "Buburuza & Motan Noir" },
            { "5941933307703", "Squid Game" },
            { "Se4t 113 tort Capsune, KZE Prints, Photo Paper Glossy", "Eroi in Pijama" },
            { "5941933307536", "Gym" },
            { "5941933307789", "Blaze" },
            { "S8et 115rt Capsune, KZE Prints,  Paper Glossy", "Peppa Pig" },
            { "Set 11  tort Capsune, K65ZE Prints, Photo Paper Glossy", "Fulger McQueen" }
        };

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
            workDir = dirs.OrderByDescending(d => d.CreationTime).ToArray().First();
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
            if (selectedPath == null) throw new ArgumentNullException("No path was provided.");
            if (!Directory.Exists(selectedPath)) throw new DirectoryNotFoundException("Work directory doesn't exist");
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
            catch (Exception)
            {
                throw new FileNotFoundException("No excel file was found within the work directory!\r\n");
            }
            form.excelPathBox.Text = excel.FullName;
            //form.newExcel = true;
            return excel;
        }


        /// <summary>
        /// Because the excel format may be updates from time to time,
        /// We just take all columns from the first row,
        /// Regardless of their order.
        /// </summary>
        /// <param name="sheet">The worksheet with all the orders</param>
        /// <returns>A dictionary that has as key the header of each column, and the index of the column in a byte</returns>
        private Dictionary<string,byte> GetExcelColumns(Worksheet sheet)
        {
            Dictionary<string, byte> columns = new Dictionary<string, byte>();
            byte col = 1;
            while (true)
            {
                string value = sheet.Cells[1, col].Value2;
                if (value == null) break;
                columns.Add(value, col);
                col++;
            }
            return columns;
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
                Dictionary<string, byte> columns = GetExcelColumns(sheet);

                byte row = 2;
                byte IDCOL               = columns["Nr. comanda"];
                byte AWBCOL              = columns["Numar AWB"];
                byte TOPPER_NAME_COL     = columns["Nume produs"];
                byte IDPRODUCT           = columns["Cod produs"];
                byte TOPPER_QUANTITY_COL = columns["Cantitate"];
                byte CLIENT_NAME         = columns["Nume client"];


                Order lastOrder = new Order();
                Builder builder = new Builder();
                while (true)
                {

                    string id = sheet.Cells[row, IDCOL].Value2;


                    if (id != null)
                    {
                        string awb = sheet.Cells[row, AWBCOL].Value2;
                        string name = sheet.Cells[row, CLIENT_NAME].Value2;
                        string tName = sheet.Cells[row, TOPPER_NAME_COL].Value2;
                        string idProduct = sheet.Cells[row, IDPRODUCT].Value2;
                        tName = ModifyName(idProduct, tName);


                        // , KZE Prints, Photo Paper Glossy
                        //      tname = tname.Replace(", KZE Prints, Photo Paper Glossy", "");
                        //      tname = tname.Remove(tname.Length - 32);

                        int qnt = (int)sheet.Cells[row, TOPPER_QUANTITY_COL].Value2;


                        if (id == lastOrder.id)
                        {
                            Order.topper topper = new Order.topper(tName, qnt, idProduct);
                            lastOrder.toppere.Add(topper);

                        }
                        else
                        {
                            if (lastOrder.id != "")
                                orders.Add(lastOrder);
                            lastOrder = new Order(id, awb, name, tName, qnt, idProduct);

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
                rows.Add(o.name, o.toppere[0].tName, o.toppere[0].tQuantity);
                foreach (Order.topper tp in o.toppere.GetRange(1, o.toppere.Count - 1))
                {
                    rows.Add(null, tp.tName, tp.tQuantity);
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
        public List<FileInfo> UnzipArchive(FileInfo zip, ref string extractedZip)
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


        public bool ReadAwbId(UglyToad.PdfPig.Content.Page page, out string idAWB)
        {
            idAWB = "";
            string text = page.Text;
            MatchCollection matches = regex.Matches(text);
            if (matches.Count == 0)
            {
                return false;
            }
            idAWB = matches[0].Value.Substring(0, 15);
            return true;

        }




        private int failed { get; set; } = 0;
        private int total { get; set; } = 0;




        /// <summary>
        /// Given a PDF file, reads the AWB number from it, and writes the order details on the page.
        /// </summary>
        /// <param name="pdfMerge">The final document upon which we constantly attach pages on</param>
        /// <param name="file">The pdf file that we'll complete page by page</param>
        /// <param name="orders">The entire orders file from the excel</param>
        /// <returns></returns>
        private List<string> WriteOnFile(PdfSharpCore.Pdf.PdfDocument pdfMerge, FileInfo file, List<Order> orders)
        {
            List<string> errorMessages = new List<string>();
            string idAWB;
            PdfSharpCore.Pdf.PdfDocument pdfWrite = PdfReader.Open(file.FullName, PdfDocumentOpenMode.Import);
            using (var pdfRead = UglyToad.PdfPig.PdfDocument.Open(file.FullName))
            {
                total += pdfWrite.PageCount;
                for (int i = 0; i < pdfWrite.PageCount; i++)
                {
                    if (!ReadAwbId(pdfRead.GetPage(i + 1), out idAWB))
                    {
                        errorMessages.Add($"Couldn't find AWB number for page {i + 1} for:\r\n{file.Name}\r\n");
                        failed++;
                        continue;
                    }

                    Order o = orders.Find(p => p.awb == idAWB);
                    //Sometimes the excel file may NOT have the AWB id that we need
                    //but we may still be able to match the order with the file name and order id.
                    //as last resort only
                    if (o == null)
                    {
                        string fileId = file.Name.Substring(0, 9);
                        o = orders.Find(p => p.id == fileId && new Regex(@"\b[0-9]{9}").IsMatch(fileId));

                        if (o == null)
                        {
                            form.textBox1.Text += $"Neither the AWB nor the order id match for:\r\n{file.Name}\r\n";
                            if (pdfWrite.PageCount > 1)
                            {
                                errorMessages.Add($"Couldn't find an order match at page {i + 1} for:\r\n{file.Name}\r\n");
                            }
                            else
                            {
                                errorMessages.Add($"Couldn't find an order match for:\r\n{file.Name}\r\n");
                            }
                            failed++;
                            continue;
                        }
                        else
                        {
                            errorMessages.Add($"Failed to match the excel AWB id, but managed to match the file name and order ID\r\n");
                        }

                        //errorMessages.Add($"Something bad happened for:\r\n{file.Name}\r\n");
                        //failed++;
                        //continue;
                    }

                    pdfMerge.AddPage(pdfWrite.Pages[i]);

                    XGraphics gfx = XGraphics.FromPdfPage(pdfMerge.Pages[pdfMerge.PageCount - 1]);
                    XRect rect = new XRect(0, gfx.PageSize.Height / 2 - 15, gfx.PageSize.Width, gfx.PageSize.Height / 2 + 15);
                    gfx.DrawRectangle(XBrushes.White, rect);

                    switch (form.drawComboBox.SelectedIndex)
                    {
                        case 0:

                            if (!WriteOnPage(gfx, o.toppere))
                            {
                                errorMessages.Add($"Couldn't write on page {i + 1} for:\r\n{file.Name}\r\n");
                                failed++;
                                continue;
                            }


                            break;
                        case 1:
                            if (!WriteOnPage(gfx, o.toppere))
                            {
                                errorMessages.Add($"Couldn't write on page {i + 1} for:\r\n{file.Name}\r\n");
                                failed++;
                                continue;
                            }
                            DrawOnPage(gfx, o.toppere, 2);
                            break;
                        case 2:
                            DrawOnPage(gfx, o.toppere, 4);
                            break;

                        default:
                            if (!WriteOnPage(gfx, o.toppere))
                            {
                                errorMessages.Add($"Couldn't write on page {i + 1} for:\r\n{file.Name}\r\n");
                                failed++;
                                continue;
                            }
                            break;
                    

                    }



                    //if (!WriteOnPage(gfx, o.toppere))
                    //{
                    //    errorMessages.Add($"Couldn't write on page {i + 1} for:\r\n{file.Name}\r\n");
                    //    failed++;
                    //    continue;
                    //}

                    //DrawOnPage(gfx, o.toppere);


                }
            }
            return errorMessages;
        }


        /// <summary>
        /// Processes a list of PDF files and writes order details onto the corresponding pages.
        /// The method merges the modified PDFs into a single output file.
        /// </summary>
        /// <param name="unzippedList">All AWB files selected, automatically or manually, in PDF format and represented by the FileInfo proxy.</param>
        /// <param name="orders">All orders read from the Order Summary .xlsx file.</param>
        /// <param name="saveDir">The directory path where we want to save the resulting pdf.</param>
        /// <param name="saveName">The file name for the resulting PDF.</param>
        /// <returns>The full file path of the merged PDF.</returns>
        /// <exception cref="ArgumentException">Selected files had incorrect names.</exception>
        /// <exception cref="FileNotFoundException">Selected files could not be found.</exception>




        public string WriteOnOrders(List<FileInfo> unzippedList, List<Order> orders, string saveDir, string saveName)
        {
            failed = total = 0;
            PdfSharpCore.Pdf.PdfDocument pdfMerge = new PdfSharpCore.Pdf.PdfDocument();
            foreach (FileInfo file in unzippedList)
            {
                if (file == null || !file.Exists)
                {
                    form.textBox1.Text += $"File {file.Name} doesn't exist.\r\n";
                    continue;
                }


                List<string> errorMessages = WriteOnFile(pdfMerge, file, orders);
                foreach (string error in errorMessages)
                {
                    form.textBox1.Text += error;
                }

            }
            if (pdfMerge.PageCount == 0)
            {
                throw new ArgumentException("None of the orders in the excel matched the pdf files.\r\n");
            }
            string returnPath = $"{saveDir}\\{saveName}.pdf";

            form.textBox1.Text += $"[{DateTime.Now.ToString("HH:mm:ss")}]\r\nSuccesfully wrote {total - failed} awbs out of {total} AWBS\r\n";
            if (failed > 0)
            {
                form.textBox1.Text += $"\r\nFailed to write {failed} AWBs. Please check them or their excel spreadsheets.\r\n";
            }
            else
            {
                form.textBox1.Text += $"All AWBs were written successfully.\r\n";
            }
            pdfMerge.Save(returnPath);
            pdfMerge.Close();
            return returnPath;
        }


        /// <summary>
        /// Modifies the name of the topper, if it is in the special swaps dictionary.
        /// Otherwise, it trims the worthless words out.
        /// </summary>
        /// <param name="tId">The Product Number (PN) of the product</param>
        /// <param name="tName">The original name of said product</param>
        /// <returns></returns>
        private string ModifyName(string tId,string tName)
        {
            if(SpecialSwaps.ContainsKey(tId))
            {
                return tName = SpecialSwaps[tId];
            }

            string[] list = { "briose de ", "briose ", "tort ", };
            foreach(string s in list)
            {
                int index = tName.LastIndexOf(s);
                if (index > 0)
                {
                    return tName = tName.Substring(index + s.Length).Replace(", KZE Prints, Photo Paper Glossy", "");
                }

            }
            return tName;
            
        }



        /// <summary>
        /// For the given pdf page which represents a whole AWB, clears the lower half, 
        /// then write the order contents: topper count, name and image (if exists).
        /// </summary>
        /// <param name="gfx">AWB graphics to be written and/or drawn on</param>
        /// <param name="toppere">What to write on the page (qnt + name + image)</param>
        /// <returns>True if successfull, false if not</returns>
        private bool WriteOnPage(XGraphics gfx, List<Order.topper> toppere)
        {
            if (gfx == null || toppere == null) {
                return true;
            }
            try
            {
          //      XGraphics gfx = XGraphics.FromPdfPage(page);

                XFont font = new XFont("Times New Roman", 14);
                XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));

  
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
                    gfx.DrawString($"{topper.tQuantity} buc: {topper.tName}", font, brush, 25, gfx.PageSize.Height / 2 + 25 + 20 * i, XStringFormats.CenterLeft);
                        // gfx.DrawImage(img, page.Width - 95 * (nrImagini / 4) + 95 * (j / 4), page.Height / 2 + 20 + 95 * (j % 4), 90, 90);
                    i++;

                    if (i == 20) break;
                }




            }
            catch (Exception ex)
            {
                throw ex;
            }
            return true;
        }


        public void ZStartTest()
        {
       //     var cli = new WebClient();
       //     cli.DownloadFile("")




            Builder builder = Builder.GetInstance();
            const string path = @"C:\Users\KZE PC\Desktop\VIsual studio projects\PDFiller\bin\Debug\debugTests\";
            string inputfPath = "417264331_Sameday_4EMG24107789758001.pdf";
            string excelPath = "orders_details_file_30-04-2025-21-52-52.xlsx";
            List<Order> orders = builder.ReadExcel(new FileInfo(path+excelPath));
            List<FileInfo> unzipped = new List<FileInfo>()
            {
                new FileInfo(path+inputfPath)
            };
            string savedPDFpath = form.mergedPath =  builder.WriteOnOrders(unzipped, orders, path, "TestName");
            Process.Start(savedPDFpath);


            



        }


        private XImage TryFindImage(string tId)
        {
            string[] extensions = { ".png", ".jpeg", ".jpg" };
            foreach (string ext in extensions)
            {
                if (System.IO.File.Exists($"{imagesDir}\\{tId}{ext}"))
                {
                    return XImage.FromFile($"{imagesDir}\\{tId}{ext}");
                }
            }
            return null; 
        }






        private void DrawOnPage(XGraphics gfx, List<Order.topper> toppere,int perPage)
        {
            Dictionary<string,XImage> images = new Dictionary<string, XImage>();
            int i = 0;
            foreach (Order.topper topper in toppere)
            {
               
                XImage img = null;
                if(images.ContainsKey(topper.tId))
                {
                    img = images[topper.tId];
                }
                else
                {
                     img = TryFindImage(topper.tId);
                     if (img != null)
                     {
                        images.Add(topper.tId, img);
                     }
                }

                //MATH =====>>       (scales with images/row)+ (pageH=90 +30 space)+no out of bounds  
                if (img != null)
                    gfx.DrawImage(img, (i % perPage) * (90 + perPage * 12) +20 + (perPage == 2 ? gfx.PageSize.Width / 2 : 20), (i / perPage) * 120 + gfx.PageSize.Height / 2 +25, 90, 90);
                //MATH =====>>
                //per pozition *  (pageH=90 +30 space + space with img/row) - (center text) + (even abscise per img/row (2= right column, 3=wide)

                string temp_name = $"{topper.tQuantity}:{topper.tName}";

                gfx.DrawString(temp_name, new XFont("Times New Roman", 12, XFontStyle.Regular), XBrushes.Black, (i % perPage) * (90 + perPage*12) + 65 - 5.7f * (topper.tName.Count() / 2) + (perPage == 2 ? gfx.PageSize.Width / 2: 20  ), (i / perPage) * 120 + 100 + gfx.PageSize.Height / 2 + 35);

                i++;
                if (i == 3 * perPage)
                {
                    return;
                }
            }

        }


    }
}
