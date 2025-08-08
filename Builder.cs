using Microsoft.Office.Interop.Excel;
using Newtonsoft.Json.Linq;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using PdfSharpCore.Pdf.Content.Objects;
using PdfSharpCore.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static Microsoft.WindowsAPICodePack.Shell.PropertySystem.SystemProperties.System;
using Excel = Microsoft.Office.Interop.Excel;



namespace PDFiller
{

    /// <summary>
    /// A singleton builder class that manages the reading of excel files and mapping of orders.
    /// Returns a Shipment object that contains all the orders and the bussiness logic.
    /// </summary>
    internal class Builder
    {
        /// <summary>
        /// Singleton instance of the Builder class.
        /// </summary>
        private static Builder menu = null;


        private Form1 form = null;





        private readonly string imagesDir = Environment.CurrentDirectory + @"\images";

        //TODO move this to a file and that's it. 
        private readonly Dictionary<string, string> SpecialSwaps = new Dictionary<string, string>()
        {
            { "5941933302128", "Peppa Pig" },
            { "5941933302135", "Paw Patrol tip1" },
            { "5941933302197", "Minnie Mouse" },
            { "5941933302227", "12 Inimi Roz <3" },
            { "5941933302258", "Buburuza & Chat Noir" },
            { "5941933302265", "12 Buburuze" },
            { "5941933302326", "Albine mari" },
            { "5941933302333", "Albinute mici" },
            { "5941933302470", "Baby Boss tip3 (Logo)" },
            { "5941933302487", "Baby Boss tip2 (baby)" },
            { "5941933302517", "Barbie tip4 (cercuri)" },
            { "5941933302524", "Barbie tip3 (silueta)" },
            { "5941933302531", "Barbie tip2 (fancy)" },
            { "5941933302548", "Barbie tip1 (funda)" },
            { "5941933307475", "Paw Patrol tip2" },
            { "5941933307536", "Gym" },
            { "5941933307543", "Grizzy" },
            { "5941933307567", "Eroi in Pijama" },
            { "5941933307642", "BanBan" },
            { "5941933307703", "Squid Game" },
            { "5941933307758", "Cars" },
            { "5941933307789", "Blaze" }
        };

        private Builder()
        {

        }

