using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayersData
{
    public static void SpawnPlayers()
    {
        foreach (cPositionData data in Globals.playersToSpawn.Values)
        {
            if (data != null)
            {
                spawnPlayer(data);
            }
        }
        Globals.playersToSpawn.Clear();
    }

    public static void RemovePlayers()
    {
        foreach (String id in Globals.PlayersToRemove)
            removePlayer(id);
        Globals.PlayersToRemove.Clear();
    }

    public static void spawnPlayer(cPositionData pos)
    {
        if (GameObject.Find(pos.SessionID) == null)
        {
            GameObject player = GameObject.Find("Avatar");

            GameObject cube = GameObject.Instantiate(player);
            cube.name = pos.SessionID;
            cube.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }
    }

    public static void removePlayer(String ID)
    {
        GameObject player = GameObject.Find(ID);
        GameObject.Destroy(player);
    }
}
