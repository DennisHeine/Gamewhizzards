using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data.Characters.CharacterInstances.States;
using Data;
using System;
using UnityEngine.Audio;
using System.IO;

public class StateMachine : MonoBehaviour
{
    public Dictionary<String, AudioClip> AudioPool = new Dictionary<string, AudioClip>();
    public StatesCollection col = new StatesCollection();
    public GameObject avatar;
    public bool playContinousJN;
    String currentState = "";
    public AudioSource s;

    private void Awake()
    {
        avatar = gameObject;
        s = avatar.GetComponent<AudioSource>();
        if (col.AvailableStates.Count == 0)
            generateNewDefaultHumanStates();
    }
      
    void Update()
    {
        if (s != null)
        {
            if(s.clip!=null)
            if (!s.isPlaying && s.clip.loadState == AudioDataLoadState.Loaded && currentState != "Idle")
            {
                s.Play();
            }
            else if (currentState == "Idle")
                s.Stop();
        }
    }    
    
    public void generateNewDefaultHumanStates()
    {
        col.AvailableStates.Clear();
        String path = Environment.CurrentDirectory;
        path = path.Replace("\\", "/");
        State state = new State("Walking", StateTypes.STATE_WALKING, true, "Moving", "file://" + path + "/footsteps.ogg");
        col.AvailableStates.Add("Walking", state);

        state = new State("Attacking", StateTypes.STATE_FIGHTING, true, "Attack6Trigger", "");
        col.AvailableStates.Add("Attacking", state);

        state = new State("Running", StateTypes.STATE_RUNNING, true, "Running", "");
        col.AvailableStates.Add("Running", state);
        loadPool("Walking", "file://" + path + "/footsteps.ogg");
        state = new State("Idle", StateTypes.STATE_IDLE, false, "Moving", "");
        col.AvailableStates.Add("Idle", state);
     
    }

    public void loadPool(String key, String path)
    {                
        string url = string.Format(path);
        WWW www = new WWW(url);
        while (!www.isDone)
            System.Threading.Thread.Sleep(100);
        AudioPool.Add(key,www.GetAudioClip(false, false, AudioType.OGGVORBIS));
    }

    void LoadSongCoroutine(String key,String path)
    {
        if (AudioPool.ContainsKey(key))
        {
            s.clip = AudioPool[key];
            s.clip.LoadAudioData();
        }
    }
    
    public void changeState(string newStateName, StateTypes type, string ID)
    {
        if (newStateName != currentState)
        {
            currentState = newStateName;                
            //Animation
            State s1 = col.AvailableStates[newStateName];
            Animator a = avatar.GetComponent<Animator>();

            a.SetBool("Moving", false);
            a.SetBool("Running", false);


            if (type == StateTypes.STATE_FIGHTING)
                a.SetTrigger(s1.Animation);


            if (type == StateTypes.STATE_RUNNING)
                a.SetBool("Moving", true);
            if (type == StateTypes.STATE_IDLE)
            {
                a.SetBool("Running", false);
                a.SetBool("Moving", false);
            }
            else
                a.SetBool(s1.Animation, s1.OnOff);
            s = avatar.GetComponent<AudioSource>();
            
            
            //gameObject.GetComponent<Animator>().Play("BaseLayer.Unarmed-Run-Forward");

            //Sound
            playContinousJN = s1.StateType == StateTypes.STATE_WALKING ? true : false;
            if (s1.StateType != StateTypes.STATE_IDLE)
            {
                if(AudioPool.ContainsKey(s1.Name))
                    LoadSongCoroutine(s1.Name,s1.Sound);
            }
            else
            {                
                s.Stop();
            }
        }
    }       
}
