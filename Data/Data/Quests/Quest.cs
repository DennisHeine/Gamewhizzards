using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests
{
    [ProtoContract]
    public class Quest : DataObject
    {
        [ProtoMember(1)]
        public Characters.NPCCharacter QuestGiver;
        [ProtoMember(2)]
        public Characters.NPCCharacter AbgebenBei;
        [ProtoMember(3)]
        public Quest[] PreQuests;
        [ProtoMember(4)]
        public QuestAufgaben[] Aufgaben;
    }
}
