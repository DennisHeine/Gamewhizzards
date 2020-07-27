using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests.Aufgaben
{
    [ProtoContract]
    public class AufgabenCollection : DataObject
    {
        [ProtoMember(1)]
        public Dictionary<int, QuestAufgaben> Aufgaben = new Dictionary<int, QuestAufgaben>();
    }
}
