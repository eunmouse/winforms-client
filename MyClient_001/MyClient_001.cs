using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyClient_001
{
    public partial class frmClient : Form
    {
        private string strIP;
        private int strPort;

        private TcpClient client;

        public frmClient()
        {
            InitializeComponent();
        }

        private void Connect()
        {
            // 클라이언트 연결
            try
            {
                strIP = txtIP.Text;
                strPort = Convert.ToInt32(txtPort.Text);

                // 클라이언트 소켓 연결
                client.Connect(strIP, strPort);
                // IPEndPoint ipAddress = new IPEndPoint(IPAddress.Parse(strIP), strPort);
                // client.Connect(ipAddress);
                writeRtbChat("서버 연결됨...");
            }
            catch (Exception ex)
            {
                Console.WriteLine("에러임 :" + ex.Message);
                writeRtbChat("연결 에러임...");
            }


        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            Thread connectthread = new Thread(Connect); // form 과는 별도 스레드에서 connect 함수 실행
            connectthread.IsBackground = true;
            connectthread.Start();
        }

        private void writeRtbChat(string str)
        {
            if (rtbChat.InvokeRequired == true)
            {
                rtbChat.Invoke((MethodInvoker)(() =>
                {
                    rtbChat.AppendText(str + Environment.NewLine); // 줄바꿈
                }));
            }
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void rtbChat_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnSend_Click(object sender, EventArgs e)
        {

        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblPort_Click(object sender, EventArgs e)
        {

        }

        private void lblIP_Click(object sender, EventArgs e)
        {

        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            // 클라이언트 소켓 생성
            client = new TcpClient();
        }
    }
}
