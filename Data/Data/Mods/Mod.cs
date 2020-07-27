using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Mods
{
    [ProtoContract]
    public class Mod : DataObject
    {
        [ProtoMember(1)]
        public BossEnemyCollection BossEnemys;
        [ProtoMember(2)]
        public NPCCollection NPCs;
        [ProtoMember(3)]
        public EnemyColection Enemys;
        [ProtoMember(4)]
        public Characters.PlayerCharacter Player;
        [ProtoMember(5)]
        public Dictionary<String, Quests.Quest> Quests = new Dictionary<string, Quests.Quest>();
        [ProtoMember(6)]
        public Dictionary<String, Data.Characters.Dialogs.Dialog> Dialogs = new Dictionary<string, Data.Characters.Dialogs.Dialog>();
        [ProtoMember(7)]
        public Dictionary<String, Data.Items.Item> Items = new Dictionary<string, Data.Items.Item>();
        // SoundName:SoundFile
        [ProtoMember(8)]
        public Dictionary<String, String> Sounds = new Dictionary<string, string>();
        // AnimationName:AnimationFile
        [ProtoMember(9)]
        public Dictionary<String, String> Animations = new Dictionary<string, string>();
        [ProtoMember(10)]
        public String LevelMesh;
        [ProtoMember(11)]
        //Name : Script
        public Dictionary<String, String> Scripts = new Dictionary<string, string>();

    }
}
