using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Actions.Fight
{
    [ProtoContract]
    public class cAttack
    {
        [ProtoMember(1)]
        public String IDPlayer = "";
        [ProtoMember(2)]
        public String TargetID="";
        [ProtoMember(3)]
        public Data.Spells.Spell Spell=new Data.Spells.Spell();
    }
}
