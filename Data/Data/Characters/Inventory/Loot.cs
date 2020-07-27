using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Inventory
{
    [ProtoContract]
    public class Loot : DataObject
    {
        [ProtoMember(1)]
        public Dictionary<int, LootItem> Items = new Dictionary<int, LootItem>();
    }
}
