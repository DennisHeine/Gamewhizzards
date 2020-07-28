using Data;
using Data.Characters.CharacterInstances.States;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalPositionsUpdate  {

    public static void updateAvatarPositions()
    {
        if (true)
        {
            Globals.locked = true;
            foreach (cPositionData pos in Globals.GlobalPlayerPositions.Values)
            {
                if (pos.SessionID != Globals.SessionID)
                {
                    GameObject avatar = GameObject.Find(pos.SessionID);
                    Animator a = avatar.GetComponent<Animator>();
                    StateMachine sm;
                    sm = avatar.GetComponent<StateMachine>();

                    if (pos != null)
                    {
                        try
                        {
                            try
                            {
                                if (pos.moving)
                                    sm.changeState("Walking", StateTypes.STATE_WALKING, pos.SessionID);
                                else
                                    sm.changeState("Idle", StateTypes.STATE_IDLE, pos.SessionID);
                            }
                            catch (Exception) { }

                            Vector3 newpos = new Vector3();
                            float time = Time.deltaTime < 1 ? Time.deltaTime : 1;

                            if (((((GWTools.posToVector3(pos).x - avatar.transform.position.x) > time * 2) || ((GWTools.posToVector3(pos).x - avatar.transform.position.x) < -time * 2) || ((GWTools.posToVector3(pos).z - avatar.transform.position.z) > time * 2 || ((GWTools.posToVector3(pos).z - avatar.transform.position.z) < -time * 2)))) /* &&(pos.moving)*/)
                            {
                                sm.changeState("Walking", StateTypes.STATE_WALKING, pos.SessionID);
                                newpos.x = avatar.transform.position.x + ((GWTools.posToVector3(pos).x - avatar.transform.position.x) * (time * 5));
                                newpos.z = avatar.transform.position.z + ((GWTools.posToVector3(pos).z - avatar.transform.position.z) * (time * 5));
                            }
                            else
                            {
                                sm.changeState("Idle", StateTypes.STATE_IDLE, pos.SessionID);
                                newpos = GWTools.posToVector3(pos);
                            }

                            if (pos.y != 0)
                                newpos.y = pos.y;

                            //apply position
                            avatar.transform.position = newpos;
                            //Quaternion rot = avatar.transform.rotation;

                            //GameObject go = pos.target == Globals.SessionID ? GameObject.Find("vThirdPersonController") : GameObject.Find(pos.target);

                            Vector3 targetPoint = new Vector3(pos.x,pos.y, pos.z) - avatar.transform.position;
                            Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
                            avatar.transform.rotation = Quaternion.RotateTowards(avatar.transform.rotation, targetRotation, 360);

                  
                        }
                        catch (Exception ex) { PlayersData.spawnPlayer(pos); }
                    }
                }
            }

            if (!Globals.casting)
            {
                GameObject avatar1 = GameObject.Find("vThirdPersonController");
                StateMachine sm1 = avatar1.GetComponent<StateMachine>();
                if (Globals.PlayerPosition.moving)
                    sm1.changeState("Moving", StateTypes.STATE_WALKING, "vThirdPersonController");
                else
                    sm1.changeState("Idle", StateTypes.STATE_IDLE, "vThirdPersonController");
            }
            Globals.locked = false;
            try
            {
                foreach (cPositionData pos in Globals.GlobalPlayerPositionsOld.Values)
                {
                    GameObject avatar = GameObject.Find(pos.SessionID);
                    Vector3 newpos = GWTools.posToVector3(Globals.GlobalPlayerPositionsOld[pos.SessionID]);
                    Animator a = avatar.GetComponent<Animator>();
                    avatar.transform.position = newpos;
                    Globals.GlobalPlayerPositionsOld.Remove(pos.SessionID);

                    if (!(((GWTools.posToVector3(pos).x - avatar.transform.position.x) > 0.01) ||
                        ((GWTools.posToVector3(pos).x - avatar.transform.position.x) < -0.01) || ((GWTools.posToVector3(pos).z - avatar.transform.position.z) > 0.01) || ((GWTools.posToVector3(pos).z - avatar.transform.position.z) < -0.01)))
                    {
                        StateMachine sm;
                        sm = avatar.GetComponent<StateMachine>();
                        sm.changeState("Idle", StateTypes.STATE_IDLE, pos.SessionID);
                    }
                }
            }
            catch (Exception) { }
        }
    }

    public static void updateEnemyPositions()
    {
        if (true)
        {
            if (!Globals.locked)

            {
                try
                {

                    foreach (cPositionDataEnemys pos in Globals.GlobalEnemyPositions.Values)
                    {

                        if (pos.SessionID != Globals.SessionID && Globals.SessionID != "" && Globals.SessionID != null)
                        {
                            GameObject avatar = GameObject.Find(pos.SessionID);
			    
                            cPositionData player;

                            Globals.GlobalPlayerPositions.TryGetValue(Globals.SessionID, out player);

                            pos.y = 0;
                            Animator a = avatar.GetComponent<Animator>();
                            StateMachine sm;
                            sm = avatar.GetComponent<StateMachine>();
	                    //----Damage---
			    //Enemy is targeted by a player
                            if (player.target != null)
                            {
				//target is current enemy
                                if (player.target == pos.SessionID)
                                {
				    //attack
                                    GameObject HealthBar = GameObject.Find("Health Bar");
                                    if (HealthBar != null)
                                    {
                                        float hp = (float)Globals.GlobalEnemyPositions[pos.SessionID].charData.CharacterStats.Health;
                                        hp = hp / 100.00f;
                                        HealthBar.GetComponent<UIProgressBar>().value = hp;
                                        Debug.Log(pos.charData.CharacterStats.Health);
                                    }
                                    else
                                        Debug.Log("ERRRR");



                                    GameObject HealthBarPlayer = GameObject.Find("Energy Bar");
                                    if (HealthBarPlayer != null)
                                    {
                                        float hp = (float)Globals.GlobalPlayerPositions[pos.target].character.CharacterStats.Health;
                                        hp = hp / 100.00f;
                                        HealthBarPlayer.GetComponent<UIProgressBar>().value = hp;

                                    }
                                    else
                                        Debug.Log("ERRRR");
                                }
                            }
                         
                            if (pos != null)
                            {
                                try
                                {
                                    try
                                    {
					
						
					
					//set animatin according to whether mob is moving
					if(sm.currentState!="Attacking" && sm.currentState!="Walking"&& sm.currentState!="Running")
                                        //	if (pos.moving)
                                            		sm.changeState("Moving", StateTypes.STATE_WALKING, pos.SessionID);
                                        	
                                    }
                                    catch (Exception) { }
                                    //sm.changeState("Idle", StateTypes.STATE_IDLE, pos.SessionID);

                                    Vector3 newpos = avatar.transform.position;
                                    float time = Time.deltaTime;// < 0.01f ? Time.deltaTime : 0.01f;                                   

                                    float avaX = 0;
                                    float avaY = 0;
                                    //Get current position of enemy gameobject(rounded)
                                    avaX = (float)Math.Round(avatar.transform.position.x, 1, MidpointRounding.AwayFromZero);//   avatar.transform.position.x-(avatar.transform.position.x % 1f);
                                    avaY = (float)Math.Round(avatar.transform.position.z, 1, MidpointRounding.AwayFromZero);//   avatar.transform.position.x-(avatar.transform.position.x % 1f);

				    //get new enemy position (from server)
                                    float targetX = (float)Math.Round(pos.x, 1, MidpointRounding.AwayFromZero);//   avatar.transform.position.x-(avatar.transform.position.x % 1f);
                                    float targetY = (float)Math.Round(pos.z, 1, MidpointRounding.AwayFromZero);//   avatar.transform.position.x-(avatar.transform.position.x % 1f);
					
                                 //   if (pos.target != null && Globals.GlobalPlayerPositions.ContainsKey(pos.target))
                                    {
                                        
                                  
                                    //    if (pos.isInFight == false)
 					    //if not colliding
                                            if (!avatar.GetComponent<DoCollission>().Stoped)                                            
                                            {

                                                //got target and target is valid
                                                if (pos.target != null && Globals.GlobalPlayerPositions.ContainsKey(pos.target))
                                                {
                                                    //Set animation
                                                    //if (!Globals.inFight)
						    if(sm.currentState!="Running")
                                                       sm.changeState("Running", StateTypes.STATE_RUNNING, pos.SessionID);
                                                   // else if(avatar.GetComponent<DoCollission>().Stoped)
                                                    //    sm.changeState("Idle", StateTypes.STATE_IDLE, pos.SessionID);
	                                        //    if(sm.currentState!="Attacking")
						//    	sm.changeState("Attacking", StateTypes.STATE_FIGHTING, pos.SessionID);
						    //set go to either current player, or other multiplayer character
                                                    GameObject go = pos.target == Globals.SessionID ? GameObject.Find("vThirdPersonController") : GameObject.Find(pos.target);
						    //Rotate towards player
                                                    Vector3 targetPoint = new Vector3(go.transform.position.x, avatar.transform.position.y, go.transform.position.z) - avatar.transform.position;
                                                    Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
                                                    avatar.transform.rotation = Quaternion.RotateTowards(avatar.transform.rotation, targetRotation, 360);
                                                   //set movement towards new positin
                                                newpos.x = avatar.transform.position.x + ((GWTools.posEnemyToVector3(pos).x - avatar.transform.position.x) * time);
                                                newpos.z = avatar.transform.position.z + ((GWTools.posEnemyToVector3(pos).z - avatar.transform.position.z) * time);

						avatar.transform.position = newpos; 
 						Globals.inFight = false;
                                                }
                                               
                                                
                                            }
                                            if(avatar.GetComponent<DoCollission>().Stoped )


						{
                                            Globals.inFight = true;
					    if(sm.currentState!="Attacking")
					   	 sm.changeState("Attacking", StateTypes.STATE_FIGHTING, pos.SessionID);
                                            GameObject go = pos.target == Globals.SessionID ? GameObject.Find("vThirdPersonController") : GameObject.Find(pos.target);

                                            Vector3 targetPoint = new Vector3(go.transform.position.x, avatar.transform.position.y, go.transform.position.z) - avatar.transform.position;
                                            Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
                                            avatar.transform.rotation = Quaternion.RotateTowards(avatar.transform.rotation, targetRotation, 360);
                                        }
                                       
				    //apply position
                                     
                                    }// END NO TARGET
                              //      else
                                    {
                                 /*       //HAS TARGET

                                        if (!avatar.GetComponent<DoCollission>().Stoped)
                                        {
                                            Vector3 targetPoint = new Vector3(pos.x, avatar.transform.position.y, pos.z) - avatar.transform.position;
                                            Quaternion targetRotation = Quaternion.LookRotation(targetPoint, Vector3.up);
                                            avatar.transform.rotation = Quaternion.Slerp(avatar.transform.rotation, targetRotation, Time.deltaTime * 10.0f);

                                            sm.changeState("Walking", StateTypes.STATE_WALKING, pos.SessionID);
                                            newpos.x = avatar.transform.position.x + ((GWTools.posEnemyToVector3(pos).x - avatar.transform.position.x) * time*5);
                                            newpos.z = avatar.transform.position.z + ((GWTools.posEnemyToVector3(pos).z - avatar.transform.position.z) * time*5);

                                        }
                                        else
                                        {
                                            sm.changeState("Attacking", StateTypes.STATE_FIGHTING, pos.SessionID);
                                            newpos.x = avatar.transform.position.x;
                                            newpos.z = avatar.transform.position.z;

                                        }*/
                                    }//END HAS TARGET

                                    //apply position
                                    avatar.transform.position = newpos;                                   
                                }
                                catch (Exception ex) { EnemysData.spawnEnemy(pos); }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Globals.locked = false;
                }
                Globals.locked = false;                
            }
        }
    }

}
