using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.WorldData
{
    [ProtoBuf.ProtoContract]
    public class cWorldData
    {
        [ProtoMember(1)]
        public List<cGameObject> GameObjects = new List<cGameObject>();
        [ProtoMember(2)]
        public byte[] ByteMetadata = { };
        [ProtoMember(3)]
        public byte[] ByteData = { };
    }
}

