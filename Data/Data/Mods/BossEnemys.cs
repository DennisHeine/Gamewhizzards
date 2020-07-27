using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mods
{
    [ProtoContract]
    public class BossEnemyCollection : DataObject
    {
        [ProtoMember(1)]
        BossEnemyData[] Bosses;
        [ProtoContract]
        class BossEnemyData
        {
            [ProtoMember(1)]
            public Characters.EnemyCharacter BossEnemy=new Characters.EnemyCharacter();
            [ProtoMember(2)]
            public cPositionData SpawnPoint=new cPositionData();
        }
    }
}
