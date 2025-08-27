using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static PDFiller.Order;

namespace PDFiller
{
    
    internal class ImagesObserver : IObserver<Shipment>
    {



        private Panel _panel;

        Graphics g;
        List<Order> _orders;
        Dictionary<string, SoldProduct> _dict = new Dictionary<string, SoldProduct>();
        private void Paint()
        {

            g.Clear(Color.White);
            if (_orders == null) return;



            int i = 0;
            foreach(var data in _dict)
            {
                g.DrawImage(data.Value.Image, new Rectangle((i % 4) * 150 + 25, (i / 4) * 130 + 10, 100, 100));
                g.DrawString($"{data.Value.Name}", new Font("Times New Roman", 14f), new SolidBrush(Color.Black), (i % 4) * 150 + 70 - 4.5f * data.Value.Name.Length, (i / 4) * 130 + 110);
                i++;
            }

        }
        public ImagesObserver(Panel panel)
        {
            this._panel = panel;
            this.g = panel.CreateGraphics();
            panel.Paint += (sender, e) => this.Paint();
            panel.Invalidate();

        }



        public void OnCompleted()
        {
            _orders = null;
            _dict.Clear();
            _panel.Invalidate();
        }


        public void OnError(Exception error)
        {
            _orders = null;
            _dict.Clear();
            _panel.Invalidate();
        }

        public void OnNext(Shipment shipment)
        {
            this._orders = shipment.Orders;
            _dict.Clear();
            foreach (var order in this._orders)
            {
                foreach (SoldProduct soldProduct in order.products)
                {
                    if (!_dict.ContainsKey(soldProduct.Id))
                    {
                        _dict[soldProduct.Id] = soldProduct;
                    }

                }
            }






            _panel.Invalidate();
        }
    }
}
