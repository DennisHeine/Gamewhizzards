using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters
{
    [ProtoContract]
    public class FightingCharacter:Characters
    {        
        [ProtoMember(1)]
        public Dictionary<int, Spells.Spell> KnownSpells = new Dictionary<int, Spells.Spell>();        
    
    }
}
