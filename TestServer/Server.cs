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

namespace TestServer
{
    public class Server : CoreNetwork
    {
        public static Server Inst { get; } = new Server();

        CoreTCP mListen;
        private Dictionary<string, Worker> wDict = new Dictionary<string, Worker>();
        private List<CoreSession> expiredList = new List<CoreSession>();

        private void Logging(string _msg)
        {
            Console.WriteLine(_msg);
        }


        public override void ReadyToStart()
        {
            MmoTranslate.Init();

            wDict["pkg"] = new Worker();
            wDict["pkg"].PushJob(new JobInfinity(async()=> {
                if (isDown)
                    return;
                while(true)
                {
                    var pkg = packageQ.pop();
                    if (pkg == default(Package))
                        break;
                    Logging($"{pkg.session.SessionId} dispatch start");
                    pkg.Packet.data.RenderBytes();
                    PackageDispatcher(pkg);
                }
                packageQ.Swap();

                foreach (var s in SessionMgr.Inst.ToSessonList())
                {
                    if (s.IsExpireHeartBeat())
                        expiredList.Add(s);
                }
                foreach (var e in expiredList)
                {
                    var del = default(CoreSession);
                    SessionMgr.Inst.CloseSession(e.SessionId, out del);
                    if (del != default(CoreSession))
                    {
                        //
                    }
                }
                expiredList.Clear();
            }));

            mListen = new CoreTCP();
            mListen.Sock.Bind(new IPEndPoint(IPAddress.Any, 30000));
            mListen.Sock.Listen(100);
        }

        public override void Start()
        {
            Task.Factory.StartNew(async () => {

                foreach (var ele in wDict)
                {
                    Logging($"{ele.Key} is start");
                    ele.Value.WorkStart();
                }

                Logging("Accept start");
                while (isDown == false)
                {
                    Socket s = mListen.Sock.Accept();
                    var sid = SessionMgr.Inst.GetNextSessionId();
                    CoreSession newClient = new CoreSession(sid, new CoreTCP(s));
                    SessionMgr.Inst.AddSession(newClient);

                    Task.Factory.StartNew(async () => {
                        var id = sid;
                        var session = default(CoreSession);
                        SessionMgr.Inst.GetSession(id, out session);
                        if (session == default(CoreSession))
                        {
                            Logging("session is null");
                            return;
                        }
                        while (session.Sock.Sock.Connected)
                        {
                            var p = await session.OnRecvTAP();
                            if (p.GetHeader() == 0)
                            {
                                session.UpdateHeartBeat();
                                Logging("hb noti recved");
                            }
                            else
                            {
                                packageQ.Push(new Package(session, p));
                                Logging("packet recved");
                            }
                        }
                    });
                }
            }, TaskCreationOptions.DenyChildAttach);
        }

        protected override void Analizer_Ans(CoreSession _s, Packet _p)
        {
            MmoCorePacket mp = new MmoCorePacket(_p);
            Logging($"{mp.pType}|{mp.cType} Recved");
        }

        protected override void Analizer_Noti(CoreSession _s, Packet _p)
        {
            MmoCorePacket mp = new MmoCorePacket(_p);
            Logging($"{mp.pType}|{mp.cType} Recved");
        }

        protected override void Analizer_Req(CoreSession _s, Packet _p)
        {
            MmoCorePacket mp = new MmoCorePacket(_p);
            Logging($"{mp.pType}|{mp.cType} Recved");
        }

        protected override void Analizer_Test(CoreSession _s, Packet _p)
        {
            MmoCorePacket mp = new MmoCorePacket(_p);
            Logging($"{mp.pType}|{mp.cType} Recved");
        }
    }
}
