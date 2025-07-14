using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDFiller
{
    internal class Unsubscriber : IDisposable
    {
        List<IObserver<Shipment>> subscribers;
        IObserver<Shipment> subscriber;
        public void Dispose() { }
        public Unsubscriber(List<IObserver<Shipment>> subscribers, IObserver<Shipment> subscriber)
        {
            this.subscribers  = subscribers;
            this.subscriber = subscriber;
        }
        public void Unsubscribe()
        {
            if (subscribers != null && subscribers.Contains(subscriber)) {
                subscribers.Remove(subscriber);
            }
        }
    }
}
