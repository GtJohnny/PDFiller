using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace PDFiller
{
    /// <summary>
    /// Object representing the entire shipment of the day, as in 
    /// the orders with their corespinding AWB's and shipment information from the excel spreadsheet.
    /// </summary>
    internal class Shipment:IObservable<Shipment>
    {
        /// <summary>
        /// The orders post-processing the excel file.
        /// Invalidated if the excel is changed.
        /// </summary>
        private List<Order> orders;
        private List<FileInfo> unzippedList;
        private FileInfo mergedPDF;
        private List<IObserver<Shipment>> _observers=  new List<IObserver<Shipment>>();

        public Shipment(List<Order> orders, List<FileInfo> unzippedList, FileInfo mergedPDF)
        {
            this.orders = orders;
            this.unzippedList = unzippedList;
            this.mergedPDF = mergedPDF;
        }
        /// <summary>
        /// Add candidate to observers list.
        /// </summary>
        /// <param name="observer">IObserver to subscribe</param>
        /// <returns>An IDisposable refference if we want to unsubscribe the user later.</returns>
        public IDisposable Subscribe(IObserver<Shipment> observer)
        {
            if (!_observers.Contains(observer))
            {
                this._observers.Add(observer);
            }
            return new Unsubscriber(_observers, observer);
        }
        /// <summary>
        /// Notify all subscribers based on current state.
        /// </summary>
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(this);
            }
        }

        /// <summary>
        /// Notify all observers with an Exception.
        /// </summary>
        /// <param name="error"></param>
        private void NotifyError(Exception error)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        /// <summary>
        /// Gets the list of orders.
        /// </summary>
        public List<Order> Orders
        {
            get { return orders; }
        }

        /// <summary>
        /// Gets the list of unzipped files.
        /// </summary>
        public List<FileInfo> UnzippedList
        {
            get { return unzippedList; }
        }

        /// <summary>
        /// Gets the merged PDF file.
        /// </summary>
        public FileInfo MergedPDF
        {
            get { return mergedPDF; }
        }
    }
}
