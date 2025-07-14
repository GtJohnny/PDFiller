using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFiller
{
    /// <summary>
    /// A concrete observer for the preview functionality.
    /// Will manage the tab ExcelPreview tab in the main window.
    /// And the coresponding data grid view
    /// </summary>
    internal class PreviewObserver:IObserver<Shipment>
    {
        private DataGridView _dataGridView;
        List<Order> _orders;
        public virtual void OnCompleted()
        {
            //No need for our use
        }
        public virtual void OnNext(Shipment shipment)
        {
            //List<Order> orders = shipment.GetOrders();
        }
        public virtual void OnError(Exception error)
        {
            // Maybe log the error or handle it in some way
        }
    }
}
