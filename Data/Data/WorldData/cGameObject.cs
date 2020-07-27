using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.WorldData
{
    [ProtoContract]
    public class cGameObject
    {
        [ProtoMember(1)]
        public String downloadPath;
        [ProtoMember(2)]
        public Data.WorldData.cGameObjectPosition Position;
        [ProtoMember(3)]
        public String Name;
        [ProtoMember(4)]
        public byte[] Mesh;
    }
}
