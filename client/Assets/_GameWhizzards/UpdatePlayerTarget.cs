using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetworkCommsDotNet;
using Data;
using NetworkCommsDotNet.Connections.TCP;
using NetworkCommsDotNet.Connections;
using System;

public class UpdatePlayerTarget : MonoBehaviour {    
    public static Connection con = null;
    public static string SessionID = "";
    public static string playerTarget=null;

    

	// Use this for initialization
	void Start () {
        NetworkComms.AppendGlobalIncomingPacketHandler<string>("SetPlayerTarget", HandleSetPlayerTarget);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (con != null && SessionID != "")
        {
            if (Input.GetKeyUp(KeyCode.Tab))
                con.SendObject("RequestCycleTarget", SessionID);
            setPlayerTarget();
        }
    }

    private void HandleSetPlayerTarget(PacketHeader packetHeader, Connection connection, string incomingObject)
    {
        Debug.Log("X");
        playerTarget = incomingObject;
    }

    private void setPlayerTarget()
    {
        foreach (cPositionDataEnemys e in Globals.GlobalEnemyPositions.Values)
        {
            try
            {
                GameObject o = GameObject.Find(e.SessionID);
                if (o.GetComponent<Light>())
                    o.GetComponent<Light>().enabled = false;
            }
            catch (Exception ex) { }
        }
        if (playerTarget != null)
        {
            GameObject camera = GameObject.Find("vThirdPersonCamera");
            AudioSource s = camera.GetComponent<AudioSource>();
            float[] data;
            s.clip = Resources.Load<AudioClip>("BarrenBoss");
            

            foreach (cPositionDataEnemys e in Globals.GlobalEnemyPositions.Values)
            {
                if (e.SessionID == playerTarget)
                {
                    GameObject o = GameObject.Find(e.SessionID);
                    o.GetComponent<Light>().enabled = true;
                }
            }
        }
        else
        {
            GameObject camera = GameObject.Find("vThirdPersonCamera");
            AudioSource s = camera.GetComponent<AudioSource>();
            float[] data;
            s.clip = Resources.Load<AudioClip>("CalmAmbient");
        }
    }
}
