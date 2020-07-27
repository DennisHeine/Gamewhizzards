using Data;
using Data.Spells;
using NetworkCommsDotNet;
using NetworkCommsDotNet.Connections;
using NetworkCommsDotNet.Connections.TCP;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkHandler  {

	public NetworkHandler()
    {
        NetworkComms.AppendGlobalIncomingPacketHandler<GlobalEnemyData>("GlobalEnemyPositions", HandleIncomingGlobalEnemyPositions);
        NetworkComms.AppendGlobalIncomingPacketHandler<GlobalPlayerData>("GlobalPlayerPositions", HandleIncomingGlobalPlayerPositions);
        NetworkComms.AppendGlobalIncomingPacketHandler<Data.WorldData.cWorldData>("DoLoadWorldData", HandleLoadWorldData);
        NetworkComms.AppendGlobalIncomingPacketHandler<cPositionData>("Authorize", HandleLoginAnswer);
        NetworkComms.AppendGlobalIncomingPacketHandler<String>("DeletePlayer", HandleDeletePlayer);
        NetworkComms.AppendGlobalIncomingPacketHandler<int>("ChangeWorld", HandleChangeWorld);
        NetworkComms.AppendGlobalIncomingPacketHandler<Data.EnemyDeadPackager>("DestroyEnemy", HandleDestroyEnemy);
        NetworkComms.AppendGlobalIncomingPacketHandler<String>("HandleEnemyStartFight", HandleEnemyHit);
        NetworkComms.AppendGlobalIncomingPacketHandler<String>("HandlePlayerDead", HandlePlayerDead);
        NetworkComms.AppendGlobalIncomingPacketHandler<Dictionary<String, Data.Spells.Spell>>("HandleUpdateCooldown", HandleUpdateCooldown);
        NetworkComms.AppendGlobalIncomingPacketHandler<Data.EquipItemPackage> ("HandleEquipItem", HandleEquipItem);
    }


    private void HandleEquipItem(PacketHeader packetHeader, Connection connection, Data.EquipItemPackage incomingObject)
    {
        if (incomingObject.Item.Type == Data.Items.Item.ItemType.ITEM_WEAPON || incomingObject.Item.Type == Data.Items.Item.ItemType.ITEM_ARMOR)
        {
            if(Globals.GlobalPlayerPositions[Globals.SessionID].character.equipment.ContainsKey(incomingObject.Item.EquipmentPosition))
                Globals.GlobalPlayerPositions[Globals.SessionID].character.equipment.Remove(incomingObject.Item.EquipmentPosition);
            Globals.GlobalPlayerPositions[Globals.SessionID].character.equipment.Add(incomingObject.Item.EquipmentPosition, incomingObject.Item);
        }
    }

    


    private void HandleUpdateCooldown(PacketHeader packetHeader, Connection connection, Dictionary<string, Spell> incomingObject)
    {
        if (Globals.Cooldowns.Count > 0)
        {
            int a = 1;
        }
        Globals.Cooldowns = incomingObject;
    }

    private void HandlePlayerDead(PacketHeader packetHeader, Connection connection, string incomingObject)
    {
        if (incomingObject == Globals.SessionID)
            Globals.dead = true;
    }

    private void HandleEnemyHit(PacketHeader packetHeader, Connection connection, string incomingObject)
    {
        Globals.GlobalAttackingEnemys.Add(incomingObject);
        //GlobalPlayerPositions[GlobalEnemyPositions[incomingObject].target].character.
    }

    private void HandleDestroyEnemy(PacketHeader packetHeader, Connection connection, Data.EnemyDeadPackager incomingObject)
    {
        Debug.Log("DDDDDDD");
        Globals.EnemysToRemove.Add(incomingObject.EnemyID);
        Debug.Log(incomingObject.EnemyID);
        try
        {
            Vector3 position = new Vector3();
            if (Globals.GlobalEnemyPositions.ContainsKey(incomingObject.EnemyID))
            {
                position = new Vector3(Globals.GlobalEnemyPositions[incomingObject.EnemyID].x, Globals.GlobalEnemyPositions[incomingObject.EnemyID].y, Globals.GlobalEnemyPositions[incomingObject.EnemyID].z);
                Globals.GlobalEnemyPositions.Remove(incomingObject.EnemyID);
                
            }
            if (Globals.GlobalEnemyPositionsOld.ContainsKey(incomingObject.EnemyID))
            {
                position = new Vector3(Globals.GlobalEnemyPositionsOld[incomingObject.EnemyID].x, Globals.GlobalEnemyPositionsOld[incomingObject.EnemyID].y, Globals.GlobalEnemyPositionsOld[incomingObject.EnemyID].z);
                Globals.GlobalEnemyPositionsOld.Remove(incomingObject.EnemyID);
            }
                
            foreach(Data.Items.Item itm in incomingObject.Loot.Values)
            {
                itm.x = position.x;
                itm.y = position.y;
                itm.z = position.z;
                Globals.Loot.Add(itm.ItemID,itm);              
            }
           
        }
        catch (Exception ex) { }


    }

    private void HandleAttackAnswer(PacketHeader packetHeader, Connection connection, cPositionDataEnemys incomingObject)
    {
        Globals.GlobalEnemyPositions[incomingObject.SessionID] = incomingObject;
    }

    private void HandleLoadWorldData(PacketHeader packetHeader, Connection connection, Data.WorldData.cWorldData incomingObject)
    {
        Globals.wd = incomingObject;
        Globals.loadWorldData = true;


    }
    private void HandleChangeWorld(PacketHeader packetHeader, Connection connection, int incomingObject)
    {
        int port = incomingObject;
        Globals.con.CloseConnection(false);
        String ServerIP = "192.168.2.11";
        ConnectionInfo serverConnectionInfo = new ConnectionInfo(ServerIP, port);
        Globals.con = TCPConnection.GetConnection(serverConnectionInfo, true);
        SceneManager.LoadScene("scne");
    }



    private void HandleDeletePlayer(PacketHeader packetHeader, Connection connection, string incomingObject)
    {
        //!!! while (locked)
        //  !!!  System.Threading.Thread.Sleep(5);
        Globals.locked = true;
        Globals.GlobalPlayerPositions.Remove(incomingObject);
        Globals.PlayersToRemove.Add(incomingObject);
        Globals.locked = false;
    }

    private void HandleLoginAnswer(PacketHeader packetHeader, Connection connection, cPositionData incomingObject)
    {
        if (incomingObject != null)
        {
            Globals.SessionID = incomingObject.SessionID;
            Globals.PlayerPosition = incomingObject;
            UpdatePlayerTarget.SessionID = Globals.SessionID;
            UpdatePlayerTarget.con = Globals.con;

        }
        else
            Application.Quit();
    }


    private void HandleIncomingGlobalPlayerPositions(PacketHeader packetHeader, Connection connection, GlobalPlayerData incomingObject)
    {

        Globals.locked = true;

        foreach (cPositionData pos in incomingObject.players.Values)
        {
            try
            {
                if (pos.SessionID != "")
                {
                    if (!Globals.GlobalPlayerPositions.ContainsKey(pos.SessionID) && pos.SessionID != Globals.SessionID)
                    {
                        Globals.GlobalPlayerPositions.Add(pos.SessionID, pos);
                        Globals.playersToSpawn.Add(pos.SessionID, pos);
                    }
                    else
                    {
                        Globals.locked = true;

                        if (pos.SessionID == Globals.SessionID)
                        {
                            Globals.PlayerPosition = pos;
                            Globals.InventoryItemsNew.Clear();

                            foreach (Data.Characters.Inventory.Inventory.ItemData item in pos.character.Inventory.Items.Values)
                            {
                                Globals.InventoryItemsNew.Add(item);
                            }
                        }

                        Globals.GlobalPlayerPositions[pos.SessionID] = pos; 
                        if (!Globals.GlobalPlayerPositionsOld.ContainsKey(pos.SessionID))
                            Globals.GlobalPlayerPositionsOld.Add(pos.SessionID, pos);
                        else
                            Globals.GlobalPlayerPositionsOld[pos.SessionID] = pos;

                        Globals.locked = false;
                    }
                    //GlobalPlayerPositionsOld[pos.SessionID] = GlobalPlayerPositions[pos.SessionID];
                }
            }
            catch (Exception ex) { }
        }
        Globals.locked = false;

    }

    private void HandleIncomingGlobalEnemyPositions(PacketHeader packetHeader, Connection connection, GlobalEnemyData incomingObject)
    {

        //while (locked)
        //  System.Threading.Thread.Sleep(5);
        Globals.locked = true;
        foreach (cPositionDataEnemys pos in incomingObject.enemys.Values)
        {

            if (pos.SessionID != "")
            {
                if (!Globals.GlobalEnemyPositions.ContainsKey(pos.SessionID) && pos.SessionID != Globals.SessionID)
                {
                    Globals.GlobalEnemyPositions.Add(pos.SessionID, pos);
                    Globals.enemysToSpawn.Add(pos.SessionID, pos);
                }
                else
                {
                    Globals.locked = true;
                    float yOld = Globals.GlobalEnemyPositions[pos.SessionID].y;
                    Globals.GlobalEnemyPositions[pos.SessionID] = pos;
                    Globals.GlobalEnemyPositions[pos.SessionID].y = yOld;

                    if (!Globals.GlobalEnemyPositionsOld.ContainsKey(pos.SessionID))
                        Globals.GlobalEnemyPositionsOld.Add(pos.SessionID, Globals.GlobalEnemyPositions[pos.SessionID]);
                    else
                        Globals.GlobalEnemyPositionsOld[pos.SessionID] = Globals.GlobalEnemyPositions[pos.SessionID];

                    Globals.locked = false;
                }
                //GlobalPlayerPositionsOld[pos.SessionID] = GlobalPlayerPositions[pos.SessionID];
            }
        }
        Globals.locked = false;

    }

}
