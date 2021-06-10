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

        public virtual void SerRead()
        {
            pType = (PACKET_TYPE)Translate.Read<ushort>(data);
            cType = Translate.Read<CONTENT_TYPE>(data);
        }

        public virtual void SerWrite()
        {
            Translate.Write(data, (ushort)pType);
            Translate.Write<CONTENT_TYPE>(data, cType);
        }
    }

}
