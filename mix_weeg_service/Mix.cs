using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO.Ports;
using System.IO;


namespace mix_weeg_service
{
    public delegate void COMServer(int portnr, string data);
    class Mix
    {
        private string ServerDNS;
        private string ServerID;
        private double GewichtsClasse;
        private int portnumbers;
        private ArrayList WeegArray = new ArrayList();
        private TcpClientWeeg tcpweeg;
        private serial_port[] serial;
        private COMServer[] MyData;
        private Thread[] Threads;
        private bool mRun;

        public Mix()
        {
        }

        public void Stop()
        {
            foreach (serial_port tmpsp in serial)
            {
                tmpsp.Stop();
            }
            mRun = false;
        }

        public void init()
        {
            ReadFromFile("c:\\weegservice.ini");
            Console.WriteLine(ServerDNS, Environment.NewLine);
            Console.WriteLine(ServerID, Environment.NewLine);
            Console.WriteLine(GewichtsClasse.ToString(), Environment.NewLine);
            tcpweeg = new TcpClientWeeg(ServerDNS);
            try
            {
                int i = 0;
                foreach (string s in SerialPort.GetPortNames())
                {
                    Weeg tmpweeg = new Weeg(i, ServerID, tcpweeg);
                    WeegArray.Add(tmpweeg);
                    i++;
                }
                portnumbers = i;
                Console.WriteLine("number of com ports: ");
                Console.WriteLine(i.ToString(), Environment.NewLine);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
            }
            serial = new serial_port[portnumbers];
            MyData = new COMServer[portnumbers];
            Threads = new Thread[portnumbers];
            try
            {
                int i = 0;
                while (i < portnumbers)
                {
                    Thread t = new Thread(AddSerialPort);
                    t.Start(i);
                    Threads[i] = t;
                    i++;
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
            }
            mRun = true;
        }

        private void AddSerialPort(object messageObj)
        {
            int portId = (int)messageObj;
            serial_port tmpserial = new serial_port();
            COMServer tmpdata = new COMServer(ParseData);
            tmpserial.init(tmpdata, portId);
            serial[portId] = tmpserial;
            MyData[portId] = tmpdata;
            Console.WriteLine("serial delegated", Environment.NewLine);
        }

        public void run()
        {
            foreach (Thread t in Threads)
            {
                t.Join();
            }
            while (mRun)
            {
                Thread.Sleep(500);
            }
        }

        public void ParseData(int portnr, string data)
        {
            //Console.WriteLine(portnr.ToString(), Environment.NewLine);
            //Console.WriteLine(data, Environment.NewLine);
            try
            {
                data = data.TrimStart();
                double Weight;
                string StringWeight;
                string[] Sdata = data.Split(' ');
                StringWeight = Sdata[0];
                if (StringWeight.Contains("-"))
                {
                    Weight = 0;
                }
                else
                {
                    StringWeight.Replace(".","");
                    Console.WriteLine(StringWeight, Environment.NewLine);
                    Weight = Convert.ToDouble(StringWeight) * GewichtsClasse;
                }
                Weeg tmpweeg = ((Weeg)WeegArray[portnr]);
                tmpweeg.ParseStableWeight(Weight);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
                Console.WriteLine("error parse");
            }
        }
        private void ReadFromFile(string filename)
        {
            try
            {
                StreamReader SR;
                string S = "";
                SR = File.OpenText(filename);
                while (S != null)
                {
                    S = SR.ReadLine();
                    if (S != null)
                    {
                        Console.WriteLine(S);
                        string[] Sdata = S.Split('=');
                        if (Sdata[0] == "ServerDNS")
                        {
                            ServerDNS = Sdata[1];
                        }
                        if (Sdata[0] == "ServerID")
                        {
                            ServerID = Sdata[1];
                        }
                        if (Sdata[0] == "GewichtsClasse")
                        {
                            GewichtsClasse = Convert.ToDouble(Sdata[1]);
                        }
                    }
                }
                SR.Close();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
            }
        }
    }
}
