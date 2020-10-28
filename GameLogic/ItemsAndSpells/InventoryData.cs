using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InventoryData {
   

    public static void UpdateInventory()
    {
        bool reload = false;

        foreach (GameObject invItem in Globals._Inventory.GetComponent<Inventory>().items)
        {
            bool found = false;
            DraggableItem di = invItem.GetComponent<DraggableItem>();
            Data.Items.Item item = di.item_dat;

            foreach (Data.Characters.Inventory.Inventory.ItemData dat in Globals.InventoryItemsNew)
            {
                if (dat.Item.Name == item.Name)
                    found = true;
            }
            if (!found)
                reload = true;
        }

        foreach (Data.Characters.Inventory.Inventory.ItemData dat in Globals.InventoryItemsNew)
        {
            bool found1 = false;
            foreach (GameObject invItem in Globals._Inventory.GetComponent<Inventory>().items)
            {
                if (dat.Item.Name == invItem.GetComponent<DraggableItem>().item_dat.Name)
                    found1 = true;
            }
            if (!found1)
                reload = true;
        }

        if (reload)
        {
            Globals._Inventory.GetComponent<Inventory>().items.Clear();
            foreach (Data.Characters.Inventory.Inventory.ItemData dat in Globals.InventoryItemsNew)
            {

                GameObject newItem = GameObject.Instantiate(GameObject.Find("DragMe"));
                newItem.AddComponent<DraggableItem>();
                newItem.GetComponent<DraggableItem>().item_name = dat.Item.Name;
                newItem.GetComponent<DraggableItem>().item_dat = dat.Item;
                Globals._Inventory.GetComponent<Inventory>().addItem(newItem);
            }
            Globals.InventoryItems = Globals.InventoryItemsNew;
        }
    }
    
}
