using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;


namespace Data
{
    [ProtoContract]
    public class GlobalEnemyData
    {
        [ProtoMember(1)]
        public Dictionary<string, cPositionDataEnemys> enemys = new Dictionary<string, cPositionDataEnemys>();

    }
}
