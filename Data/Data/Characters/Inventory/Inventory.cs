using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Inventory
{
    [ProtoContract]
    public class Inventory : DataObject
    {
        [ProtoContract]
        public class ItemData
        {
            [ProtoMember(1)]
            public Items.Item Item = new Items.Item();
            [ProtoMember(2)]
            public int ItemPositions = 0;
            [ProtoMember(3)]
            public int Count = 0;
        }

        [ProtoMember(1)]
        public Dictionary<String, ItemData> Items = new Dictionary<String, ItemData>();
    }
}
