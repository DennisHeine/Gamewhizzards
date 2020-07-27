using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Data.Items
{
    [ProtoContract]
    public class Item: DataObject
    {
        public Item(String _UIImagePath, String _MeshNameAndPath, int _minLevel, int _maxLevel, ItemType _itemType, Data.Characters.Stats.Stats _ItemStats)
        {
            UIImage = _UIImagePath; MeshNameAndPath = _MeshNameAndPath; MinLevel = _minLevel; MaxLevel = _maxLevel; ItemStats = _ItemStats;Type = _itemType;
            
        }
        
        public Item()
        {
            
        }

       [ProtoContract]
        public enum ItemType
        {
            ITEM_WEAPON,
            ITEM_ARMOR,
            ITEM_MAGICAL,
            ITEM_CONSUMABLE
        }

        [ProtoMember(1)]
        public String UIImage="";
        [ProtoMember(2)]
        public String MeshNameAndPath = "";
        [ProtoMember(3)]
        public int MinLevel=0;
        [ProtoMember(4)]
        public int MaxLevel=0;
        [ProtoMember(5)]
        public ItemType Type=ItemType.ITEM_WEAPON;
        [ProtoMember(6)]
        public Data.Characters.Stats.Stats ItemStats=new Data.Characters.Stats.Stats();
        [ProtoMember(7)]
        public String Name="";
        [ProtoMember(8)]
        public Data.Characters.Equipment.cEquipment.StatType EquipmentPosition = Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND;
        [ProtoMember(9)]
        public String ItemID = "";
        [ProtoMember(10)]
        public float x=0;
        [ProtoMember(11)]
        public float y = 0;
        [ProtoMember(12)]
        public float z = 0;

    }   
}
