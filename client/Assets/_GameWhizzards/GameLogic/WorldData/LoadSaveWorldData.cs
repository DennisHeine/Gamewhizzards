using LE_LevelEditor;
using LE_LevelEditor.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveWorldData  {
    public static void LoadWorldData(Data.WorldData.cWorldData data)
    {

        byte[] dataAsByteArray;
        byte[] metaAsByteArray;
        dataAsByteArray = data.ByteData;
        metaAsByteArray = data.ByteMetadata;

        if (dataAsByteArray.Length > 0 && metaAsByteArray.Length > 0)
        {

            // Search for an instance of the LE_LevelEditorMain.
            LE_LevelEditorMain lvlEditor = UnityEngine.Object.FindObjectOfType<LE_LevelEditorMain>();

            // You can either check with 'lvlEditor.IsReady' if the level editor is initialized (currently after the first update loop)
            // or simply add a callback like below.
            lvlEditor.ExecuteWhenReady(() =>
            {

                // Now that we know that the editor is initialized a load event can be acquired from it.
                // Execute the callbacks of the acquired event in order to load the level into the editor
                lvlEditor.GetLoadEvent().LoadLevelDataFromBytesCallback(dataAsByteArray);
                lvlEditor.GetLoadEvent().LoadLevelMetaFromBytesCallback(metaAsByteArray);

                // You could make some default operations on the level, since it is fully loaded now
                // For example you could make the camera always look at the player
            });

        }      
    }

    public static void OnSaveWorldData(object sender, LE_SaveEvent e)
    {

        byte[] levelDataAsByteArray = e.SavedLevelData;
        byte[] levelMetaAsByteArray = e.SavedLevelMeta;

        Data.WorldData.cWorldData WorldData = new Data.WorldData.cWorldData();
        WorldData.ByteMetadata = levelMetaAsByteArray;
        WorldData.ByteData = levelDataAsByteArray;
        Globals.con.SendObject("SaveWorldData", WorldData);

    }
}
