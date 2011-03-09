using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO.Ports;
using System.IO;


namespace mix_weeg_service
{
    class main
    {
        public string ServerDNS;
        public string ServerID;
        public double GewichtsClasse;
        public int portnumbers;
        public ArrayList WeegArray = new ArrayList();
        //public TcpClientWeeg tcpweeg;

        //public Weeg[] WeegArray;
        public main()
        {
        }
        public void init()
        {

            ReadFromFile("c:\\weegservice.ini");
            Console.WriteLine(ServerDNS, Environment.NewLine);
            Console.WriteLine(ServerID, Environment.NewLine);
            Console.WriteLine(GewichtsClasse.ToString(), Environment.NewLine);
            //tcpweeg = new TcpClientWeeg(ServerDNS);
            try
            {

                int i = 0;
                foreach (string s in SerialPort.GetPortNames())
                {
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
            try
            {
                int j = 0;
                int k = portnumbers;
                serial_port[] serial = new serial_port[k];
                serial_port.COMServer[] MyData = new serial_port.COMServer[k];
                while (j < k)
                {
                    Weeg tmpweeg = new Weeg(j, ServerDNS, ServerID);
                    WeegArray.Add(tmpweeg);
                    serial_port tmpserial = new serial_port();
                    serial_port.COMServer tmpdata = new serial_port.COMServer(ParseData);
                    serial[j] = tmpserial;
                    serial[j].init(j);
                    MyData[j] = tmpdata;
                    serial[j].AddDelegate(MyData[j]);
                    Console.WriteLine("serial delegated", Environment.NewLine);
                    j++;
                }
                int i = 0;
                while (true)
                {
                    Thread.Sleep(1);
                    i++;
                    if (i >= 500)
                    {
                        Console.WriteLine("pauze", Environment.NewLine);
                        i = 0;
                    }
                }
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
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
                string[] Sdata = data.Split(' ');
                if (Sdata[0].Contains("-"))
                {
                    Weight = 0;
                }
                else
                {
                    Weight = Convert.ToDouble(Sdata[0]) * GewichtsClasse;
                }
                Weeg tmpweeg = ((Weeg)WeegArray[portnr]);
                tmpweeg.ParseStableWeight(Weight);
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
                Console.WriteLine("error parse");
            }
            //int i = 0;
            //foreach (string word in SWeight)
            //{
            //    Console.Write(i.ToString());
            //    Console.WriteLine(word);
            //    i++;
            //}

        }
        public void ReadFromFile(string filename)
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
                            GewichtsClasse = Convert.ToInt32(Sdata[1]);
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
