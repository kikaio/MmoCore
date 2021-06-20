using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TestClientr;

namespace TestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("client is start");
            Client.Inst.ReadyToStart();
            Client.Inst.Start();

            while (Client.Inst.isDown == false)
            {
                Thread.Sleep(1000 * 3);
            }
            Console.WriteLine("client is down, press any key");
            Console.ReadKey();
        }
    }
}
