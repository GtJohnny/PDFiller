using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    /// <summary>
    /// Object representing the entire shipment of the day, as in 
    /// the orders with their corespinding AWB's and shipment information from the excel spreadsheet.
    /// </summary>
    internal class Shipment
    {
        /// <summary>
        /// The orders post-processing the excel file.
        /// Invalidated if the excel is changed.
        /// </summary>
        private List<Order> orders;


        private List<FileInfo> unzippedList;
        private FileInfo mergedPDF;
        private List<IObserver> _observers;
        

        public Shipment(List<Order> orders)
        {
            this.orders = orders;

        }

    }
}
