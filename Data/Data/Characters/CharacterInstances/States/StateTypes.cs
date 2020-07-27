using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters.CharacterInstances.States
{
    [ProtoContract]
    public enum StateTypes
    {
            STATE_CASTING,
            STATE_IDLE,
            STATE_WALKING,
            STATE_FIGHTING,
            STATE_GOT_HIT,
            STATE_JUMPING,
            STATE_RUNNING
        }
}
