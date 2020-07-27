using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Dialogs
{
    [ProtoContract]
    public class Dialog : DataObject
    {
        [ProtoMember(1)]
        public String ID;
        [ProtoMember(2)]
        public Quests.Quest[] QuestsDoneRequired;
        [ProtoMember(3)]
        public Dialog[] DialogsDoneRequired;
        [ProtoMember(4)]
        public bool DialogDone=false;
        [ProtoMember(5)]
        public String DialogText;
    }
}
