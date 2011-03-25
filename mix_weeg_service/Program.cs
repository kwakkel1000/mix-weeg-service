using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace mix_weeg_service
{
    class Program
    {
        static Mix mix;
        ~Program()
        {
            mix.Stop();
        }
    
        static void Main(string[] args)
        {
            try
            {
                mix = new Mix();
                mix.init();
                mix.run();
            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.ToString(), Environment.NewLine);
                Console.WriteLine("error afgevangen in program", Environment.NewLine);
                Console.ReadLine();
            }
        }
    }
}
