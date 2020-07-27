using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mods
{
    [ProtoContract]
    public class EnemyColection : DataObject
    {
        [ProtoMember(1)]
        public EnemyData[] Enemys;
        [ProtoContract]
        public class EnemyData
        {
            [ProtoMember(1)]
            public Characters.EnemyCharacter Enemy;
            [ProtoMember(2)]
            public cPositionData[] SpawnPoints;
            [ProtoMember(3)]
            public long avgSpawnRange;
            [ProtoMember(4)]
            public long avgRespawnTime;
            [ProtoMember(5)]
            public int SpawnMinLevel;
            [ProtoMember(6)]
            public int SpawnMaxLeve;
            [ProtoMember(7)]
            public int SpawnMinNumber;
            [ProtoMember(8)]
            public int SpawnMaxNumber;
                       
        }
    }
}
