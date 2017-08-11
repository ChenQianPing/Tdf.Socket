using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tdf.Socket.Service
{
    public partial class FormService : Form
    {
        public FormService()
        {
            InitializeComponent();

            var serThread = new Thread(new ThreadStart(Listen));
            serThread.Start();
        }

        private void Listen()
        {
            // 包含了一个IP地址
            var ip = IPAddress.Parse("127.0.0.1");

            // 包含了一对IP地址和端口号
            var iep = new IPEndPoint(ip, 8500);

            /* 
             * 创建一个Socket；
             * 
             * 1.AddressFamily.InterNetWork：使用 IP4地址。
             * 
             * 2.SocketType.Stream：支持可靠、双向、基于连接的字节流，
             * 而不重复数据。此类型的 Socket 与单个对方主机进行通信，
             * 并且在通信开始之前需要远程主机连接。
             * Stream 使用传输控制协议 (Tcp) ProtocolType 和 InterNetworkAddressFamily。
             * 
             * 3.ProtocolType.Tcp：使用传输控制协议。
             */
            var socket = new System.Net.Sockets.Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 绑定一个本地的IP和端口号（IPEndPoint），socket监听哪个端口
            socket.Bind(iep);

            // 定义byte数组存放从客户端接收过来的数据
            var buffer = new byte[1024 * 1024];
            
            // 同一个时间点过来10个客户端，排队
            socket.Listen(10);

            lblMsg.Text = string.Format("开始在{0}{1}上监听", iep.Address.ToString(), iep.Port.ToString());

            System.Net.Sockets.Socket client;
            int recv;

            while (true)
            {

                // 接收连接并返回一个新的Socket
                client = socket.Accept();

                // 将接收过来的数据放到buffer中，并返回实际接受数据的长度
                recv = client.Receive(buffer);

                // 将字节转换成字符串
                var cliMsg = Encoding.UTF8.GetString(buffer, 0, recv);

                cliMsg = DateTime.Now.ToShortTimeString() + "," + client.RemoteEndPoint + "发来信息：" + cliMsg;

                this.listBox1.Items.Add(cliMsg);

                var serMsg = "服务器返回信息：OK.";
                var serByte = Encoding.UTF8.GetBytes(serMsg);

                // 输出数据到Socket
                client.Send(serByte);
            }

        }


        private void FormService_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}
