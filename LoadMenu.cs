using IrcClasses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject o = GameObject.Find("IrcWindow");
        IrcGui irc = o.GetComponent<IrcGui>();
        irc.enabled = false;
        irc.showWindow = false;
        o.active = false;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
