using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net .Sockets ;
using System.Threading;

namespace Client
{
    public partial class Client : Form
    {
        public Client()
        {
            InitializeComponent();
        }
        private IPAddress myIp = IPAddress.Parse("127.0.0.1");
        private IPEndPoint MyServer;
        private Socket sk;
        private bool bb = true;
        private Thread client;
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                myIp = IPAddress.Parse (textBox1 .Text );
                MyServer = new IPEndPoint(myIp ,Int32.Parse (textBox2.Text ));
                sk = new Socket(AddressFamily .InterNetwork,SocketType.Stream ,ProtocolType.Tcp);
                sk.Connect(MyServer);
                toolStripStatusLabel1.Text = "主机："+myIp +"端口："+textBox2 .Text +" 连接成功！";
                  client = new Thread(new ThreadStart(target));
                client.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString ());
            }
        }

        private void target()
        {
            try
            {
                while (bb)
                { 
                  Byte[] str=new  Byte[1024];
                  sk.Receive(str,str.Length,0);
                  string aaa = System.Text.Encoding.Default.GetString(str);
                  richTextBox1.AppendText(aaa+"\n");
                 }
            }
                catch(Exception ex)
            {
                MessageBox.Show(ex.ToString ());
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
              Byte[] sd=new Byte[1024];
              string send = textBox3.Text + ">>" + richTextBox2.Text + "\r\n";
              richTextBox1.AppendText(send +"\n");
              sd = System.Text.Encoding.Default.GetBytes(send .ToArray());
              sk.Send(sd,sd.Length ,0);
              richTextBox2.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString ());
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                Byte [] bytee=new Byte[64];
                string sd = "--@--" + "\r\n";
                bytee = System.Text.Encoding.Default.GetBytes(sd.ToCharArray());
                sk.Send(bytee ,bytee.Length ,0);
                client.Abort();
                sk.Close();
                bb = false;
                toolStripStatusLabel1.Text = "主机：" + myIp + "端口：" + textBox2.Text + " 关闭成功！";
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString ());
            }
        }

       

    }
}
