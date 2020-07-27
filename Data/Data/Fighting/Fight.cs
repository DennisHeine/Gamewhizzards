using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Fighting
{
    [ProtoContract]
    public class Fighting : DataObject
    {
        [ProtoMember(1)]
        public FightingData fightingData;
        [ProtoMember(2)]
        public FightingStatus[] fightingStatusHistory;
        [ProtoContract]
        public class FightingData
        {
            [ProtoMember(1)]
            public Characters.CharacterInstances.FightingCharacterInstance AttackingCharacter;
            [ProtoMember(2)]
            public Characters.CharacterInstances.FightingCharacterInstance DefendingCharacter;
            [ProtoMember(3)]
            public Characters.Skills.Skill CastedSkill;
            [ProtoMember(4)]
            public long CastStartTimestamp;
        }

        [ProtoContract]
        public class FightingStatus
        {
            [ProtoMember(1)]
            public int CooldownLeft;
            [ProtoMember(2)]
            public bool hasHit = false;
        }
    }
}
