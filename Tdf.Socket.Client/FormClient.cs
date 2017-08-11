using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tdf.Socket.Client
{
    public partial class FormClient : Form
    {
        public FormClient()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var client = new Client(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8500));
            lblMsg.Text = @"服务器IP及端口：" + client.Socket.RemoteEndPoint.ToString();
            try
            {
                // 发送消息
                client.Send(tbMsg.Text);
                listBox1.Items.Add($"{client.Socket.LocalEndPoint}发送信息：{tbMsg.Text}");

                // 接收
                var recvMsg = client.Receive();
                listBox1.Items.Add(recvMsg);
                listBox1.Items.Add("");
            }
            finally
            {
                client.Close();
            }

        }

    }
}
