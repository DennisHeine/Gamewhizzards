using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.WorldData
{
    [ProtoContract]
    public class cGameObjectPosition
    {
        [ProtoMember(1)]
        public float x;
        [ProtoMember(2)]
        public float y;
        [ProtoMember(3)]
        public float z;

        [ProtoMember(4)]
        public float rx;
        [ProtoMember(5)]
        public float ry;
        [ProtoMember(6)]
        public float rz;
        [ProtoMember(7)]
        public float rw;

        [ProtoMember(8)]
        public float sx;
        [ProtoMember(9)]
        public float sy;
        [ProtoMember(10)]
        public float sz;        
    }
}
