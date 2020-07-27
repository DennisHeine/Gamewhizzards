using Data;
using Data.Characters.CharacterInstances.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnemysData
{

    public static void AttackingEnemys()
    {
        foreach (String id in Globals.GlobalAttackingEnemys)
        {
            try
            {
                GameObject avatar1 = GameObject.Find(id);
                StateMachine sm;
                sm = avatar1.GetComponent<StateMachine>();
                sm.changeState("Attacking", StateTypes.STATE_FIGHTING, id);
                try
                {
                    GameObject avatar2 = GameObject.Find("FANTASY_WOLF");
                    AudioSource s = avatar2.GetComponent<AudioSource>();
                    s.Stop();
                    s.Play();
                    s.mute = false;
                    s.loop = false;
                    Globals.mute = false;
                    System.Threading.Thread t = new System.Threading.Thread(SpellData.muteThrd);
                    t.Start();
                }
                catch (Exception ex) { }
            }
            catch (Exception ex) { }
        }

        Globals.GlobalAttackingEnemys.Clear();
    }

    public static void SpawnEnemys()
    {
        foreach (cPositionDataEnemys data in Globals.enemysToSpawn.Values)
        {
            if (data != null)
            {
                spawnEnemy(data);
            }
        }
        Globals.enemysToSpawn.Clear();
    }

    public static void RemoveEnemys()
    {
        foreach (String id in Globals.EnemysToRemove)
            removeEnemy(id);
        Globals.EnemysToRemove.Clear();
    }

    public static void spawnEnemy(cPositionDataEnemys data)
    {
        if (GameObject.Find(data.SessionID) == null)
        {
            GameObject player = GameObject.Find(data.charData.protoAssetName);

            GameObject cube = GameObject.Instantiate(player);

            cube.name = data.SessionID;
            cube.transform.position = new Vector3(data.x, data.y, data.z);



            Rigidbody r = cube.AddComponent<Rigidbody>();
            r.useGravity = true;
            r.mass = 1;
            r.isKinematic = false;

            r.collisionDetectionMode = CollisionDetectionMode.Continuous;

            r.freezeRotation = true;


            GameObject tpc = GameObject.Find("vThirdPersonController");
            CapsuleCollider cOld = cube.GetComponent<CapsuleCollider>();

            Vector3 c = cOld.center;
            c.y = (float)0.6;
            cOld.center = c;            

        }
    }


    public static void removeEnemy(String ID)
    {
        GameObject player = GameObject.Find(ID);
        GameObject.Destroy(player);
    }


}
