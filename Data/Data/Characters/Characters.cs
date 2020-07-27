using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Characters
{
    [ProtoContract]
    public class Characters : DataObject
    {
        [ProtoMember(1)]
        public String Bezeichnung;
        [ProtoMember(2)]
        public String Category;
        [ProtoMember(3)]        
        public String MeshNameAndPath;
        [ProtoMember(4)]
        public String[] Textures;       
    }    
}
