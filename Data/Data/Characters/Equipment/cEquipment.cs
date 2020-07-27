using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Equipment
{
    [ProtoContract]
    public class cEquipment
    {
        [ProtoContract]
        public enum StatType
        {
            EQU_LEFT_HAND            
        }

        [ProtoMember(1)]
        public Dictionary<StatType, Data.Items.Item> Items = new Dictionary<StatType, Items.Item>();
    }
}
