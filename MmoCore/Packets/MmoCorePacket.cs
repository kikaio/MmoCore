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

        public MmoCorePacket()
        {
        }

        public MmoCorePacket(Packet _p) : base(_p)
        {
            cType = Translate.Read<CONTENT_TYPE>(data);
        }

        public MmoCorePacket(PACKET_TYPE _pt, CONTENT_TYPE _ct)
            : base(128)
        {
            pType = _pt;
            cType = _ct;
        }


        //this method must call Packet(header, data) constructor
        public virtual void SerRead()
        {
        }

        public virtual void SerWrite()
        {
            Translate.Write<PACKET_TYPE>(data, pType);
            Translate.Write<CONTENT_TYPE>(data, cType);
        }
    }

    public class HelloReq : MmoCorePacket
    {
        public HelloReq()
            : base(PACKET_TYPE.REQ, CONTENT_TYPE.HELLO)
        {
        }
        public HelloReq(MmoCorePacket _mp)
        {
            pType = _mp.pType;
            cType = _mp.cType;
            data = _mp.data;
            header = _mp.header;
        }

        public override void SerRead()
        {
        }

        public override void SerWrite()
        {
            base.SerWrite();
            UpdateHeader();
        }
    }

    public class WelcomeAns : MmoCorePacket
    {
        public long sId { get; set; }
        public WelcomeAns()
            : base(PACKET_TYPE.ANS, CONTENT_TYPE.WELCOME)
        {
        }

        public WelcomeAns(MmoCorePacket _mp)
        {
            pType = _mp.pType;
            cType = _mp.cType;
            data = _mp.data;
            header = _mp.header;
        }

        public override void SerRead()
        {
            sId = Translate.Read<long>(data);
        }

        public override void SerWrite()
        {
            base.SerWrite();
            Translate.Write(data, sId);
            UpdateHeader();
        }
    }

    public class ChatNoti : MmoCorePacket
    {
        public string msg { get; set; }
        public ChatNoti()
            : base(PACKET_TYPE.NOTI, CONTENT_TYPE.CHAT)
        {
        }
        public ChatNoti(MmoCorePacket _mp)
        {
            pType = _mp.pType;
            cType = _mp.cType;
            data = _mp.data;
            header = _mp.header;
        }

        public override void SerRead()
        {
            msg = Translate.Read<string>(data);
        }

        public override void SerWrite()
        {
            base.SerWrite();
            Translate.Write(data, msg);
            UpdateHeader();
        }
    }

    public class DHKeyReq : MmoCorePacket
    {
        public string dhKey { get; set; }
        public DHKeyReq()
            :base(PACKET_TYPE.REQ, CONTENT_TYPE.DH_KEY_CHECK)
        {
        }
        public DHKeyReq(MmoCorePacket _mp)
        {
            pType = _mp.pType;
            cType = _mp.cType;
            data = _mp.data;
            header = _mp.header;
        }

        public override void SerRead()
        {
            dhKey = Translate.Read<string>(data);
        }
        public override void SerWrite()
        {
            base.SerWrite();
            Translate.Write(data, dhKey);
            UpdateHeader();
        }
    }

    public class DHKeyAns : MmoCorePacket
    {
        public DHKeyAns()
            : base(PACKET_TYPE.ANS, CONTENT_TYPE.DH_KEY_CHECK)
        {
        }

        public DHKeyAns(MmoCorePacket _mp)
        {
            pType = _mp.pType;
            cType = _mp.cType;
            data = _mp.data;
            header = _mp.header;
        }

        public override void SerWrite()
        {
            base.SerWrite();
            UpdateHeader();
        }
    }

}
