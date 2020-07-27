using Data;
using Data.Actions.Fight;
using Data.Items;
using Data.WorldData;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LoginServer
{
    public class Program
    {

        private static bool blocked = false;
        private static double MAX_ENEMY_DISTANCE = 20.0;

        private static ConcurrentDictionary<string,cPositionData> GlobalPlayers = new ConcurrentDictionary<string, cPositionData>();
        //private static ConcurrentDictionary<string, cPositionData> GlobalPlayers = new ConcurrentDictionary<string, cPositionData>();
        private static ConcurrentDictionary<string, cPositionDataEnemys> GlobalEnemys = new ConcurrentDictionary<string, cPositionDataEnemys>();
        public static System.Threading.Thread timeoutThread;
        public static System.Threading.Thread enemyPositionUpdateThrd;
        private static List<System.Threading.Thread> playerMovementThreads = new List<System.Threading.Thread>();        
        public static int port;
        public static List<int> ports = new List<int>();
        public static Data.WorldData.cWorldData wd = new cWorldData();
        private static List<System.Threading.Thread> respawnThreads = new List<System.Threading.Thread>();
        private static Dictionary<String,Data.Spells.Spell> Cooldowns = new Dictionary<String,Data.Spells.Spell>();

        static void Main(string[] args)
        {
            try
            {
                Data.WorldState.worldState = Data.WorldState.Load();
            }
            catch (Exception) { }
            timeoutThread = new System.Threading.Thread(handleTimeout);
            timeoutThread.Start();
            
            enemyPositionUpdateThrd = new System.Threading.Thread(EnemyPositionUpdateThrd);
            enemyPositionUpdateThrd.Start();

            
            
            NetworkComms.AppendGlobalIncomingPacketHandler<Data.WorldData.cWorldData>("SaveWorldData", HandleSaveWorldData);
            //NetworkComms.AppendGlobalIncomingPacketHandler<cPositionDataEnemys>("TargetReachedEnemy", TargetReachedEnemy);
            NetworkComms.AppendGlobalIncomingPacketHandler<bool>("RequestWorldData", HandleLoadWorldData);
            NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("cPositionData", HandleIncomingPosition);
            NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("GlobalPositionUpdate", HandleGlobalPositionUpdate);
            NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("GlobalPositionUpdateSet", HandleGlobalPositionUpdateSet);
            NetworkComms.AppendGlobalIncomingPacketHandler<LoginData>("Login", HandleLogin);
            NetworkComms.AppendGlobalIncomingPacketHandler<String[]>("ChangeWorld", HandleChangeWorld);
            NetworkComms.AppendGlobalIncomingPacketHandler<String>("RequestCycleTarget", HandleRequestCycleTarget);
            NetworkComms.AppendGlobalIncomingPacketHandler<Data.Actions.Fight.cAttack>("AttackTargetPlayer", HandleAttackPlayer);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("CancelTarget", HandleCancelTarget);
            NetworkComms.AppendGlobalIncomingPacketHandler<string>("RequestUpdateCooldowns", HandleRequestUpdateCooldowns);
            NetworkComms.AppendGlobalIncomingPacketHandler<Data.EquipItemPackage>("EquipItem", HandleRequestEquipItem);

            

            NetworkCommsDotNet.Connections.Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, 10000));
            
            cEnemys e = new cEnemys();
            String protoName = "FANTASY_WOLF";
            Data.Characters.EnemyCharacter enemyChar = new Data.Characters.EnemyCharacter();
            enemyChar.protoAssetName = protoName;
            
            GlobalEnemys= e.spawnEnemys(10, (float)-9.618841, (float)-9.091998, (float)9.618841, (float)9.091998,  enemyChar);
            startEnemyMovementThreds();

            while (true)
                System.Threading.Thread.Sleep(500);
        }

        private static void HandleRequestEquipItem(PacketHeader packetHeader, Connection connection, EquipItemPackage incomingObject)
        {
            if (GlobalPlayers[incomingObject.SessionID].character.equipment.ContainsKey(incomingObject.Item.EquipmentPosition))
                GlobalPlayers[incomingObject.SessionID].character.equipment.Remove(incomingObject.Item.EquipmentPosition);
            GlobalPlayers[incomingObject.SessionID].character.equipment.Add(incomingObject.Item.EquipmentPosition, incomingObject.Item);
          //  connection.SendObject("HandleEquipItem", incomingObject);
        }

        private void EnemyEnterFight()
        {
            
        }
        private static void HandleRequestUpdateCooldowns(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            try
            {
                Dictionary<String, Data.Spells.Spell> outp = new Dictionary<string, Data.Spells.Spell>();
                foreach (String key in Cooldowns.Keys)
                {
                    outp.Add(Cooldowns[key].Name, Cooldowns[key]);
                }
                connection.SendObject("HandleUpdateCooldown", outp);
            }
            catch (Exception ex) { }
        }

        private static void HandleCancelTarget(PacketHeader packetHeader, Connection connection, string incomingObject)
        {
            try
            {
                GlobalPlayers[incomingObject].target = null;
                connection.SendObject("SetPlayerTarget", (string)null);
            }
            catch (Exception ex) { }
        }
        
        private static void HandleAttackPlayer(PacketHeader packetHeader, Connection connection, cAttack incomingObject)
        {
            try
            {
                               
                while (!GlobalEnemys.ContainsKey(incomingObject.TargetID) || !GlobalPlayers.ContainsKey(incomingObject.IDPlayer))
                {
                    System.Threading.Thread.Sleep(100);
                }

                String SessionID = incomingObject.IDPlayer;
                if ((Cooldowns.ContainsKey(incomingObject.Spell.Name) && Cooldowns[incomingObject.Spell.Name].Cooldown_Left <= 0) || !Cooldowns.ContainsKey(incomingObject.Spell.Name))
                {

                    Data.cPositionData player = GlobalPlayers[incomingObject.IDPlayer];
                    Data.cPositionDataEnemys enemy = GlobalEnemys[incomingObject.TargetID];
                    //Data.Items.Item mainHand = player.character.equipment.Items[Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND];
                    int dmgWpnStat = 10;
                    //GlobalEnemys[incomingObject.TargetID].charData.CharacterStats.Health -= dmgWpnStat;
                    Data.Characters.Stats.Stats charStats = GlobalEnemys[incomingObject.TargetID].charData.CharacterStats;
                    int wpnDmg = 0;
                    if(GlobalPlayers[SessionID].character.equipment.ContainsKey(Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND))
                        wpnDmg = GlobalPlayers[SessionID].character.equipment[Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND].ItemStats.Damage;
                    dmgWpnStat += wpnDmg;
                    charStats.Health -= dmgWpnStat;
                    GlobalEnemys[incomingObject.TargetID].charData.CharacterStats = charStats;
                    GlobalEnemys[incomingObject.TargetID].target = incomingObject.IDPlayer;

                    System.Threading.Thread t = new System.Threading.Thread(SpellCooldownThrd);
                    SpellCooldownTrdParam param = new SpellCooldownTrdParam();
                    param.Spell = incomingObject.Spell;
                    param.SessionID = incomingObject.Spell.Name;
                    param.Spell.Cooldown_Left = param.Spell.Cooldown;
                    t.Start(param);

                    if (charStats.Health <= 0)
                    {
                        GlobalPlayers[incomingObject.IDPlayer].target = null;
                        EnemyDeadPackager pkg = new EnemyDeadPackager();
                        pkg.EnemyID = incomingObject.TargetID;
                        pkg.UserID = incomingObject.IDPlayer;

                        Data.Items.Item Sword = new Data.Items.Item();
                        Sword.MaxLevel = 10;
                        Sword.MinLevel = 0;
                        Sword.ItemStats.Damage = 5;
                        Sword.Name = "Sword";
                        Sword.MeshNameAndPath = "";
                        Sword.Type = Data.Items.Item.ItemType.ITEM_WEAPON;
                        Sword.UIImage = "";
                        Sword.ItemID = genSessionID();

                        pkg.Loot.Add(Sword.ItemID, Sword);

                        cPositionDataEnemys outval = null;
                        if(GlobalEnemys.ContainsKey(incomingObject.TargetID))
                            while (!GlobalEnemys.TryRemove(incomingObject.TargetID, out outval))
                                System.Threading.Thread.Sleep(10);

                        foreach (Connection c in NetworkComms.GetExistingConnection())
                        {
                            try
                            {
                                if (c.ConnectionAlive())
                                    c.SendObject("DestroyEnemy", pkg);
                            }
                            catch (Exception e) { }
                        }

                       
                        GlobalPlayers[incomingObject.IDPlayer].target = null;
                        connection.SendObject("SetPlayerTarget", (string)null);

                        System.Threading.Thread t1 = new System.Threading.Thread(RespawnThread);
                        t1.Start();
                        respawnThreads.Add(t1);

                    }
                }
            }
            catch (Exception ex) { }
        }

        private static void RespawnThread()
        {
           
            System.Threading.Thread.Sleep(20000);
            try
            {
                cEnemys e = new cEnemys();
                String protoName = "FANTASY_WOLF";
                Data.Characters.EnemyCharacter enemyChar = new Data.Characters.EnemyCharacter();
                enemyChar.protoAssetName = protoName;

                cPositionDataEnemys enemy = e.spawnSingleEnemy((float)-9.618841, (float)-9.091998, (float)9.618841, (float)9.091998, enemyChar, "");
                while (!GlobalEnemys.TryAdd(enemy.SessionID, enemy))
                    System.Threading.Thread.Sleep(10);
            }
            catch (Exception ex) { }
        }

        private static void HandleRequestCycleTarget(PacketHeader packetHeader, Connection connection, string SessionID)
        {
            try
            {

                System.Diagnostics.Debug.WriteLine("X");
                ArrayList enemycycle = new ArrayList();
                bool retry = false;
                do
                {
                    try
                    {

                        foreach (cPositionDataEnemys e in GlobalEnemys.Values)
                        {
                            try
                            {
                                if (isInRange(SessionID, e.SessionID))
                                {
                                    enemycycle.Add(e);
                                }
                            }
                            catch (Exception ex) { }
                        }
                    }
                    catch (Exception ex)
                    {
                        enemycycle.Clear();
                        retry = true;
                    }
                } while (retry);

                cPositionData player = (cPositionData)GlobalPlayers[SessionID]; ;
                //Enemys in range?
                if(GlobalPlayers.ContainsKey(SessionID))
                if (enemycycle.Count > 0 )
                {
                    
                    //None yet selected?
                    if (player.target == null)
                    {
                        player.target = ((cPositionDataEnemys)enemycycle[0]).SessionID;
                    }
                    else
                    {
                        //cycle through
                        for (int i = 0; i < enemycycle.Count; i++)
                        {
                            //Is enemy targeted?
                            if ((player.target) == ((cPositionDataEnemys)enemycycle[i]).SessionID)
                            {
                                //last enemy?
                                if (i == enemycycle.Count - 1)
                                {
                                    //select first enemy

                                    player.target = ((cPositionDataEnemys)enemycycle[0]).SessionID;
                                    break;
                                }
                                else
                                {
                                    //select next enemy
                                    player.target = ((cPositionDataEnemys)enemycycle[i + 1]).SessionID;
                                    break;
                                }
                            }
                        }
                    }
                }
                else//no enemys in range
                {
                    player.target = null;
                }

                
                ((Data.cPositionData)GlobalPlayers[player.SessionID]).target = player.target;
                connection.SendObject("SetPlayerTarget", player.target);
            }
            catch(Exception ex) { }
        }

        private static bool isInRange(string sessionID1, string sessionID2)
        {
            double x = ((cPositionData)GlobalPlayers[sessionID1]).x;
            double z = ((cPositionData)GlobalPlayers[sessionID1]).z;

            double x1 = GlobalEnemys[sessionID2].x;
            double z1= GlobalEnemys[sessionID2].z;
            double distance = GetDistance(x,z,x1,z1);
            double b = distance;
            if (distance <= MAX_ENEMY_DISTANCE)
                return true;
            else
                return false;
        }

        private static void startEnemyMovementThreds()
        {
            foreach(cPositionDataEnemys enemy in GlobalEnemys.Values)            
            {
                String ID = enemy.SessionID;
                System.Threading.ParameterizedThreadStart pts = new System.Threading.ParameterizedThreadStart(EnemyMovementThrd);
                System.Threading.Thread t = new System.Threading.Thread(pts);
                t.Start(ID);
                playerMovementThreads.Add(t);
            }

        }
        private static void genRndPoint(ref float x1, ref float y1, ref float x2, ref float y2)
        {
            double distance = 0;
            float x = 0;
            float y = 0;
            do
            {



                double range = (double)x2 - (double)x1;
                double sample = (new Random()).NextDouble();
                double scaled = (sample * range) + x1;
                x = (float)scaled;

                range = (double)y2 - (double)y1;
                sample = (new Random()).NextDouble();
                scaled = (sample * range) + y1;
                y = (float)scaled;


                distance = GetDistance(x1, y1, x, y);
            } while (distance < 2);

            x1 = x;
            y1 = y;

        }

        private static double GetDistance(double x1, double y1, double x2, double  y2)
        {
            //pythagorean theorem c^2 = a^2 + b^2
            //thus c = square root(a^2 + b^2)
            double a = x1-x2;
            double b = y1-y2;
            double ret= Math.Sqrt(a * a + b * b);
            return ret;
        }
        private static ConcurrentDictionary<String, System.Threading.Thread> EnemyFightingThreads = new ConcurrentDictionary<string, System.Threading.Thread>();
        private static void EnemyMovementThrd(object obj)
        {
            String ID = (String)obj;
            int sleep = 0;
            bool cont = true;
            while(cont)
            {
                if (GlobalEnemys.ContainsKey(ID))
                {
                    
                    if (GlobalEnemys[ID].target != null)
                    {
                        if (GlobalPlayers.ContainsKey(GlobalEnemys[ID].target))
                        {
                            try
                            {

                                float avaX = (float)Math.Round(GlobalEnemys[ID].x, 1, MidpointRounding.AwayFromZero);//   avatar.transform.position.x-(avatar.transform.position.x % 1f);
                                float avaY = (float)Math.Round(GlobalEnemys[ID].z, 1, MidpointRounding.AwayFromZero);//   avatar.transform.position.x-(avatar.transform.position.x % 1f);

                                Data.cPositionData player = GlobalPlayers[GlobalEnemys[ID].target];

                                GlobalEnemys[ID].x = GlobalPlayers[GlobalEnemys[ID].target].x + 3.0f;
                                GlobalEnemys[ID].z = GlobalPlayers[GlobalEnemys[ID].target].z + 3.0f;
                                if (GetDistance(avaX, avaY, player.x, player.z) < 5)
                                {
                                    if (!GlobalEnemys[ID].isInFight)
                                    {
                                        GlobalEnemys[ID].isInFight = true;
                                        System.Threading.Thread EnemyFightingThread = new System.Threading.Thread(EnemyFightingThr);
                                        EnemyFightingThread.Start(ID);
                                        while (!EnemyFightingThreads.TryAdd(ID, EnemyFightingThread))
                                            System.Threading.Thread.Sleep(10);
                                    }
                                }
                                else
                                {
                                    if (EnemyFightingThreads.ContainsKey(ID))
                                    {
                                        EnemyFightingThreads[ID].Abort();
                                        System.Threading.Thread value=null;
                                        while (!EnemyFightingThreads.TryRemove(ID, out value))
                                            System.Threading.Thread.Sleep(10);
                                    }
                                    GlobalEnemys[ID].isInFight = false;
                                }

                                sleep = 500;
                            }
                            catch (Exception ex) { }
                        }
                        else
                        {
                            GlobalEnemys[ID].target = null;
                            GlobalEnemys[ID].isInFight = false;
                        }
                    }
                    else
                    { 
                        float x1 = GlobalEnemys[ID].upperLefX;
                        float y1 = GlobalEnemys[ID].upperLefY;
                        genRndPoint(ref x1, ref y1, ref GlobalEnemys[ID].lowerRightX, ref GlobalEnemys[ID].lowerRightY);
                        GlobalEnemys[ID].x = x1;
                        GlobalEnemys[ID].z = y1;
                        sleep= (new Random()).Next(3000, 7000);
                    }
                }
                else
                {
                    cont = false;
                }

                int wait = (sleep);
                System.Threading.Thread.Sleep(wait);
            }
        }

        private static void EnemyFightingThr(object ID)
        {
            bool cont = true;
            while (cont)
            {
                try
                {
                    //target=null?????
                    if (GlobalPlayers.ContainsKey(GlobalEnemys[(String)ID].target) && GlobalEnemys[(String)ID].isInFight)
                    {
                        GlobalPlayers[GlobalEnemys[(String)ID].target].character.CharacterStats.Health -= 5;
                        if (GlobalPlayers[GlobalEnemys[(String)ID].target].character.CharacterStats.Health <= 0)
                        {
                            cPositionData outval = null;
                            while(!GlobalPlayers.TryRemove(GlobalEnemys[(String)ID].target,out outval))
                                System.Threading.Thread.Sleep(10);
                            String target = GlobalEnemys[(String)ID].target;
                            
                            foreach (Connection c in NetworkComms.GetExistingConnection())
                            {
                                try
                                {
                                    if (c.ConnectionAlive())
                                        c.SendObject("HandlePlayerDead", target);
                                }
                                catch (Exception ex) { }
                            }
                            GlobalEnemys[(String)ID].target = null;
                        }
                        else
                        {
                            foreach (Connection c in NetworkComms.GetExistingConnection())
                            {
                                try
                                {
                                    if (c.ConnectionAlive())
                                        c.SendObject("HandleEnemyStartFight", ID);
                                }
                                catch (Exception ex) { }
                            }
                        }
                    }
                    else
                    {
                        cont = false;
                    }
                }
                catch (Exception ex) { }
                System.Threading.Thread.Sleep(3000);
            }
            System.Threading.Thread outval1 = null;
            while(!EnemyFightingThreads.TryRemove((String)ID, out outval1))
                System.Threading.Thread.Sleep(10);
        }

        private static void EnemyPositionUpdateThrd()
        {
            while (true)
            {
                try
                {
                    GlobalEnemyData e = new GlobalEnemyData();
                    foreach(cPositionDataEnemys enemy in GlobalEnemys.Values)
                        e.enemys.Add(enemy.SessionID,enemy);
                    foreach (Connection c in NetworkComms.GetExistingConnection())
                    {
                        if (c.ConnectionAlive())
                            c.SendObject("GlobalEnemyPositions", e);
                    }
                }
                catch (Exception ex) { }
                System.Threading.Thread.Sleep(500);
            }
        }

    

        private static void HandleSaveWorldData(PacketHeader packetHeader, Connection connection, cWorldData incomingObject)
        {
            wd = incomingObject;
        }

        private static void HandleLoadWorldData(PacketHeader packetHeader, Connection connection, bool incomingObject)
        {
            try
            {
                connection.SendObject("DoLoadWorldData", wd);
            }
            catch (Exception ex) { }
        }

        public static Hashtable warteschlefie = new Hashtable();



        private static void HandleChangeWorld(PacketHeader packetHeader, Connection connection, String[] args)
        {
            ConcurrentDictionary<String, Object> packetData = new ConcurrentDictionary<String, Object>();
            while (!packetData.TryAdd("SessionID", args[1]))
                System.Threading.Thread.Sleep(10);
            while(!packetData.TryAdd("Connection", connection))
                System.Threading.Thread.Sleep(10);
            while(!packetData.TryAdd("IDWorld", args[0]))
                System.Threading.Thread.Sleep(10);

            if (!warteschlefie.Contains(args[0]))
            {
                ArrayList data = new ArrayList();
                data.Add(packetData);
                warteschlefie.Add(args[0], data);
            }
            else
            {
                ((ArrayList)warteschlefie[args[0]]).Add(packetData);
            }
            ArrayList users = (ArrayList)warteschlefie[args[0]];

            if(users.Count==5)
            {
                
                int port = 0;
                while (port == 0 || ports.Contains(port))
                {
                    Random rnd = new Random();
                    port = rnd.Next(10001, 20000);
                }
                ports.Add(port);
                Process.Start(".\\core_server.exe", args[0]+" "+port.ToString()+" "+ users[0]+" "+users[1] + " " + users[2] + " " + users[3] + " " + users[4]);
                System.Threading.Thread.Sleep(5000);
                for (int i = 0; i < users.Count; i++)
                {
                    ConcurrentDictionary<String, Object> userData = (ConcurrentDictionary<String, Object>)users[i];
                    try {
                        ((Connection)userData["Connection"]).SendObject("ChangeWorld", port);
                    }catch (Exception) { }
                }
            }
        }

        public static void handleTimeout()
        {
            while (true)
            {
                try
                {
                    foreach (cPositionData player in GlobalPlayers.Values)
                    {
                        long now = ConvertToTimestamp(DateTime.Now);
                        if (player.lastUpdate < now - 30 && player.lastUpdate != 0)
                        {
                            foreach(cPositionDataEnemys e in GlobalEnemys.Values)
                            {
                                if(e.target==player.SessionID)
                                {
                                    GlobalEnemys[e.SessionID].target = null;
                                    GlobalEnemys[e.SessionID].isInFight = false;
                                }
                            }
                            cPositionData outval = null;
                            while(!GlobalPlayers.TryRemove(player.SessionID,out outval))
                                System.Threading.Thread.Sleep(10);
                            foreach (Connection c in NetworkComms.GetExistingConnection())
                                c.SendObject("DeletePlayer", player.SessionID);
                            break;
                        }
                    }
                }
                catch (Exception) { }
                System.Threading.Thread.Sleep(5000);
            }

        }

        public class SpellCooldownTrdParam
        {
            public Data.Spells.Spell Spell;
            public String SessionID;
        }

        public static void SpellCooldownThrd(object spelldat)
        {
                Data.Spells.Spell dSpell = ((SpellCooldownTrdParam)spelldat).Spell;
            if (!Cooldowns.ContainsKey(((SpellCooldownTrdParam)spelldat).SessionID))
            {

                Cooldowns.Add(((SpellCooldownTrdParam)spelldat).SessionID, dSpell);
            }
           

                dSpell.Cooldown_Left = dSpell.Cooldown * 10;
                while (dSpell.Cooldown_Left > 0)
                {
                    dSpell.Cooldown_Left--;
                    Cooldowns[((SpellCooldownTrdParam)spelldat).SessionID].Cooldown_Left = dSpell.Cooldown_Left;
                    System.Threading.Thread.Sleep(100);
                }
            
        }
        
        public static void HandleLogin(PacketHeader packetHeader, Connection connection, LoginData incomingObject)
        {
            if (incomingObject.Username == "user" && incomingObject.Password == "pass")
            {
                String sessoinID = genSessionID();
                cPositionData dat = new cPositionData();
                dat.x = 0; dat.y = 0; dat.z = 0; dat.SessionID = sessoinID;
                /*
                dat.character.CharacterStats = Data.Characters.Stats.Stat.generateCharacterStats();
                Data.Characters.Stats.Stat.StatType[] type = { Data.Characters.Stats.Stat.StatType.STAT_STRENGTH };
                dat.character.equipment.Items.Add(Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND, new Data.Items.Item());
                dat.character.equipment.Items[Data.Characters.Equipment.cEquipment.StatType.EQU_LEFT_HAND].ItemStats = Data.Characters.Stats.Stat.generateItemStats(type);
                */
                dat.character = new Data.Characters.PlayerCharacter();
                dat.character.CharacterStats.Health = 100;
                dat.character.Spells.Add("Lightning", new Data.Spells.Spell("Lightning", "Casts a lightning bolt, dealing 10dmg", Data.Spells.Spell.SpellType.SPELL_ATTACK, new Data.Characters.Stats.Stats(0, 0, 0, 0, 0, 0, 10, "num1"),1, "Bolt", "LightningStart", "LightningEnd", 3,"1"));
                dat.character.Spells["Lightning"].Cooldown_Left = 0;
                
                Data.Characters.Inventory.Inventory inv = new Data.Characters.Inventory.Inventory();
                Data.Items.Item Sword = new Data.Items.Item();
                Sword.MaxLevel = 10;
                Sword.MinLevel = 0;
                Sword.ItemStats.Damage = 5;
                Sword.Name = "Sword";
                Sword.MeshNameAndPath = "";
                Sword.Type = Data.Items.Item.ItemType.ITEM_WEAPON;
                Sword.UIImage = "";
                Sword.ItemID = genSessionID();

                Data.Characters.Inventory.Inventory.ItemData invItem = new Data.Characters.Inventory.Inventory.ItemData();
                invItem.Item = Sword;
                invItem.Count = 1;
                invItem.ItemPositions = 1;


                inv.Items.Add(Sword.ItemID, invItem);

                Sword = new Data.Items.Item();
                Sword.MaxLevel = 10;
                Sword.MinLevel = 0;
                Sword.ItemStats.Damage = 5;
                Sword.Name = "Sword";
                Sword.MeshNameAndPath = "";
                Sword.Type = Data.Items.Item.ItemType.ITEM_WEAPON;
                Sword.UIImage = "";               
                Sword.ItemID = genSessionID();

                invItem = new Data.Characters.Inventory.Inventory.ItemData();
                invItem.Item = Sword;
                invItem.Count = 1;
                invItem.ItemPositions = 1;


                inv.Items.Add(Sword.ItemID, invItem);

                dat.character.Inventory = inv;
                

                while (!GlobalPlayers.TryAdd(sessoinID, dat))
                    System.Threading.Thread.Sleep(10);
                connection.SendObject("Authorize", dat);
            }
            else
            {
                connection.SendObject("Authorize", "");
            }
        }

        private static String genSessionID()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[25];
            System.Threading.Thread.Sleep(500);
            var random = new System.Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringChars);
            return finalString;
        }

        private static void HandleGlobalPositionUpdateSet(PacketHeader packetHeader, Connection connection, cPositionData incomingObject)
        {
            try
            {
                String id = incomingObject.SessionID;//in unique id ändern!
                cPositionData pos = incomingObject;
                if (!GlobalPlayers.ContainsKey(id))
                {
                    while (!GlobalPlayers.TryAdd(id, pos))
                        System.Threading.Thread.Sleep(10);
                    //!!!GlobalPlayers[id].character.CharacterStats = Data.Characters.Stats.Stat.generateCharacterStats();
                }
                else
                {
                    copyIncToGlobalPlayerPos(incomingObject);
                }
            }
            catch (Exception ex) { }
        }

        private static void copyIncToGlobalPlayerPos(cPositionData incomingObject)
        {
            GlobalPlayers[incomingObject.SessionID].x = incomingObject.x;
            GlobalPlayers[incomingObject.SessionID].y = incomingObject.y;
            GlobalPlayers[incomingObject.SessionID].z = incomingObject.z;
            GlobalPlayers[incomingObject.SessionID].rx = incomingObject.rx;
            GlobalPlayers[incomingObject.SessionID].ry = incomingObject.ry;
            GlobalPlayers[incomingObject.SessionID].rz = incomingObject.rz;
        }

        private static void HandleGlobalPositionUpdate(PacketHeader packetHeader, Connection connection, cPositionData incomingObject)
        {
            String id = incomingObject.SessionID;//in unique id ändern!
            cPositionData pos = incomingObject;
            if (!GlobalPlayers.ContainsKey(id))
            {
                while (!GlobalPlayers.TryAdd(id, pos))
                    System.Threading.Thread.Sleep(10);
                //!!!GlobalPlayers[id].character.CharacterStats = Data.Characters.Stats.Stat.generateCharacterStats();
            }
            else
            {
                bool moving = ((cPositionData)GlobalPlayers[incomingObject.SessionID]).moving;
                long l = ((cPositionData)GlobalPlayers[incomingObject.SessionID]).lastUpdate;
                copyIncToGlobalPlayerPos(pos);
                ((cPositionData)GlobalPlayers[incomingObject.SessionID]).moving = moving;
                ((cPositionData)GlobalPlayers[incomingObject.SessionID]).lastUpdate = ConvertToTimestamp(DateTime.Now);

            }
            GlobalPlayerData d = new GlobalPlayerData();
            foreach (cPositionData pos1 in GlobalPlayers.Values)
                d.players.Add(pos1.SessionID, pos1);
            
         //   GlobalEnemyData e = new GlobalEnemyData();
          //  e.enemys = GlobalEnemys;
            try
            {
                connection.SendObject("GlobalPlayerPositions", d);
                
            }
            catch (Exception) { }
        }

        private static void updateMoving()
        {
            while (true)
            {
                try
                {
                    foreach (cPositionData incomingObject in GlobalPlayers.Values)
                    {
                        if (ConvertToTimestamp(DateTime.Now) - ((cPositionData)GlobalPlayers[incomingObject.SessionID]).lastUpdate < 100)
                        {
                            ((cPositionData)GlobalPlayers[incomingObject.SessionID]).moving = true;
                        }
                        else
                        {
                            ((cPositionData)GlobalPlayers[incomingObject.SessionID]).moving = false;
                        }
                    }
                }
                catch (Exception) { }
                System.Threading.Thread.Sleep(10);
            }
        }
        private static long ConvertToTimestamp(DateTime value)
        {
            long epoch = (value.Ticks - 621355968000000000) / 10000000;
            return epoch;
        }
        private static void HandleIncomingPosition(PacketHeader packetHeader, Connection connection, cPositionData incomingObject)
        {
            if (GlobalPlayers.ContainsKey(incomingObject.SessionID))
            {
                try
                {
                    if (incomingObject.x > 0.00001 || incomingObject.x < -0.00001 || incomingObject.y > 0.00001 || incomingObject.y < -0.00001)
                    {

                        ((cPositionData)GlobalPlayers[incomingObject.SessionID]).lastUpdate = ConvertToTimestamp(DateTime.Now);
                        ((cPositionData)GlobalPlayers[incomingObject.SessionID]).moving = true;

                        //GlobalPlayers[incomingObject.SessionID].moving = true;
                    }
                    else
                    {
                        ((cPositionData)GlobalPlayers[incomingObject.SessionID]).moving = false;
                    }
                }
                catch (Exception) { }

                try
                {
                    connection.SendObject("cPositionData1", incomingObject);
                }
                catch (Exception) { }
            }
        }

    }
}
