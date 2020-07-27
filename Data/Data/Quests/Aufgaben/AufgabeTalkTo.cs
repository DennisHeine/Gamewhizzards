using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests.Aufgaben
{
    [ProtoContract]
    public class AufgabeTalkTo : QuestAufgaben
    {
        [ProtoMember(1)]
        public Characters.NPCCharacter CharacterToTalkTo;
    }
}
