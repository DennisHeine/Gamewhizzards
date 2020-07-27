using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.UI
{
    [ProtoContract]
    public class Notification : DataObject
    {
        [ProtoMember(1)]
        public String Text;
        [ProtoMember(2)]
        public bool YesButton;
        [ProtoMember(3)]
        public bool NoButton;
        [ProtoMember(4)]
        public bool CancelButton;
        [ProtoMember(5)]
        public int Timeout;
    }
}
