using System;
using System.Collections.Generic;

using System.Text;

using NetworkCommsDotNet;
using ProtoBuf;
namespace Data
{
    [ProtoContract]
    public class cPackage
    {
        [ProtoMember(1)]
        public String SessionID = "";
    }
}
