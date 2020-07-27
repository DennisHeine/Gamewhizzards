using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters
{
    [ProtoContract]
    public class NPCCharacter : Characters
    {
        [ProtoMember(1)]
        public Quests.Quest[] Quests;
        [ProtoMember(2)]
        public Dialogs.Dialog[] Dialogs;
    }
}
