using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkCommsDotNet;
using Data;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Connections;
using System;
using UnityEngine.Networking;
using Data.Characters.CharacterInstances.States;
using System.Net;
using System.Net.Sockets;
using IrcClasses;
using TriLib;
using System.IO;
using Data.Spells;
using LE_LevelEditor.Events;
using LE_LevelEditor;

public class init : MonoBehaviour
{

    
    // Use this for initialization
    void Start()
    {
        //Networking events
        NetworkHandler h = new NetworkHandler();

        StateMachine sm;
        sm = new StateMachine();
        
        if (!Directory.Exists(Application.dataPath + "\\GW_Data\\"))
            Directory.CreateDirectory(Application.dataPath + "\\GW_Data\\");
       
        //Connect to server
        IPHostEntry hostEntry;
        hostEntry = Dns.GetHostEntry("gw.h2x.us");

       // String ServerIP = "192.168.2.109";
        var ServerIP = hostEntry.AddressList[0];
        ConnectionInfo serverConnectionInfo = new ConnectionInfo(ServerIP.ToString(), 10000);
        Globals.con = TCPConnection.GetConnection(serverConnectionInfo, true);

        //Login
        doLogin();

        //Show Chat
        GameObject o = GameObject.Find("IrcWindow");
        IrcGui irc = o.GetComponent<IrcGui>();
        o.active = true;
        irc.enabled = true;
        irc.showWindow = true;
        
        //Hide World Editor
        GameObject c = GameObject.Find("Canvas");//GetComponent<Canvas>();
        Canvas ca = c.GetComponent<Canvas>();
        ca.enabled = false;

  

        //OnSave handler
        LE_EventInterface.OnSave += LoadSaveWorldData.OnSaveWorldData;

        //Hide Inventory
        Globals._Inventory = GameObject.Find("Inventory");
        Inventory inv=Globals._Inventory.AddComponent<Inventory>();
        inv.inventory_area_background = (Texture2D)Resources.Load<Texture2D>("BlankPanel-3");

        Globals._Inventory.SetActive(false);

        GameObject dragme = GameObject.Find("DragMe");
        dragme.AddComponent<DragMe>();


        //Start cooldowns thread
        Globals.cooldownThread = new System.Threading.Thread(CooldownThrd);
        Globals.cooldownThread.Start();

        //Request world update
        Globals.con.SendObject("RequestWorldData", true);
    }
          
    void Update()
    {           
        //Is player logged in?
        if (Globals.SessionID != "" && Globals.SessionID != null && !Globals.dead)
        {
            GameObject avatar = GameObject.Find("vThirdPersonController");
            cPositionData pos = Globals.PlayerPosition;

            //----Global stuff-----
            if (Globals.loadWorldData)
            {
                Globals.loadWorldData = false;
                LoadSaveWorldData.LoadWorldData(Globals.wd);
            }

            if (Globals.mute)
            {
                GameObject avatar2 = GameObject.Find("FANTASY_WOLF");
                AudioSource s = avatar2.GetComponent<AudioSource>();
                s.mute = true;
            }

            if (Globals.dead)
                Destroy(avatar);


            //----Key Input---
            if (Input.GetKeyUp("space"))
            {
                //Change Level
                //Globals.con.SendObject("ChangeWorld", new String[] { "1", Globals.SessionID });
            }
            //toggle inventory
            if (Input.GetKeyUp(KeyCode.I))
            {
                Globals._Inventory.SetActive(!Globals._Inventory.active);
            }
            
            if(Input.GetKeyUp("escape"))
            {
                if (UpdatePlayerTarget.playerTarget != null)
                {
                    Globals.con.SendObject<string>("CancelTarget", Globals.SessionID);
                }
            }

            //Lock/unlock mouse/movement
            if (Input.GetMouseButton(1))
            {

                if (Cursor.visible == true)
                {
                    Cursor.visible = false;
                    GWTools.lockMouse();
                }
                /*
                //Cancel targeting
                if (UpdatePlayerTarget.playerTarget != null)
                {
                    Globals.con.SendObject<string>("CancelTarget", Globals.SessionID);
                }
                else//Lock/unlock cursor
                {
                    if (Cursor.visible == false)
                    {
                        Cursor.visible = true;
                        GWTools.unlockMouse();
                    }
                    else
                    {
                        Cursor.visible = false;
                        GWTools.lockMouse();
                    }
                }*/
            }
            else
            {
                if (Cursor.visible == false)
                {
                    Cursor.visible = true;
                    GWTools.unlockMouse();
                }
            }
            //-------Data update functions----------

            //Inventory
            InventoryData.UpdateInventory();

            GameObject inv = GameObject.Find("Inventory");
            if (inv != null)
            {
                Inventory inv_sc = inv.GetComponent<Inventory>();
                
                for (int i = 0; i < inv_sc.items.Count; i++)
                {
                    GameObject goItem = (GameObject)inv_sc.items[i];
                    Item scItem = goItem.GetComponent<Item>();
                    bool found = false;
                    
                    foreach (Data.Items.Item itmEqu in Globals.PlayerPosition.character.equipment.Values)
                    {
                        Debug.Log(itmEqu.ItemID);
                        if (itmEqu.ItemID == scItem.item_dat.ItemID)
                        {
                            found = true;                            
                        }
                    }
                    if (found)
                    {
                        scItem.textColor = Color.yellow;                        
                    }
                    else
                    {
                        scItem.textColor = Color.white;
                    }
                    scItem.inventoryTexture = Resources.Load<Texture2D>("sword");
                    inv.GetComponent<Inventory>().items[i] = goItem;
                }
            }


            foreach(Data.Items.Item itm in Globals.Loot.Values)
            {
                if (!GameObject.Find(itm.ItemID))
                {
                    GameObject newObj = GameObject.Instantiate(GameObject.Find("DragMe"));
                    newObj.name = itm.ItemID;
                    newObj.transform.position = new Vector3(itm.x, Terrain.activeTerrain.SampleHeight(new Vector3(itm.x,0,itm.z)), itm.z);

                    GameObject defaultObj = GameObject.Instantiate(GameObject.Find("default"));
                    newObj.AddChild(defaultObj);
                }
            }
            


            //Spell Casting
            SpellData.CheckInputAndCast(pos, avatar);
            //Attacking Enemys - Statemachine for attacking enemies
            EnemysData.AttackingEnemys();                                               

            //Spawn new Players/Enemys
            EnemysData.SpawnEnemys();
            PlayersData.SpawnPlayers();

            //Destroy obsolete Players/Enemys
            EnemysData.RemoveEnemys();
            PlayersData.RemovePlayers();

            //------Position update--------------
            //Get current position
            cPositionData currpos = GWTools.genPosDataFromGameObject(avatar);
            //Send current position to server
            Globals.con.SendObject("GlobalPositionUpdate", currpos);
            //Update players and enemys positions
            GlobalPositionsUpdate.updateAvatarPositions();
            GlobalPositionsUpdate.updateEnemyPositions();
            

        }
    }

    void OnGUI()
    {
        if (Globals.dead)
            GUI.Box(new Rect(0, 0, Screen.width, Screen.height), "You are dead.");
    }

    private void doLogin()
    {
        LoginData dat = new LoginData();
        dat.Username = "user";
        dat.Password = "pass";
        Globals.con.SendObject("Login", dat);
    }

    private void CooldownThrd(object obj)
    {
        while (true)
        {
            Globals.con.SendObject("RequestUpdateCooldowns", Globals.SessionID);
            System.Threading.Thread.Sleep(100);
        }
    }
}

