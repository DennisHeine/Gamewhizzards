using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Stats
{
    [ProtoContract]
    public class Stats : DataObject
    {
        [ProtoMember(1)]
        public int Health=0;
        [ProtoMember(2)]
        public int Mana=0;
        [ProtoMember(3)]
        public int Stamina=0;
        [ProtoMember(4)]
        public int Strength=0;
        [ProtoMember(5)]
        public int Intelligence=0;
        [ProtoMember(6)]
        public int Luck=0;
        [ProtoMember(7)]
        public int Damage = 0;
        [ProtoMember(8)]
        public String Hotkey = "";

        public Stats()
        {
        }

        public Stats(int _Health, int _Mana, int _Stamina, int _Strength, int _Intelligence, int _Luck, int _Damage, String _Hotkey)
        {
            Health = _Health;
            Mana = _Mana;
            Stamina = _Stamina;
            Strength = _Strength;
            Intelligence = _Intelligence;
            Luck = _Luck;
            Damage = _Damage;
            Hotkey = _Hotkey;
        }
    }
}
