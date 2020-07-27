using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    [ProtoContract]
    public class cPositionData
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
        public String SessionID;
        [ProtoMember(9)]
        public long lastUpdate = 0;
        [ProtoMember(10)]
        public bool moving;
        [ProtoMember(11)]
        public String target=null;
        [ProtoMember(12)]
        public Characters.PlayerCharacter character=new Characters.PlayerCharacter();
       
    }
}
