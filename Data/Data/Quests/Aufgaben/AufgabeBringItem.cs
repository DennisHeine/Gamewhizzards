using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests.Aufgaben
{
    [ProtoContract]
    public class AufgabeBringItem : QuestAufgaben
    {
        [ProtoMember(1)]
        public Data.Items.Item ItemToBring;
    }
}
