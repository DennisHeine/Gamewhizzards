using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace Data
{
    [ProtoContract]
    public static class WorldState
    {
        [ProtoContract]
        public class CompleteWorldState:Mods.Mod
        {
            [ProtoMember(1)]
            public Dictionary<String, cPositionData> worldPlayerPositions = new Dictionary<String, cPositionData>();
            [ProtoMember(2)]            
            public Dictionary<String, Data.Items.Item> worldItems = new Dictionary<String, Data.Items.Item>();
            [ProtoMember(3)]
            public Characters.PlayerCharacter playerCharacter = new Characters.PlayerCharacter();
        }
        [ProtoMember(1)]
        public static CompleteWorldState worldState = new CompleteWorldState();
        public static CompleteWorldState Load(String filePath = ".\\WorldState.dat")
        {            
            string file = filePath;
            CompleteWorldState listofa = new CompleteWorldState();
            XmlSerializer formatter = new XmlSerializer(listofa.GetType());
            FileStream aFile = new FileStream(file, FileMode.Open);
            byte[] buffer = new byte[aFile.Length];
            aFile.Read(buffer, 0, (int)aFile.Length);
            MemoryStream stream = new MemoryStream(buffer);
            return (CompleteWorldState)formatter.Deserialize(stream);
        }


        public static void Save(CompleteWorldState listofa)
        {
            string path = ".\\WorldState.dat";
            FileStream outFile = File.Create(path);
            XmlSerializer formatter = new XmlSerializer(listofa.GetType());
            formatter.Serialize(outFile, listofa);
        }
    }
}
