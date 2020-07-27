using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Inventory
{
    [ProtoContract]
    public class LootItem:Data.Items.Item
    {
        [ProtoMember(1)]
        float Probability;
    }
}
