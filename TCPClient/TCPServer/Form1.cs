using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperSimpleTcp;


namespace TCPServer
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;
        private void buttonStart_Click(object sender, EventArgs e)
        {
            server.Start();
            textBoxInfo.Text += $"Starting...{Environment.NewLine}";
            buttonStart.Enabled = false;
            buttonSend.Enabled = true;


        }

        private void Server_Load(object sender, EventArgs e)
        {
            buttonSend.Enabled = false;
            server = new SimpleTcpServer(textBoxIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;

        }

        private void Events_DataReceived(object sender, DataReceivedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBoxInfo.Text = $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }

        private void Events_ClientDisconnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBoxInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                listBoxClientIP.Items.Remove(e.IpPort);
            });
        }

        private void Events_ClientConnected(object sender, ConnectionEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                textBoxInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                listBoxClientIP.Items.Add(e.IpPort);

            });

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if(server.IsListening)
            {
                if(!string.IsNullOrEmpty(textBoxMessage.Text) && listBoxClientIP.SelectedItem!=null) //проверяется подключение к серверу и налчие сообщения
                {
                    server.Send(listBoxClientIP.SelectedItem.ToString(), textBoxMessage.Text);
                    textBoxInfo.Text += $"Server: {textBoxMessage.Text}{Environment.NewLine}";
                    textBoxMessage.Text = string.Empty;

                }

            }
        }
    }
}
