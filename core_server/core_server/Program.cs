using Data;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace core_server
{
    public class Program
    {
        private static Dictionary<string, cPositionData> GlobalPlayers = new Dictionary<string, cPositionData>();
        public static System.Threading.Thread timeoutThread;
        public static int port;
        public static string WorldID;
        static void Main(string[] args)
        {
            int count = 0;
            foreach (String arg in args)
            {
                if (count >= 2)
                {
                    String sessoinID = arg;
                    cPositionData dat = new cPositionData();
                    dat.x = 0; dat.y = 0; dat.z = 0; dat.SessionID = sessoinID;
                    if(!GlobalPlayers.ContainsKey(sessoinID))
                        GlobalPlayers.Add(sessoinID, dat);
                }
                else if(count==1)
                {
                    port = int.Parse(arg);
                }
                else if(count==0)
                {
                    WorldID = arg;
                }
                count++;
            }
            try
            {
                Data.WorldState.worldState = Data.WorldState.Load();
            }
            catch (Exception) { }
            timeoutThread = new System.Threading.Thread(handleTimeout);
            timeoutThread.Start();


            NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("cPositionData", HandleIncomingPosition);
            NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("GlobalPositionUpdate", HandleGlobalPositionUpdate);
            NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("GlobalPositionUpdateSet", HandleGlobalPositionUpdateSet);
            NetworkComms.AppendGlobalIncomingPacketHandler<LoginData>("Login", HandleLogin);
            NetworkCommsDotNet.Connections.Connection.StartListening(ConnectionType.TCP, new IPEndPoint(IPAddress.Any, port));
            while (true)
                System.Threading.Thread.Sleep(5000);
        }

        public static void handleTimeout()
        {
            while(true)
            {
                try
                {
                    foreach (cPositionData player in GlobalPlayers.Values)
                    {
                        long now = ConvertToTimestamp(DateTime.Now);
                        if (player.lastUpdate < now-30&&player.lastUpdate!=0)
                        {
                            GlobalPlayers.Remove(player.SessionID);
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

        public static void HandleLogin(PacketHeader packetHeader, Connection connection, LoginData incomingObject)
        {
            if (incomingObject.Username == "user" && incomingObject.Password == "pass")
            {
                String sessoinID = genSessionID();
                cPositionData dat = new cPositionData();
                dat.x = 0; dat.y = 0; dat.z = 0; dat.SessionID = sessoinID;
                GlobalPlayers.Add(sessoinID, dat);
                connection.SendObject("Authorize", sessoinID);
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
            String id = incomingObject.SessionID;//in unique id ändern!
            cPositionData pos = incomingObject;
            if (!GlobalPlayers.ContainsKey(id))
            {
                GlobalPlayers.Add(id, pos);
            }
            else
            {
                GlobalPlayers.Remove(id);
                GlobalPlayers.Add(id, pos);
            }
        }

        private static void HandleGlobalPositionUpdate(PacketHeader packetHeader, Connection connection, cPositionData incomingObject)
        {
            String id = incomingObject.SessionID;//in unique id ändern!
            cPositionData pos = incomingObject;
            if (!GlobalPlayers.ContainsKey(id))
            {
                GlobalPlayers.Add(id, pos);
            }
            else
            {
                bool moving = GlobalPlayers[incomingObject.SessionID].moving;
                long l = GlobalPlayers[incomingObject.SessionID].lastUpdate;
                GlobalPlayers[incomingObject.SessionID] = pos;
                GlobalPlayers[incomingObject.SessionID].moving = moving;
                GlobalPlayers[incomingObject.SessionID].lastUpdate = ConvertToTimestamp(DateTime.Now);

            }
            GlobalPlayerData d = new GlobalPlayerData();
            d.players = GlobalPlayers;
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
                        if (ConvertToTimestamp(DateTime.Now) - GlobalPlayers[incomingObject.SessionID].lastUpdate < 100)
                        {
                            GlobalPlayers[incomingObject.SessionID].moving = true;
                        }
                        else
                        {
                            GlobalPlayers[incomingObject.SessionID].moving = false;
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

                        GlobalPlayers[incomingObject.SessionID].lastUpdate = ConvertToTimestamp(DateTime.Now);
                        GlobalPlayers[incomingObject.SessionID].moving = true;

                        //GlobalPlayers[incomingObject.SessionID].moving = true;
                    }
                    else
                    {
                        GlobalPlayers[incomingObject.SessionID].moving = false;
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