        private Builder(Form1 form)
        {
            this.form = form;
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
            if (dirs.Length == 0)
            {
                throw new DirectoryNotFoundException("Root Directory contains no subdirectories.\r\n");
            }

            DirectoryInfo workDir;
            workDir = dirs.OrderByDescending(d => d.CreationTime).ToArray().First();
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
        /// <param name="data">A string matrix containing the data read from the excel sheet</param>
        /// <param name="nrCol">Number of columns</param>
        /// <returns>A dictionary that has as key the header of each column, and the index of the column in a byte</returns>
        private Dictionary<string, byte> GetExcelColumns(string[,] data,int nrCol)
        {
            Dictionary<string, byte> columns = new Dictionary<string, byte>();
            for(byte col = 1; col <= nrCol; col++){
                columns.Add(data[1, col], col);
            }
            return columns;
        }



        /// <summary>
        /// Adds an order to the specified list of orders. 
        /// Also handles multiple generated AWBs for one order.
        /// </summary>
        /// <remarks>If the order contains multiple AWB numbers, a new order is created for each AWB,  and
        /// all such orders are added to the list. The original order remains unchanged.</remarks>
        /// <param name="orders">The list of orders to which the order will be added.</param>
        /// <param name="order">The order to add. If the order's AWB contains multiple values separated by commas,  each AWB will be treated
        /// as a separate order and added individually.</param>
        private void AddOrderToList(List<Order> orders, Order order)
        {
            string[] awbs = order.awb.Split(',');
            if (awbs.Length > 1)
            {
                form.textBox1.AppendText("One order in the Excel file had more than one AWB generated:\r\n");
                foreach (string a in awbs)
                {
                    form.textBox1.AppendText($"-> {a}\r\n");

                    order.awb = a.Trim();
                    order.note = awbs.Count()+"awbs";
                    orders.Add(new Order(order));
                }
            }
            else
            {
                orders.Add(order);
            }
           
        }

        /// <summary>
        /// Read the excel file, and return a list of orders.
        /// Note that if the excel column headers change, this code will also be changed.
        /// Also if an order has more than one AWB, then the order will be duplicated on purpose.
        /// </summary>
        /// <param name="excel">The excel file to be read</param>
        /// <returns>A list of `Order` objects</returns>
        public List<Order> ReadExcel(FileInfo excel)
        {
            Cursor.Current = Cursors.WaitCursor;

            Excel.Application app = new Excel.Application();
            List<Order> orders = new List<Order>();
            Workbook book = app.Workbooks.Open(excel.FullName);
            Worksheet sheet = null;
            try
            {
                sheet = book.Worksheets[1];
            }
            catch(COMException ex)
            {
                form.textBox1.AppendText("The excel file is empty or doesn't contain any worksheets.\r\n");
                form.textBox1.AppendText(ex.Message);
                book.Close(0);
                app.Quit();
                return null;
            }
            Range range = sheet.UsedRange;
            int x=range.Rows.Count;
            int y=range.Columns.Count;
            string[,] data = new string[x+1, y+1];
            for (int i = 1; i <= x; i++)
            {
                for (int j = 1; j <= y; j++)
                {
                    data[i, j] = sheet.Cells[i, j].Value2?.ToString();
                }
            }
            book.Close(0);
            app.Quit();
            Marshal.ReleaseComObject(book);
            Marshal.ReleaseComObject(app);
            GC.Collect();
            try
            {
                Dictionary<string, byte> columns = GetExcelColumns(data,y);

                byte IDCOL = columns["Nr. comanda"];
                byte AWBCOL;
                try
                {
                    AWBCOL = columns["Numar AWB"];

                }
                catch (KeyNotFoundException)
                {
                    form.textBox1.AppendText("The excel file doesn't contain the 'Numar AWB' column. Trying 'AWB number'\r\n");
                    AWBCOL = columns["AWB number"];
                }
                byte TOPPER_NAME_COL = columns["Nume produs"];
                byte IDPRODUCT = columns["Cod produs"];
                byte TOPPER_QUANTITY_COL = columns["Cantitate"];
                byte CLIENT_NAME = columns["Nume client"];
                byte SHIPPING_ADDRESS = columns["Adresa de livrare"];
                Order order = new Order();
                string id = "first value";

                for(int row = 2;row <= x;row++)
                {
                    id = data[row,IDCOL];
                    if(id == null)
                    {
                        break;
                    }

                    string awb = data[row, AWBCOL];
                    string name = data[row, CLIENT_NAME];
                    string tName = data[row, TOPPER_NAME_COL];
                    string idProduct = data[row, IDPRODUCT];
                    string shippingAddress = data[row, SHIPPING_ADDRESS];
                    int qnt = int.Parse(data[row, TOPPER_QUANTITY_COL]);
                    tName = ModifyName(idProduct, tName);

                    string country = shippingAddress.Split(',').Last().Trim();
                    country = new RegionInfo(country).EnglishName;

                    if (id == order.id)
                    {
                        order.toppers.Add(new Order.topper(tName, qnt, idProduct));
                    }
                    else
                    {
                        if (order.id != "")
                        {

                            AddOrderToList(orders, order);

                        }
                        order = new Order(id, awb, name, tName, qnt, idProduct, country);
                    }

                }
                AddOrderToList(orders, order);
                return orders;
            }
            catch (Exception ex)
            {

                form.textBox1.AppendText(ex.Message);
                return new List<Order>();
            }
           
        }






        //internal void ReadExcelTable(List<Order> orders, DataGridViewRowCollection rows)
        //{
        //    //    System.Data.DataTable dt = new System.Data.DataTable("Order Preview");
        //    rows.Clear();
        //    foreach (Order o in orders)
        //    {
        //        rows.Add(o.name, o.toppere[0].tName, o.toppere[0].tQuantity);
        //        foreach (Order.topper tp in o.toppere.GetRange(1, o.toppere.Count - 1))
        //        {
        //            rows.Add(null, tp.tName, tp.tQuantity);
        //        }
        //    }
        //}








        /// <summary>
        /// Extracts the archive per se.
        /// </summary>
        /// <param name="zip">The zip file</param>
        /// <param name="extractedZip">The fullpath to the folder that was extracted, for convenience</param>
        /// <returns>A list containing all the pdf files extracted.</returns>
        /// <exception cref="Exception"></exception>
        public List<FileInfo> UnzipArchive(FileInfo zip, out string extractedZip)
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
            if (workDir == null || !workDir.Exists)
            {
                throw new ArgumentNullException("Work Directory no longer exists!\r\n");
            }
            FileInfo[] zips;
            DirectoryInfo[] extractedZips;
            try
            {
                zips = workDir.GetFiles().Where(x => x.Extension == ".zip").ToArray();
                if (zips.Length == 0)
                {
                    throw new FileNotFoundException("No zip files exist in the work directory.\r\n");
                }
                extractedZips = workDir.GetDirectories().ToArray();
            }
            catch (IndexOutOfRangeException ex)
            {
                throw ex;
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
        /// Identifies the AWB number from the given page.
        /// Aswell as the country of origin based on the AWB format.
        /// </summary>
        /// <param name="page">Page to read text from</param>
        /// <param name="orders">Orders from which one should have a matching AWB code</param>
        /// <param name="awb">The AWB code in question</param>
        /// <returns>True if successfull, false if not</returns>
        public bool ReadAwbId(UglyToad.PdfPig.Content.Page page, List<Order> orders, out string awb)
        {
            awb = "";
            string text = page.Text;
            foreach(Order o in orders)
            {
                if(text.Contains(o.awb))
                {
                    awb = o.awb;
                    return true;
                }
            }
            return false;

        }




        private int failed { get; set; } = 0;
        private int total { get; set; } = 0;




        /// <summary>
        /// Given a PDF file, reads the AWB number from it, and writes the order details on the page.
        /// Also clears out the lower half and writes the country name in the middle.
        /// </summary>
        /// <param name="pdfMerge">The final document upon which we constantly attach pages on</param>
        /// <param name="file">The pdf file that we'll complete page by page</param>
        /// <param name="orders">The entire orders file from the excel</param>
        /// <returns></returns>
        private List<string> WriteOnFile(PdfSharpCore.Pdf.PdfDocument pdfMerge, FileInfo file, List<Order> orders)
        {
            List<string> errorMessages = new List<string>();
            PdfSharpCore.Pdf.PdfDocument pdfWrite = PdfReader.Open(file.FullName, PdfDocumentOpenMode.Import);
            using (var pdfRead = UglyToad.PdfPig.PdfDocument.Open(file.FullName))
            {
                total += pdfWrite.PageCount;
                for (int i = 0; i < pdfWrite.PageCount; i++)
                {
                    string awb = "";
                    if (!ReadAwbId(pdfRead.GetPage(i + 1), orders, out awb))
                    {
                        errorMessages.Add($"Couldn't find AWB number for page {i + 1} for:\r\n{file.Name}\r\n");
                        failed++;
                        continue;
                    }

                    Order o = orders.Find(p => p.awb == awb);
                    //Sometimes the excel file may NOT have the AWB id that we need
                    //but we may still be able to match the order with the file name and order id.
                    //as last resort only
                    if (o == null)
                    {
                        string fileId = file.Name.Split('_')[0];
                        o = orders.Find(p => p.id == fileId);

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
                

                    //for(int index = 0; index < 15; index++)
                    //{
                    //    o.toppers.Add(o.toppers[0]);
                    //}


                    pdfMerge.AddPage(pdfWrite.Pages[i]);

                    XGraphics gfx = XGraphics.FromPdfPage(pdfMerge.Pages[pdfMerge.PageCount - 1]);
                    XRect rect = new XRect(0, gfx.PageSize.Height / 2 - 15, gfx.PageSize.Width, gfx.PageSize.Height / 2 + 15);


                    //for(int index = 0;index < 30; index++)
                    //{
                    //    o.toppers.Add(o.toppers[0]);
                    //}


                    //lower half of the page
                    gfx.DrawRectangle(XBrushes.White, rect);
                    //Write the country in the middle
                    gfx.DrawString(o.country+o.note, new XFont("Times New Roman", 12, XFontStyle.Regular), XBrushes.Black, gfx.PageSize.Width *0.5f , gfx.PageSize.Height * 0.5f+65, XStringFormats.Center);

                    if (!WriteOnPage(gfx, o.toppers))
                    {
                        errorMessages.Add($"Couldn't write on page {i + 1} for:\r\n{file.Name}\r\n");
                        failed++;
                        continue;
                    }
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
        /// <returns>A Shipment object representing the completed ordeal.</returns>
        /// <exception cref="ArgumentException">Selected files had incorrect names.</exception>
        /// <exception cref="FileNotFoundException">Selected files could not be found.</exception>
        public Shipment WriteOnOrders(List<FileInfo> unzippedList, List<Order> orders, string saveDir, string saveName)
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
            return new Shipment(orders,unzippedList,returnPath);
        }


        /// <summary>
        /// Modifies the name of the topper, if it is in the special swaps dictionary.
        /// Otherwise, it trims the worthless words out.
        /// </summary>
        /// <param name="tId">The Product Number (PN) of the product</param>
        /// <param name="tName">The original name of said product</param>
        /// <returns>The name with </returns>
        private string ModifyName(string tId, string tName)
        {
            //Sa vad ce naiba fac cu numele in bulgara, cum le editez 
            if (SpecialSwaps.ContainsKey(tId))
            {
                return tName = SpecialSwaps[tId];
            }
            /*
             омплект украса за торта KZE Prints Пес Патрул/ Paw Patrol, Гланцова хартия, Многоцветен, 17 бр
             */

            string[] list = { "briose de ", "briose ", "tort ", };
            foreach (string s in list)
            {
                int index = tName.LastIndexOf(s);
                if (index > 0)
                {
                    return tName = tName.Substring(index + s.Length).Replace(", KZE Prints, Photo Paper Glossy", "");
                }

            }

            //byte[] bytes = tName.ToCharArray()
            //   .Select(c => (byte)c)
            //   .ToArray();
            //string decodedString = System.Text.Encoding.UTF8.GetString(bytes);
            
            //improvizatie pentru bulgaria  
            if(tName.StartsWith("Комплект украса"))
            {
                return tName.Substring(tName.IndexOf('/'), tName.IndexOf(',') - tName.IndexOf('/')).Trim(',', '/', ' ', '\\');
            }

            return tName;

        }



        /// <summary>
        /// For the given pdf page which represents a whole AWB,
        /// write the order contents: topper count, name and image (if exists).
        /// </summary>
        /// <param name="gfx">AWB graphics to be written and/or drawn on</param>
        /// <param name="toppere">What to write on the page (qnt + name + image)</param>
        /// <returns>True if successfull, false if not</returns>
        private bool WriteOnPage(XGraphics gfx, List<Order.topper> toppere)
        {

            Dictionary<string, XImage> images = new Dictionary<string, XImage>();
            int i = 0;
            WebClient client = new WebClient();
            if (!Directory.Exists(imagesDir))
            {
                Directory.CreateDirectory(imagesDir);
            }

            int perPage = form.drawComboBox.SelectedIndex * 2;

            foreach (Order.topper topper in toppere)
            {
                if (perPage > 0)
                {
                    XImage img = null;
                    if (images.ContainsKey(topper.id))
                    {
                        img = images[topper.id];
                    }
                    else
                    {
                        img = TryFindImage(topper.id);
                        if (img != null)
                        {
                            images.Add(topper.id, img);
                        }
                        else
                        {
                            try
                            {
                                client.DownloadFile($@"https://raw.githubusercontent.com/GtJohnny/PDFillerImages/main/{topper.id}.png", $"{imagesDir}/{topper.id}.png");

                            }
                            catch (WebException)
                            {
                                form.textBox1.Text += $"Couldn't download image for {topper.id}.\r\n";
                            }

                            img = TryFindImage(topper.id);
                        }
                    }

                    //MATH =====>>       (scales with images/row)+ (pageH=90 +30 space)+no out of bounds  
                    if (img != null && perPage >= 2 && i < 3 * perPage)
                        gfx.DrawImage(img, (i % perPage) * (90 + perPage * 12) + 20 + (perPage == 2 ? gfx.PageSize.Width / 2 : 20), (i / perPage) * 120 + gfx.PageSize.Height / 2 + 75, 90, 90);
                }

                string temp_name = $"{topper.quantity}:{topper.name}";
                if (perPage == 4)
                {
                    //MATH =====>>
                    //per position *  (pageH=90 +30 space + space with img/row) - (center text) + (even abscise per img/row (2= right column, 3=wide)
                    gfx.DrawString(temp_name, new XFont("Times New Roman", 12, XFontStyle.Regular), XBrushes.Black, (i % perPage) * (90 + perPage * 12) + 65 - 5.7f * (topper.name.Count() / 2) + (perPage == 2 ? gfx.PageSize.Width / 2 : 16), (perPage == 2 ? 250 : 100) + (i / perPage) * 120 + gfx.PageSize.Height / 2 + 75);
                    if(i == 3 * perPage)
                    {
                        return true;
                    }
                }
                else
                {
                    gfx.DrawString(temp_name, new XFont("Times New Roman", 12, XFontStyle.Regular), XBrushes.Black, 50, gfx.PageSize.Height * 0.60f + 15 * i);
                    if(i == 20)
                    {
                        return true;
                    }
                }
                i++;

            }
            return true;






            //if (gfx == null || toppere == null)
            //{
            //    return true;
            //}
            //try
            //{
            //    //      XGraphics gfx = XGraphics.FromPdfPage(page);

            //    XFont font = new XFont("Times New Roman", 14);
            //    XSolidBrush brush = new XSolidBrush(XColor.FromKnownColor(XKnownColor.Black));


            //    int i = 0;

            //    /*
            //    ///This WILL dissappear from here.
            //    DirectoryInfo imagesDir = new DirectoryInfo("C:\\Users\\KZE PC\\Desktop\\VIsual studio projects\\PDFiller\\bin\\Debug\\images\\");
            //    FileInfo[] images  = imagesDir.GetFiles("*.jpg");
            //    Random rng = new Random(i);
            //    */



            //    foreach (var topper in toppere)
            //    {

            //        gfx.DrawString($"{topper.quantity} buc: {topper.name}", font, brush, 25, gfx.PageSize.Height / 2 + 25 + 20 * i, XStringFormats.CenterLeft);
            //        // gfx.DrawImage(img, page.Width - 95 * (nrImagini / 4) + 95 * (j / 4), page.Height / 2 + 20 + 95 * (j % 4), 90, 90);
            //        i++;

            //        if (i == 20) break;
            //    }




            //}
            //catch (Exception ex)
            //{
            //    throw ex;
            //}
            //return true;
        }


        public void ZStartTest()
        {
            //     var cli = new WebClient();
            //     cli.DownloadFile("")


            WebClient client = new WebClient();
            byte[] image = client.DownloadData(@"https://raw.githubusercontent.com/GtJohnny/PDFillerImages/main/5941933302180.png");
            //File.WriteAllBytes($"5941933302180.png", image);



        }


        private XImage TryFindImage(string tId)
        {
            string[] extensions = { ".png", ".jpeg", ".jpg" };
            foreach (string ext in extensions)
            {
                if (File.Exists($"{imagesDir}\\{tId}{ext}"))
                {
                    return XImage.FromFile($"{imagesDir}\\{tId}{ext}");
                }
            }
            return null;
        }



    }
}
