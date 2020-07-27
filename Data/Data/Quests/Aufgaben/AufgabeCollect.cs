using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests.Aufgaben
{
    [ProtoContract]
    public class AufgabeCollect : QuestAufgaben
    {
        [ProtoMember(1)]
        public Data.Items.Item ItemToBring;
        [ProtoMember(2)]
        public int NumberToBring;
        [ProtoMember(3)]
        public int NumberCollected;
    }
}
