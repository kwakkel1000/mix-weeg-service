﻿using System;
using System.Collections.Generic;
using System.Text;

namespace mix_weeg_service
{
    class Weeg
    {
        public string ServerID;
        private double MinGewicht = 40;
        public int Hoog = 0;
        public Double Gew0 = 0;
        public Double Gew1 = 0;
        public Double Gew2 = 0;
        public Double Gew3 = 0;
        public Double Gew4 = 0;
        public Double Gew5 = 0;
        public Double Gew6 = 0;
        public Double Gew7 = 0;
        public Double Gew8 = 0;
        public Double Gew9 = 0;
        public Double Gew10 = 0;
        public Double Gew11 = 0;
        public int Com = -1;
        public TcpClientWeeg tcpweeg;

        public Weeg(int com, string server, string Serverid)
        {
            Com = com;
            tcpweeg = new TcpClientWeeg(server);
            ServerID = Serverid;
        }
        public void ParseStableWeight(double Weight)
        {
            //Console.WriteLine(Weight.ToString());
            Gew0 = Gew1;
            Gew1 = Gew2;
            Gew2 = Gew3;
            Gew3 = Gew4;
            Gew4 = Gew5;
            Gew5 = Gew6;
            Gew6 = Gew7;
            Gew7 = Gew8;
            Gew8 = Gew9;
            Gew9 = Gew10;
            Gew10 = Gew11;
            Gew11 = Convert.ToDouble(Weight);
            if (Weight < MinGewicht)
            {
                if (Hoog == 1)
                {
                    Hoog = 0;
                    bool match = false;
                    double gewicht = -1;
                    if (System.Math.Abs(Gew0 - Gew1) < 30 && System.Math.Abs(Gew1 - Gew2) < 30)
                    {
                        //Console.WriteLine("test1 ");
                        if (Gew1 > MinGewicht)
                        {
                            gewicht = Gew1;
                            match = true;
                        }
                    }
                    if (System.Math.Abs(Gew2 - Gew3) < 30 && System.Math.Abs(Gew3 - Gew4) < 30)
                    {
                        //Console.WriteLine("test2 ");
                        if (Gew3 > MinGewicht)
                        {
                            gewicht = Gew3;
                            match = true;
                        }
                    }
                    if (System.Math.Abs(Gew4 - Gew5) < 30 && System.Math.Abs(Gew5 - Gew6) < 30)
                    {
                        //Console.WriteLine("test3 ");
                        if (Gew5 > MinGewicht)
                        {
                            gewicht = Gew5;
                            match = true;
                        }
                    }
                    if (System.Math.Abs(Gew6 - Gew7) < 30 && System.Math.Abs(Gew7 - Gew8) < 30)
                    {
                        //Console.WriteLine("test4 ");
                        if (Gew7 > MinGewicht)
                        {
                            gewicht = Gew7;
                            match = true;
                        }
                    }
                    if (match)
                    {
                        //Console.WriteLine("Send Gewicht: ");
                        string senddata;
                        senddata = "<AddGewicht> ";
                        senddata = senddata + ServerID + " ";
                        senddata = senddata + (Com + 1).ToString() + " ";
                        senddata = senddata + gewicht.ToString() + " ";
                        senddata = senddata + "</AddGewicht>\r\n";
                        //Console.WriteLine(senddata);
                        tcpweeg.send(senddata);
                    }
                }
            }
            else
            {
                Hoog = 1;
            }
        }
    }
}
