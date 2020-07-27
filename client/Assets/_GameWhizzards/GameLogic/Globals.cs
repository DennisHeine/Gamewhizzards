using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class Globals  {
    
    public static Rect windowRect = new Rect((Screen.width - 200) / 2, (Screen.height - 300) / 2, 200, 300);
    public static NetworkCommsDotNet.Connections.Connection con;
    public static Dictionary<string, cPositionData> GlobalPlayerPositions = new Dictionary<string, cPositionData>();
    public static Dictionary<string, cPositionData> GlobalPlayerPositionsOld = new Dictionary<string, cPositionData>();
    public static Dictionary<string, cPositionDataEnemys> GlobalEnemyPositions = new Dictionary<string, cPositionDataEnemys>();
    public static Dictionary<string, cPositionDataEnemys> GlobalEnemyPositionsOld = new Dictionary<string, cPositionDataEnemys>();
    public static cPositionData PlayerPosition = new cPositionData();
    public static GameObject _Inventory;

    public static List<string> PlayersToRemove = new List<string>();
    public static Hashtable playersToSpawn = new Hashtable();
    public static Hashtable enemysToSpawn = new Hashtable();
    public static List<string> EnemysToRemove = new List<string>();
    public static Dictionary<String, Data.Spells.Spell> Cooldowns = new Dictionary<string, Data.Spells.Spell>();
    public static System.Threading.Thread cooldownThread = null;
    public static bool locked = false;
    public static List<Data.Characters.Inventory.Inventory.ItemData> InventoryItems = new List<Data.Characters.Inventory.Inventory.ItemData>();
    public static List<Data.Characters.Inventory.Inventory.ItemData> InventoryItemsNew = new List<Data.Characters.Inventory.Inventory.ItemData>();
    public static List<String> GlobalAttackingEnemys = new List<string>();
    public static bool exit = false;
    public static bool dead = false;
    public static bool inFight = false;

    public static cPositionDataEnemys playerTarget = null;




    public static Data.WorldData.cWorldData wd = new Data.WorldData.cWorldData();
    public static bool loadWorldData = false;
    public static Transform t;
    public static cPositionData newPos;

    public static String SessionID = "";
    public static GameObject o;
    
    public static bool castingAudioEffectMuted = true;
    public static bool mute = false;
    public static bool casting = false;
    public static bool bolVisible = false;
    internal static IEnumerable<Item> PayerEquipment;
    public static Dictionary<String, Data.Items.Item> Loot = new Dictionary<string, Data.Items.Item>();
}
