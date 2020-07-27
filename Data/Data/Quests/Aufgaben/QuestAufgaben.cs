using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Quests
{
    [ProtoContract]
    public class QuestAufgaben : DataObject
    {
        [ProtoMember(1)]
        public bool Done = false;
        [ProtoContract]
        public enum AufgabeType
        {
            KILL_X,
            COLLECT_X,
            BRING_ITEM,
            TALK_TO
        }
    }
}
