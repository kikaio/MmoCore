using CoreNet.Networking;
using CoreNet.Protocols;
using MmoCore.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MmoCore.Protocols
{
    public static class MmoTranslate
    {
        private static bool isInit = false;
        public static void Init()
        {
            if (isInit)
                return;
            Translate.RegistCustom<CONTENT_TYPE>((NetStream _s, object _val)
                => {
                    _s.WriteInt32((Int32)_val);
                },
            (NetStream _s) => {
                var ret = default(CONTENT_TYPE);
                ret = (CONTENT_TYPE)_s.ReadInt32();
                return ret;
            });

            isInit = true;
        }
    }
}
