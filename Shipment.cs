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
    /// <remarks>
    /// In order to make the flow of the app as lazy as possible,
    /// The <see cref="Shipment"/> object will hold the current state of the orders,
    /// once the excel/pdfs file were processed, to distinguish them from
    /// other files selected by the user.
    /// The <see cref="Shipment"/> object is observable, so any class that needs to
    /// read that state can subscribe to it.
    /// </remarks>
    /// 
    internal class Shipment:IObservable<Shipment>
    {
        /// <summary>
        /// The orders post-processing the excel file.
        /// Invalidated if the excel is changed.
        /// </summary>
        private List<Order> orders;
        private List<FileInfo> unzippedList;
        private string mergedPDF;
        private List<IObserver<Shipment>> _observers=  new List<IObserver<Shipment>>();

        public Shipment()
        {
            this.orders = new List<Order>();
            this.unzippedList = new List<FileInfo>();
            this.mergedPDF = null;
        }
        public Shipment(List<Order> orders, List<FileInfo> unzippedList, string mergedPDF)
        {
            this.orders = orders;
            this.unzippedList = unzippedList;
            this.mergedPDF = mergedPDF;
        }

        /// <summary>
        /// Update the Shipment object with new data.
        /// And automatically notifies subscribers.
        /// </summary>
        /// <param name="shipment">New Shipment object</param>
        public void Update(Shipment shipment)
        {
            this.orders = shipment.orders;
            this.unzippedList = shipment.unzippedList;
            this.mergedPDF = shipment.mergedPDF;
            Notify();
        }

        public void Update(List<Order> orders, List<FileInfo> unzippedList, string mergedPDF)
        {
            this.orders = orders;
            this.unzippedList = unzippedList;
            this.mergedPDF = mergedPDF;
            Notify();
        }

        /// <summary>
        /// Add candidate to observers list.
        /// </summary>
        /// <param name="observer">IObserver to subscribe</param>
        /// <returns>An IDisposable refference if we want to unsubscribe the object later.</returns>
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
       
        public void Notify()//Maybe use aspects too later?
        {
           
            if (orders == null || orders.Count == 0)
            {
                NotifyError(new Exception("Orders list is empty!"));
                return;
            }
          
            foreach (var observer in _observers)
            {
                observer.OnNext(this);
            }
        }

        /// <summary>
        /// Notify all observers with an Exception.
        /// </summary>
        /// <param name="error"></param>
        public void NotifyError(Exception error)
        {
            foreach (var observer in _observers)
            {
                observer.OnError(error);
            }
        }

        /// <summary>
        /// Will clear all observers for the current shipment status.
        /// </summary>
        public void NotifyCompleted()
        {
            this.Orders.Clear();
            foreach (var observer in _observers)
            {
                observer.OnCompleted();
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
        public string MergedPDF
        {
            get { return mergedPDF; }
        }
    }
}
