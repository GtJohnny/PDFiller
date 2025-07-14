using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PDFiller
{
    /// <summary>
    /// 
    /// </summary>
    internal class SummaryObserver : IObserver<Shipment>
    {
        private DataGridView _dataGridView;
        public SummaryObserver(DataGridView dataGridView) {
            this._dataGridView = dataGridView;
            this._dataGridView.Rows.Clear();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            _dataGridView.Rows.Clear();
            _dataGridView.Rows.Add(null,error.Message);
        }

        public void OnNext(Shipment shipment)
        {

            _dataGridView.Rows.Clear();
            Dictionary<string, int> dict = new Dictionary<string, int>();
            var orders = shipment.Orders;

            foreach (Order o in orders)
            {
                foreach (Order.topper tp in o.toppers)
                {
                    //KeyValuePair<string, string> key = new KeyValuePair<string, string>(tp.tName, tp.tId);
                    string key = tp.name;

                    if (dict.ContainsKey(key))
                    {
                        dict[key] += tp.quantity;
                    }
                    else
                    {
                        dict[key] = tp.quantity;
                    }
                }
            }
            foreach (var pair in dict)
            {
               
                _dataGridView.Rows.Add(pair.Value, pair.Key);

            }
            _dataGridView.Sort(_dataGridView.Columns[0], ListSortDirection.Descending);

        }
    }
}
