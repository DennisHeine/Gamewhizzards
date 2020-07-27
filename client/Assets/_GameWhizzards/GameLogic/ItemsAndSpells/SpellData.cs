using Data;
using Data.Spells;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public static class SpellData  {
    private static Thread boltThread;
    public static void CheckInputAndCast(cPositionData pos, GameObject avatar)
    {
        foreach (Data.Spells.Spell spell in pos.character.Spells.Values)
        {
            if (Input.GetKeyDown(spell.Hotkey))
            {
                String hotkey = spell.Hotkey;
                if ((Globals.Cooldowns.ContainsKey(spell.Name) && Globals.Cooldowns[spell.Name].Cooldown_Left <= 0) || !Globals.Cooldowns.ContainsKey(spell.Name) && !Globals.casting)
                {
                    Debug.Log(UpdatePlayerTarget.playerTarget);
                    if (UpdatePlayerTarget.playerTarget != null)
                    {


                        GameObject target = GameObject.Find(UpdatePlayerTarget.playerTarget);


                        GameObject s = GameObject.Find(spell.SourceObject);
                        GameObject e = GameObject.Find(spell.TargetObject);

                        Vector3 posNewAvatar = avatar.transform.position;
                        posNewAvatar.y += (float)1;

                        Vector3 posNewTarget = target.transform.position;
                        posNewTarget.y += (float)1;

                        s.transform.position = posNewAvatar;
                        e.transform.position = posNewTarget;
                        Globals.bolVisible = true;
                        Globals.casting = true;
                        boltThread = new System.Threading.Thread(endBoltThrd);
                        boltThread.Start();



                        Globals.castingAudioEffectMuted = false;
                        avatar.GetComponent<Animator>().runtimeAnimatorController = GameObject.Instantiate((RuntimeAnimatorController)Resources.Load("Anim1"));


                        if (UpdatePlayerTarget.playerTarget != null)
                        {
                            Data.Actions.Fight.cAttack attack = new Data.Actions.Fight.cAttack();
                            attack.IDPlayer = Globals.SessionID;
                            attack.TargetID = UpdatePlayerTarget.playerTarget;
                            Spell retS = null;
                            foreach (Spell sp in Globals.PlayerPosition.character.Spells.Values)
                            {
                                if (sp.Hotkey == hotkey)
                                {
                                    retS = sp;
                                }
                            }
                            attack.Spell = retS;
                            GameObject SpellSlot = GameObject.Find("Slot " + hotkey);
                            if (SpellSlot)
                            {
                                SpaceD_Cooldown cd = SpellSlot.GetComponent<SpaceD_Cooldown>();
                                cd.StartCooldown(retS.Cooldown);
                            }

                            Globals.con.SendObject("AttackTargetPlayer", attack);
                        }

                    }                   
                }               
            }
            GameObject bolt = GameObject.Find(spell.MainObject);
            bolt.GetComponent<AudioSource>().mute = Globals.castingAudioEffectMuted;
            if (Globals.bolVisible == false && Globals.casting)
            {


                avatar.GetComponent<Animator>().runtimeAnimatorController = GameObject.Instantiate((RuntimeAnimatorController)Resources.Load("Invector_BasicLocomotionLITE"));

                GameObject s = GameObject.Find(spell.SourceObject);
                GameObject e = GameObject.Find(spell.TargetObject);

                s.transform.position = Vector3.one;
                e.transform.position = Vector3.one;
                Globals.casting = false;
            }
        }
    }
    
    public static void muteThrd()
    {
        System.Threading.Thread.Sleep(647);
        Globals.mute = true;
    }

    private static void endBoltThrd()
    {
        System.Threading.Thread.Sleep(1000);
        Globals.castingAudioEffectMuted = true;
        Globals.bolVisible = false;
    }

}
