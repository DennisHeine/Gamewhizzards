using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters
{
    [ProtoContract]
    public class EnemyCharacter:FightingCharacter
    {
        [ProtoMember(1)]
        public Data.Characters.Inventory.Loot Loot=new Inventory.Loot();
        [ProtoMember(2)]
        public SpellRotationItem[] SpellRoatation= { };
        [ProtoMember(3)]
        public String protoAssetName = "";
        [ProtoMember(4)]
        public Stats.Stats CharacterStats = new Stats.Stats();
        

        [ProtoContract]
        public class SpellRotationItem
        {
            [ProtoMember(1)]
            public Spells.Spell Spell=new Spells.Spell();
            [ProtoMember(2)]
            public int Weight=0;
            [ProtoMember(3)]
            int CastEveryNMinutes=0;
            [ProtoMember(4)]
            int CastOnlyInPhase=0;
        }
    }
}
