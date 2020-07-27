using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;


namespace Data.Characters
{
    [ProtoContract]
    public class PlayerCharacter:FightingCharacter
    {
        [ProtoMember(1)]
        public Inventory.Inventory Inventory = new Inventory.Inventory();
        [ProtoMember(2)]
        public Stats.Stats CharacterStats = new Stats.Stats();
        [ProtoMember(3)]
        public Dictionary<String, Spells.Spell> Spells = new Dictionary<string, Spells.Spell>();
        [ProtoMember(4)]
        public Dictionary<Data.Characters.Equipment.cEquipment.StatType,Data.Items.Item> equipment = new Dictionary<Data.Characters.Equipment.cEquipment.StatType, Data.Items.Item>();
    }
}
