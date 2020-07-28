// Decompiled with JetBrains decompiler
// Type: DraggableItem
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0EBE16EA-48A7-422E-A301-FC236929F611
// Assembly location: C:\Users\Dennis\Desktop\GW DevBuild\GameWhizzards_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class DraggableItem : Item
{
  public override void ActiveInInventory()
  {
    if (Input.GetMouseButton(0))
      this.Drag();
    else
      this.Drop();
  }

  protected virtual void Drag()
  {
    this.SetHidden(true);
    this.getInventory().SetDragged(this.gameObject);
  }

  protected virtual void Drop()
  {
    this.SetActive(false);
    this.getInventory().SetDragged((GameObject) null);
    if (this.getInventory().MouseOverInventory())
    {
      this.DroppedOnInventory(Input.mousePosition);
    }
    else
    {
      Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      RaycastHit hitInfo = new RaycastHit();
      if (Physics.Raycast(ray, out hitInfo))
        this.DroppedOn(hitInfo.transform.gameObject);
      else
        this.DroppedOn((GameObject) null);
    }
  }

  public virtual void DroppedOn(GameObject gameObject)
  {
    this.SetHidden(false);
  }

  public virtual void DroppedOnInventory(Vector3 mousePosition)
  {
    this.SetHidden(false);
  }
}
