using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    [ProtoContract]
    public class EnemyDeadPackager
    {
        [ProtoMember(1)]
        public String UserID="";
        [ProtoMember(2)]
        public String EnemyID="";
        [ProtoMember(3)]
        public Dictionary<String, Data.Items.Item> Loot = new Dictionary<string, Items.Item>();
    }
}
