using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.CharacterInstances
{
    [ProtoContract]
    public class CharacterInstance : DataObject
    {
        [ProtoMember(1)]
        public Characters Character;
        [ProtoMember(2)]
        public String Name;
        [ProtoMember(3)]
        public String ID;
        [ProtoMember(4)]
        public States.StatesCollection State;
    }
}
