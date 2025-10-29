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

        public PreviewObserver(DataGridView dataGridView)
        {
            _dataGridView = dataGridView;
            _dataGridView.Rows.Clear();
        }

        public virtual void OnCompleted()
        {
            _dataGridView.Rows.Clear();
        }
        public virtual void OnNext(Shipment shipment)
        {
            List<Order> orders = shipment.Orders;
            var rows = _dataGridView.Rows;
            rows.Clear();
            foreach (Order o in orders)
            {
                rows.Add(o.country, o.customerName, o.products[0].Name, o.products[0].Quantity);
                foreach (SoldProduct product in o.products.GetRange(1, o.products.Count - 1))
                {
                    rows.Add(null,null, product.Name, product.Quantity);
                }
            }
        }
        public virtual void OnError(Exception error)
        {
            _dataGridView.Rows.Clear();
            _dataGridView.Rows.Add(null ,null,error.Message,null);
        }
    }
}
