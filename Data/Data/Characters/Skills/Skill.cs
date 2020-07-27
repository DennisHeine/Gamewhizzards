using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Skills
{
    [ProtoContract]
    public class Skill : DataObject
    {
        [ProtoContract]
        public enum SkillTypes
        {
            SKILL_BUFF,
            SKILL_ATTACK
        }
        [ProtoMember(1)]
        public SkillTypes Type;
        [ProtoMember(2)]
        public Stats.Stats BonusesToPlayer;
        [ProtoMember(3)]
        public Stats.Stats DamageDoneToEnemy;
        [ProtoMember(4)]
        public String Animation;
        [ProtoMember(5)]
        public int Cooldown;
        [ProtoMember(6)]
        public int TimeToCast;
        [ProtoMember(7)]
        public String UIImage;
    }
}
