using CoreNet.Protocols;
using MmoCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmoCore.Packets
{
    public interface ISerializePacket
    {
        void SerRead();
        void SerWrite();
    }

    public class MmoCorePacket : Packet, ISerializePacket
    {
        public CONTENT_TYPE cType { get; set; } = CONTENT_TYPE.NONE;

        public MmoCorePacket(PACKET_TYPE _pt, CONTENT_TYPE _ct)
            : base(128)
        {
            pType = _pt;
            cType = _ct;
        }

        public MmoCorePacket(Packet _p) : base(_p)
        {
            cType = Translate.Read<CONTENT_TYPE>(data);
        }
        
        //this method must call Packet(header, data) constructor
        public virtual void SerRead()
        {
            pType = Translate.Read<PACKET_TYPE>(data);
            cType = Translate.Read<CONTENT_TYPE>(data);
        }

        public virtual void SerWrite()
        {
            Translate.Write(data, pType);
            Translate.Write(data, cType);
        }
    }

    public class HBNoti : MmoCorePacket
    {
        public HBNoti()
            : base(PACKET_TYPE.NOTI, CONTENT_TYPE.HB_CHECK)
        {
        }
        public override void SerRead()
        {
            base.SerRead();
        }

        public override void SerWrite()
        {
            base.SerWrite();
        }
    }

    public class HelloReq : MmoCorePacket
    {
        public HelloReq()
            : base(PACKET_TYPE.REQ, CONTENT_TYPE.HELLO)
        {
        }
        public override void SerRead()
        {
            base.SerRead();
        }

        public override void SerWrite()
        {
            base.SerWrite();
        }
    }

    public class WelcomeAns : MmoCorePacket
    {
        public long sId { get; set; }
        public WelcomeAns()
            : base(PACKET_TYPE.ANS, CONTENT_TYPE.WELCOME)
        {
        }
        public override void SerRead()
        {
            base.SerRead();
            sId = Translate.Read<long>(data);
        }

        public override void SerWrite()
        {
            base.SerWrite();
            Translate.Write(data, sId);
            UpdateHeader();
        }
    }

    public class ChatNoti: MmoCorePacket
    {
        public string msg { get; set; }
        public ChatNoti()
            : base(PACKET_TYPE.NOTI, CONTENT_TYPE.CHAT)
        {
        }
        public override void SerRead()
        {
            base.SerRead();
            msg = Translate.Read<string>(data);
        }

        public override void SerWrite()
        {
            base.SerWrite();
            Translate.Write(data, msg);
            UpdateHeader();
        }
    }
}
