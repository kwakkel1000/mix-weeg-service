using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;

namespace mix_weeg_service
{
    class serial_port
    {

        private COMServer comServer;
        private SerialPort _serialPort;
        private int portnr;

        private bool mRun;

        public serial_port()
        {
            Console.WriteLine("new serial_port()", Environment.NewLine);
            mRun = true;
        }

        public void Stop()
        {
            mRun = false;
        }

        public void init(int j)
        {
            while (mRun)
            {
                Console.WriteLine("new SerialPort()", Environment.NewLine);
                portnr = j;
                _serialPort = new SerialPort();
                int i = 0;
                foreach (string s in SerialPort.GetPortNames())
                {
                    if (j == i)
                    {
                        Console.WriteLine(s);
                        _serialPort.PortName = s;
                    }
                    i++;
                }
                _serialPort.ReadTimeout = 500;
                _serialPort.WriteTimeout = 500;
                _serialPort.BaudRate = 9600;
                _serialPort.Parity = (Parity)Enum.Parse(typeof(Parity), _serialPort.Parity.ToString());
                _serialPort.DataBits = int.Parse(_serialPort.DataBits.ToString());
                _serialPort.StopBits = (StopBits)Enum.Parse(typeof(StopBits), _serialPort.StopBits.ToString());
                _serialPort.Handshake = (Handshake)Enum.Parse(typeof(Handshake), _serialPort.Handshake.ToString());
                _serialPort.Open();
                _serialPort.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                while (_serialPort.IsOpen == true)
                {
                    Thread.Sleep(50);
                }
                Console.WriteLine("closed", Environment.NewLine);
            }
            Console.WriteLine("eind init", Environment.NewLine);
        }

        public void AddDelegate(COMServer tmpcomServer)
        {
            comServer = tmpcomServer;
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(50);
                string data = _serialPort.ReadLine();
                Console.WriteLine(data, Environment.NewLine);
                if (comServer != null)
                {
                    comServer(portnr, data);
                }
                else
                {
                    Console.WriteLine("comServer == null", Environment.NewLine);
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
            }
        }      
    }
}
