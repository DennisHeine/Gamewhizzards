using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests.Aufgaben
{
    [ProtoContract]
    public class AudgabeKill: QuestAufgaben
    {
        [ProtoMember(1)]
        public int NumberToKill;
        [ProtoMember(2)]
        public int AllreadyKilled;
        [ProtoMember(3)]
        public Characters.EnemyCharacter Enemy;
    }
}
