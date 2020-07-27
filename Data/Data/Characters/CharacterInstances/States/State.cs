using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.CharacterInstances.States
{
    [ProtoContract]
    public class State : DataObject
    {
        public State(String sName, StateTypes sStateType, bool bOnOff, String sAnimation, String sSound)
        {
            Name = sName; StateType = sStateType; Animation = sAnimation; Sound = sSound;OnOff = bOnOff;
        }
        public State()
        {

        }
        [ProtoMember(1)]
        public String Name;
        [ProtoMember(2)]
        public StateTypes StateType;
        [ProtoMember(3)]
        public String Animation;
        [ProtoMember(4)]
        public String Sound;
        [ProtoMember(5)]
        public bool OnOff;
    }
}
