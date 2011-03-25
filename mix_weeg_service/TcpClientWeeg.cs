using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace mix_weeg_service
{
    class TcpClientWeeg
    {
        private TcpClient tcpclient;
        private string ServerDNS;
        public TcpClientWeeg(string server)
        {
            ServerDNS = server;
        }

        public void Open()
        {
            tcpclient = new TcpClient();
            tcpclient.Connect(ServerDNS, 3001);
        }
        public void send(string str)
        {
            while (tcpclient.Connected == false)
            {
                Open();
            }
            if (tcpclient.Connected == true)
            {
                try
                {
                    //Open();
                    Stream stm = tcpclient.GetStream();
                    ASCIIEncoding asen = new ASCIIEncoding();
                    byte[] data = asen.GetBytes(str);
                    stm.Write(data, 0, data.Length);
                    //Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }
            }
        }
        public void Close()
        {
            tcpclient.Close();
            tcpclient = null;
        }
    }
}
