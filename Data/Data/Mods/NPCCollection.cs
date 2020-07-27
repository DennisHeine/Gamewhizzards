using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mods
{
    [ProtoContract]
    public class NPCCollection : DataObject
    {
        [ProtoMember(1)]
        NPCData[] NPCs;
        [ProtoContract]
        class NPCData
        {
            [ProtoMember(1)]
            public Characters.NPCCharacter NPC;
            [ProtoMember(2)]
            public cPositionData[] SpawnPoints;
        }
    }
}
