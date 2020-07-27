using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.CharacterInstances
{
    [ProtoContract]
    public class FightingCharacterInstance:FightingCharacter
    {
        [ProtoMember(1)]
        public int CurrentHealth;
        [ProtoMember(2)]
        public int CurrentMana;
        [ProtoMember(3)]
        public bool isDead;
        [ProtoMember(4)]
        public Characters Target;
    }
}
