using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.Inventory
{
    [ProtoContract]
    public class InventoryItem:Data.Items.Item
    {
        [ProtoMember(1)]
        int Position;
        [ProtoMember(2)]
        int StackSize;
    }
}
