using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GWTools {
    public static Vector3 posEnemyToVector3(cPositionDataEnemys pos)
    {
        Vector3 v = new Vector3(pos.x, pos.y, pos.z);
        return v;
    }

    public static Vector3 posToVector3(cPositionData pos)
    {
        Vector3 v = new Vector3(pos.x, pos.y, pos.z);
        return v;
    }

    public static double GetDistance(double x1, double y1, double x2, double y2)
    {
        //pythagorean theorem c^2 = a^2 + b^2
        //thus c = square root(a^2 + b^2)
        double a = x1 - x2;
        double b = y1 - y2;
        double ret = Math.Sqrt(a * a + b * b);
        return ret;
    }

    public static cPositionData genPosDataFromGameObject(GameObject avatar)
    {
        cPositionData currpos = new cPositionData();
        currpos.SessionID = Globals.SessionID;
        currpos.x = avatar.transform.position.x;
        currpos.y = avatar.transform.position.y;
        currpos.z = avatar.transform.position.z;
        currpos.rx = avatar.transform.rotation.z;
        currpos.ry = avatar.transform.rotation.y;
        currpos.rz = avatar.transform.rotation.z;
        currpos.rw = avatar.transform.rotation.w;
        currpos.target = UpdatePlayerTarget.playerTarget;
        return currpos;
    }

    public static void unlockMouse()
    {
        GameObject v = GameObject.Find("vThirdPersonCamera");
        vThirdPersonCamera cam = v.GetComponent<vThirdPersonCamera>();
        cam.lockCamera = true;
        /*Invector.CharacterController.vThirdPersonInput i = v.GetComponent<Invector.CharacterController.vThirdPersonInput>();
        Invector.CharacterController.vThirdPersonController c = v.GetComponent<Invector.CharacterController.vThirdPersonController>();
        c.enabled = false;
        i.enabled = false;*/

        Invector.CharacterController.vThirdPersonAnimator a = v.GetComponent<Invector.CharacterController.vThirdPersonAnimator>();
        Invector.CharacterController.vThirdPersonInput i = v.GetComponent<Invector.CharacterController.vThirdPersonInput>();
        Invector.CharacterController.vThirdPersonController c = v.GetComponent<Invector.CharacterController.vThirdPersonController>();
   //     c.lockMovement = true;
        //c.enabled = false;
    //    a.enabled = false;
       // i.enabled = false;
        //i.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        c.keepDirection = false;
    }

    public static void lockMouse()
    {
        Cursor.lockState = CursorLockMode.Locked;
        GameObject v = GameObject.Find("vThirdPersonCamera");
        vThirdPersonCamera cam = v.GetComponent<vThirdPersonCamera>();
        cam.lockCamera = false;

        Invector.CharacterController.vThirdPersonAnimator a = v.GetComponent<Invector.CharacterController.vThirdPersonAnimator>();
        Invector.CharacterController.vThirdPersonInput i = v.GetComponent<Invector.CharacterController.vThirdPersonInput>();
        Invector.CharacterController.vThirdPersonController c = v.GetComponent<Invector.CharacterController.vThirdPersonController>();

   //     i.enabled = true;
     //   c.lockMovement = false;
     //   c.enabled = true;
     //   a.enabled = true;

        //i.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

    }
}
