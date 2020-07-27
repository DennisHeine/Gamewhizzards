using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.CharacterInstances.States
{
    [ProtoContract]
    public class StatesCollection : DataObject
    {
        [ProtoMember(1)]
        public Dictionary<string, Data.Characters.CharacterInstances.States.State> AvailableStates = new Dictionary<string, Data.Characters.CharacterInstances.States.State>();
        public StatesCollection()
        {            
        }       
    }
}
