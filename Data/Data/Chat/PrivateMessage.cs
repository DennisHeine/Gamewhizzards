using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Chat
{
    [ProtoContract]
    public class PrivateMessageChat : DataObject
    {
        [ProtoMember(1)]
        Characters.CharacterInstances.PlayerFightingCharacterInstance Member1;
        [ProtoMember(2)]
        Characters.CharacterInstances.PlayerFightingCharacterInstance Member2;
        [ProtoMember(3)]
        ChatMessage[] History;
    }
}
