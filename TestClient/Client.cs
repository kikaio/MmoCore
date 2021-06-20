using CoreNet.Jobs;
using CoreNet.Networking;
using CoreNet.Protocols;
using CoreNet.Sockets;
using MmoCore.Packets;
using MmoCore.Protocols;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TestClientr
{
    public class Client : CoreNetwork
    {
        public static Client Inst { get; } = new Client();

        private Dictionary<string, Worker> wDict = new Dictionary<string, Worker>();

        private CoreSession mSession;

        private void Logging(string msg)
        {
            Console.WriteLine(msg);
        }

        public override void ReadyToStart()
        {
            MmoTranslate.Init();

            wDict["hb"] = new Worker();
            long deltaTicks = (long)(TimeSpan.FromMilliseconds(CoreSession.hbDelayMilliSec).Ticks*0.75f);
            wDict["hb"].PushJob(new JobNormal(DateTime.MinValue, DateTime.MaxValue, deltaTicks, () => {
                if (isDown)
                    return;
                Task.Factory.StartNew(async () => {
                    var hb = new HeartbeatNoti();
                    await mSession.OnSendTAP(hb);
                });
            }));

 //           wDict["pkg"] = new Worker();
        }

        public override void Start()
        {
            Task.Factory.StartNew(async () => {
                ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 30000);
                CoreTCP tcp = new CoreTCP(AddressFamily.InterNetwork);
                mSession = new CoreSession(-1, tcp);

                Logging($"try conn to server");
                tcp.Sock.Connect(ep);
                Logging($"conn is complete");

                foreach (var ele in wDict)
                {
                    Logging($"{ele.Key} is start");
                    ele.Value.WorkStart();
                }

                while (true)
                {
                    var p = new HelloReq();
                    p.SerWrite();
                    await mSession.OnSendTAP(p);
                    await Task.Delay(10 * 1000);
                }
            });
        }

        protected override void Analizer_Ans(CoreSession _s, Packet _p)
        {
            throw new NotImplementedException();
        }

        protected override void Analizer_Noti(CoreSession _s, Packet _p)
        {
            throw new NotImplementedException();
        }

        protected override void Analizer_Req(CoreSession _s, Packet _p)
        {
            throw new NotImplementedException();
        }

        protected override void Analizer_Test(CoreSession _s, Packet _p)
        {
            throw new NotImplementedException();
        }
    }
}
