using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    [ProtoContract]
    public class LoginData
    {
        [ProtoMember(1)]
        public string Username;
        [ProtoMember(2)]
        public string Password;
    }
}
