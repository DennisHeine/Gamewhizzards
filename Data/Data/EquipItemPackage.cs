using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    [ProtoContract]
    public class EquipItemPackage
    {
        [ProtoMember(1)]
        public String SessionID = "";
        [ProtoMember(2)]
        public Data.Items.Item Item;
    }
}
