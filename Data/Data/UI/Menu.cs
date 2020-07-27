using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.UI
{
    [ProtoContract]
    public class Menu: DataObject
    {
        [ProtoMember(1)]
        String ID;
        [ProtoMember(2)]
        MenuItem[] Items;
    }
}
