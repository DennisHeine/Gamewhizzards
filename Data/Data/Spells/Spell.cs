using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Spells
{
    [ProtoContract]
    public class Spell : DataObject
    {
        [ProtoContract]
        public enum SpellType
        {
            SPELL_BUFF,
            SPELL_ATTACK,
            SPELL_HEAL,
            SPELL_ATTACK_DOT,
            SPELL_HEAL_HOT
        }

        [ProtoMember(1)]
        public String Name="";
        [ProtoMember(2)]
        public String Description="";
        [ProtoMember(3)]
        public SpellType Type=SpellType.SPELL_ATTACK;
        [ProtoMember(4)]
        public Data.Characters.Stats.Stats Stats=new Data.Characters.Stats.Stats();
        [ProtoMember(5)]
        public int SpellLevel=0;
        [ProtoMember(6)]
        public String MainObject="";
        [ProtoMember(7)]
        public String SourceObject = "";
        [ProtoMember(8)]
        public String TargetObject= "";
        [ProtoMember(9)]
        public int Cooldown = 0;
        [ProtoMember(10)]
        public int Cooldown_Left = 0;
        [ProtoMember(11)]
        public String Hotkey = "";

        public Spell()
        {

        }

        public Spell(String _Name, String _Description, SpellType _Type, Data.Characters.Stats.Stats _Stats,int _SpellLevel, String _MainObject, String _SourceObject, String _TargetObject, int _Cooldown, String _Hotkey)
        {
            Name = _Name;
            Description = _Description;
            Type = _Type;
            Stats = _Stats;
            SpellLevel = _SpellLevel;
            MainObject = _MainObject;
            SourceObject = _SourceObject;
            TargetObject = _TargetObject;
            Cooldown = _Cooldown;
            Cooldown_Left = Cooldown;
            Hotkey = _Hotkey;                
        }

    }
}
