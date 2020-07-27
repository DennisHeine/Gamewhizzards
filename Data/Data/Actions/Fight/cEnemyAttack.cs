using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Actions.Fight
{
    class cEnemyAttack
    {
        [ProtoContract]
        public class cAttack
        {
            [ProtoMember(1)]
            public String IDEnemy = "";
            [ProtoMember(2)]
            public int DamageDone = 0;
        }
    }
}
