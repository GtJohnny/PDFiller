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
        private struct TopperData
        {
            public string name;
            public Bitmap bmp;
            public TopperData(string name, Bitmap bmp)
            {
                this.name = name;
                this.bmp = bmp;
            }
        }
        private Panel _panel;
        Graphics g;
        List<Order> _orders;
        Dictionary<string, TopperData> _toppers = new Dictionary<string, TopperData>();
        string path = Environment.CurrentDirectory + @"\Images";


        private void LoadBMPs()
        {
            foreach (var order in _orders)
            {
                foreach (var topper in order.toppers)
                {
                    string key = topper.id;
                    string file_path = $@"{path}\{key}.png";

                    if (!_toppers.ContainsKey(key) && File.Exists(file_path)){ 
                        
                        Bitmap bmp = Bitmap.FromFile(file_path) as Bitmap;
                        _toppers[key] = new TopperData(topper.name, bmp);
                    }
                }
            }
        }
        private void Paint()
        {
            g.Clear(Color.White);
            if (_orders == null) return;
            if (_toppers.Count == 0) LoadBMPs();
            int i = 0;
            foreach(var data in _toppers)
            {
                g.DrawImage(data.Value.bmp, new Rectangle((i % 4) * 150 + 25, (i / 4) * 130 + 10, 100, 100));
                g.DrawString($"{data.Value.name}", new Font("Times New Roman", 14f), new SolidBrush(Color.Black), (i % 4) * 150 + 70 - 4.5f * data.Value.name.Length, (i / 4) * 130 + 110);
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
            throw new NotImplementedException();
        }


        public void OnError(Exception error)
        {
            _orders = null;
            _toppers.Clear();
            _panel.Invalidate();
        }

        public void OnNext(Shipment shipment)
        {
            this._orders = shipment.Orders;
            _toppers.Clear();
            _panel.Invalidate();
        }
    }
}
