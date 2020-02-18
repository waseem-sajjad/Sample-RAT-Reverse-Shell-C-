using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;

namespace Sample_RAT
{
    public class ClientsConEventArgs : EventArgs
    {
        public TcpClient NewClient { get; set; }
        public ClientsConEventArgs(TcpClient newClient)
        {
            NewClient = newClient;
        }
    }
    public class TextRecivedEventArgs : EventArgs
    {
        public TcpClient WhoSend { get; set; }
        public string TextReceived { get; set; }
        public TextRecivedEventArgs(TcpClient clientWhoSend, string clientTextRecived)
        {
            WhoSend = clientWhoSend;
            TextReceived = clientTextRecived;
        }
    }
    public class ClientRemoveEventArgs : EventArgs
    {
        public TcpClient RemoveClient { get; set; }
        public ClientRemoveEventArgs(TcpClient removeClient)
        {
            RemoveClient = removeClient;
        }
    }
}