using System;
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
        private Panel _panel;
        Graphics g;
        List<Order> orders;
        Dictionary<string, Bitmap> data = new Dictionary<string, Bitmap>();

        private void Paint() { 
        //{
        //    foreach (FileInfo file in files)
        //    {
        //        Bitmap img = Bitmap.FromFile(file.FullName) as Bitmap;
        //        g.DrawImage(img, new Rectangle((nr % 5) * 130, (nr / 5) * 130, 100, 100));
        //        nr++;
        //    }


            g.Clear(Color.White);
            if (orders != null)
            {
                string path = Environment.CurrentDirectory + @"/Images";
                int i = 0;

                foreach (Order order in orders)
                {
                    foreach(Order.topper topper in order.toppers)
                    {
                        string key = topper.id;
                        if (data.ContainsKey(key))
                        {
                            g.DrawImage(data[key], new Rectangle((i % 5) * 130, (i / 5) * 130, 100, 100));
                            i++;
                        }
                        else
                        {
                            string file_path = $@"{path}/ {key}.png";
                            if (File.Exists(path))
                            {
                                data[key] = Bitmap.FromFile(file_path) as Bitmap;
                            }
                        }
                    }
                    //Bitmap bmp = Bitmap.FromFile(@$"{path}/{order.topp}")
                }
            }
        }
        public ImagesObserver(Panel panel)
        {
            this._panel = panel;
            this.g = panel.CreateGraphics();
            panel.Invalidate();
        }



        public void OnCompleted()
        {
            throw new NotImplementedException();
        }


        public void OnError(Exception error)
        {
           
        }

        public void OnNext(Shipment value)
        {
            throw new NotImplementedException();
        }
    }
}
