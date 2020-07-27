using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Chat
{
    [ProtoContract]
    public class ChatMessage : DataObject
    {
        [ProtoMember(1)]
        Characters.CharacterInstances.PlayerFightingCharacterInstance From;
        [ProtoMember(2)]
        long Timestamp;
        [ProtoMember(3)]
        String Text;
    }
    [ProtoContract]
    public class ChatMessageChannel:ChatMessage
    {
        [ProtoMember(1)]
        Characters.CharacterInstances.PlayerFightingCharacterInstance To;
    }
}
