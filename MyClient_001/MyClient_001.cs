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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyClient_001
{
    public partial class frmClient : Form
    {
        private int strPort;

        private TcpClient client;
        private NetworkStream stream;

        public frmClient()
        {
            InitializeComponent();
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            // TcpClient 인스턴스 생성 
            client = new TcpClient();
        }

        private void Connect()
        {
            try
            {
                // 서버와 연결
                strPort = Convert.ToInt32(txtPort.Text);
                client.Connect(txtIP.Text, strPort);
                writeRtbChat("서버 연결됨...");

                // Receive 스레드 생성 
                Thread receiveThread = new Thread(Receive);
                receiveThread.IsBackground = true;
                receiveThread.Start();
    
            }
            catch (Exception ex)
            {
                Console.WriteLine("에러임 :" + ex.Message);
                writeRtbChat("연결 에러임...");
            }
        }

        private void Receive()
        {
            // 스트림 값 받아오기
            stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead;

            // 반환값은 실제로 읽은 바이트 수 (스트림 끝 EOF 도달하면 0 리턴), 연결이 끊어지면 0 반환
            while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
            {
                // 바이트 -> 문자열로 디코딩 
                string receivedChat = Encoding.Default.GetString(buffer, 0, bytesRead);
                writeRtbChat("서버 : " + receivedChat);
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

        private void Send()
        {
            try
            {
                // 서버에 메시지 전송
                string msg = txtMessage.Text;

                // 문자열 -> 바이트로 인코딩
                byte[] byteMsg = Encoding.Default.GetBytes(msg);
                stream.Write(byteMsg, 0, byteMsg.Length);
                writeRtbChat("클라이언트 : " + msg);

                // txtMessage 객체는 UI 스레드(메인 스레드) 에서만 접근 가능하여, Invoke 로 넘겨서 처리 
                if (txtMessage.InvokeRequired == true)
                {
                    txtMessage.Invoke((MethodInvoker)(() =>
                    {
                        txtMessage.Clear();
                    }));
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("에러임 : " + ex.Message);
                writeRtbChat("메시지 전송과정에서 에러발생");
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            // Send 스레드 생성
            Thread sendThread = new Thread(Send);
            sendThread.IsBackground = true;
            sendThread.Start();
        }

        private void txtPort_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void frmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            client.Close();
        }
    }
}
