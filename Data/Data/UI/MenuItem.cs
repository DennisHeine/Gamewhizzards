using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.UI
{
    [ProtoContract]
    public class MenuItem : DataObject
    {
        [ProtoMember(1)]
        String Text;
        [ProtoMember(2)]
        int Weight;
        [ProtoMember(3)]
        String OnClickFunction;
        [ProtoMember(4)]
        public MenuItem[] SubItems;
    }
}
