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

namespace PDFiller
{
    
    internal class ImagesObserver : IObserver<Shipment>
    {
        //private struct TopperData
        //{
        //    public string name;
        //    public Bitmap bmp;
        //    public TopperData(string name, Bitmap bmp)
        //    {
        //        this.name = name;
        //        this.bmp = bmp;
        //    }
        //}
        private Hashtable _toppers = new Hashtable();
        private Panel _panel;
        Graphics g;
        List<Order> orders;
        //Dictionary<string, TopperData> data = new Dictionary<string, TopperData>();
        string path = Environment.CurrentDirectory + @"/Images";


        private void DrawTopper(Order.topper topper,int i)
        {
            string key = topper.id;
            if (_toppers.ContainsKey(key))
            {

                return;
            }
            else
            {
                string file_path = $@"{path}/ {key}.png";
                if (File.Exists(path))
                {
                    Bitmap bmp = Bitmap.FromFile(file_path) as Bitmap;
                    //data[key] = new TopperData(topper.name, bmp);
                    g.DrawImage(bmp, new Rectangle((i % 4) * 130 + 10, (i / 4) * 130 + 10, 100, 100));
                    g.DrawString($"{topper.quantity}.{topper.name}", _panel.Font, new SolidBrush(Color.Black), (i % 4) * 130 + 55, (i / 4) * 130 + 110);
                }
            }
        }
        private void Paint()
        {
            g.Clear(Color.White);
            if (orders == null) return;

            int i = 0;

            foreach (Order order in orders)
            {
                foreach (Order.topper topper in order.toppers)
                {
                    DrawTopper(topper,i++);
                }
            }

        }
        public ImagesObserver(Panel panel)
        {
            this._panel = panel;
            this.g = panel.CreateGraphics();
            panel.Invalidate();
            panel.Paint += (sender, e) => this.Paint();
        }



        public void OnCompleted()
        {
            throw new NotImplementedException();
        }


        public void OnError(Exception error)
        {
            orders = null;
            _panel.Invalidate();
        }

        public void OnNext(Shipment shipment)
        {
            this.orders = shipment.Orders;
            _toppers.Clear();
            _panel.Invalidate();
        }
    }
}
