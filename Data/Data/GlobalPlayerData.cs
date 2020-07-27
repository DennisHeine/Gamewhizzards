using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    [ProtoContract]
    public class GlobalPlayerData
    {
        [ProtoMember(1)]
        public Dictionary<string, cPositionData> players = new Dictionary<string, cPositionData>();

    }
}
