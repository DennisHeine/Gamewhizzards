// Decompiled with JetBrains decompiler
// Type: Item
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0EBE16EA-48A7-422E-A301-FC236929F611
// Assembly location: C:\Users\Dennis\Desktop\GW DevBuild\GameWhizzards_Data\Managed\Assembly-CSharp.dll

using Data;
using UnityEngine;

public class Item : MonoBehaviour
{
  public Color textColor = Color.white;
  public Texture2D inventoryTexture;
  public string item_name;
  public Data.Items.Item item_dat;
  private bool is_active;
  private bool is_hidden;

  public bool GetActive()
  {
    return this.is_active;
  }

  public void SetActive(bool b)
  {
    this.is_active = b;
  }

  public bool GetHidden()
  {
    return this.is_hidden;
  }

  public void SetHidden(bool b)
  {
    this.is_hidden = b;
  }

  private void Start()
  {
  }

  public Inventory getInventory()
  {
    return GameObject.Find("Inventory").GetComponent(typeof (Inventory)) as Inventory;
  }

  public Texture2D getTexture()
  {
    return this.inventoryTexture;
  }

  public string getName()
  {
    return this.item_name;
  }

  private void OnMouseDown()
  {
    this.getInventory().addItem(this.gameObject);
    this.gameObject.SetActiveRecursively(false);
  }

  public virtual void ClickedInInventory(GameObject item)
  {
    EquipItemPackage objectToSend = new EquipItemPackage();
    objectToSend.SessionID = Globals.SessionID;
    Data.Items.Item itemDat = item.GetComponent<EquipmentItem>().item_dat;
    objectToSend.Item = itemDat;
    Globals.con.SendObject<EquipItemPackage>("EquipItem", objectToSend);
    Debug.Log((object) "Clicked...");
  }

  public virtual void ActiveInInventory()
  {
  }
}
