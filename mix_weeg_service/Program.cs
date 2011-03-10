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
        static void Main(string[] args)
        {
            try
            {

                main Main;
                Main = new main();
                Main.init();
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
