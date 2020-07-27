using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Chat
{
    [ProtoContract]
    public class ChannelChat : DataObject
    {
        [ProtoMember(1)]
        public String ChannelName;
        [ProtoMember(2)]
        public ChatMessage[] Verlauf;
        [ProtoMember(3)]
        public Characters.CharacterInstances.PlayerFightingCharacterInstance[] Member;
    }
}
