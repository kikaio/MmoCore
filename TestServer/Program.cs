using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TestServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("server is start");

            Server.Inst.ReadyToStart();
            Server.Inst.Start();

            while (Server.Inst.isDown == false)
            {
                Thread.Sleep(3 * 1000);
            }

            Console.WriteLine("server is down, press any key");
            Console.ReadKey();

        }
    }
}
