using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Text;
using UnityEngine.SceneManagement;

public class Player_Manager : MonoBehaviour
{

    // The weapon the player has.
    public Scene MainHandWeapon;

    void Start()
    {
        
    }

    public static void Save(GameObject o)    
    {
        float test = 50;
        
        Debug.Log(Application.persistentDataPath);

        // Stream the file with a File Stream. (Note that File.Create() 'Creates' or 'Overwrites' a file.)
        FileStream file = File.Create("e:\\PlayerData.dat");
        // Create a new Player_Data.
        Player_Data data = new Player_Data();
        //Save the data.
        data.weapon = o;

        //Serialize to xml
        DataContractSerializer bf = new DataContractSerializer(data.GetType());
        MemoryStream streamer = new MemoryStream();

        //Serialize the file
        bf.WriteObject(streamer, data);
        streamer.Seek(0, SeekOrigin.Begin);

        //Save to disk
        file.Write(streamer.GetBuffer(), 0, streamer.GetBuffer().Length);

        // Close the file to prevent any corruptions
        file.Close();

        string result = XElement.Parse(Encoding.ASCII.GetString(streamer.GetBuffer()).Replace("\0", "")).ToString();
        Debug.Log("Serialized Result: " + result);

    }
}


[DataContract]
class Player_Data
{
    [DataMember]
    private GameObject _weapon;

    public GameObject weapon
    {
        get { return _weapon; }
        set { _weapon = value; }
    }
}