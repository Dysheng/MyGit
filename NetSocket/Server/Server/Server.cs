using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Server : Form
    {
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            string hostname = Dns.GetHostName();
            IPHostEntry ipe = Dns.Resolve(Dns.GetHostName());
            textBox1 .Text =ipe .AddressList [0].ToString ();
        }
        private IPAddress myIP = IPAddress.Parse("127.0.0.1");
        private IPEndPoint MyServer;
        private Socket socket;
        private bool bb = true;
        private Socket nSocket;
        private Thread th;

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                myIP = IPAddress.Parse(textBox1 .Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show("输入的IP不对！");
            }

            try
            {
                MyServer = new IPEndPoint(myIP, Int32.Parse(textBox2.Text));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(MyServer);
                socket.Listen(8880);
                toolStripStatusLabel1.Text = "主机：" + myIP + "端口：" + textBox2.Text + "开始监听";
               th = new Thread(new ThreadStart (target));
                th.Start();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = ex.ToString();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void target()
        {
            try
            {
                nSocket = socket.Accept();
                bb = false;
                if (nSocket.Connected)
                {
                    statusStrip1.Text = "与客户端连接！";
                    Byte[] bytee=new Byte[64];
                    bytee = System.Text.Encoding.Default.GetBytes("欢迎使用本服务器\n".ToString());
                    nSocket.Send(bytee,bytee.Length ,0);
                    while(!bb)
                    {
                        Byte[]bbb=new Byte[64];
                        nSocket.Receive(bbb,bbb.Length ,0);
                        string ccc = System.Text.Encoding.Default.GetString(bbb);
                        richTextBox1.AppendText(ccc+"\r\n");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                Byte [] byee=new Byte[64];
                if (textBox3.Text != "")
                {
                    string send = textBox3.Text + ">>" + richTextBox2.Text + "\r\n";
                    richTextBox1.AppendText(send + "\n");
                    byee = System.Text.Encoding.Default.GetBytes(send.ToCharArray());
                    nSocket.Send(byee, byee.Length, 0);
                    richTextBox2.Clear();
                }
                else
                {
                    MessageBox.Show("输入名称！");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                th.Abort();
                socket.Close();
                nSocket.Close();
                bb = false;
                toolStripStatusLabel1.Text = "主机" + myIP + "端口：" + textBox1.Text + "停止监听";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
