// Decompiled with JetBrains decompiler
// Type: Inventory
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0EBE16EA-48A7-422E-A301-FC236929F611
// Assembly location: C:\Users\Dennis\Desktop\GW DevBuild\GameWhizzards_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using UnityEngine;

public class Inventory : MonoBehaviour
{
  public ArrayList items = new ArrayList();
  public ArrayList to_remove = new ArrayList();
  private int margin_top = 50;
  private int margin_left_side = 20;
  private int margin_right_side = 70;
  private int margin_bottom = 30;
  private int vertical_spacing = 20;
  private int inventory_width = 320;
  private int item_width = 50;
  private int item_height = 50;
  private int item_padding = 10;
  private int label_height = 40;
  private int item_columns = 3;
  public Color textColor = Color.white;
  public static Inventory inv;
  public Texture2D inventory_area_background;
  public Texture2D up_button_texture;
  public Texture2D down_button_texture;
  public Font label_font;
  private float vSliderValue;
  private int starting_row;
  private bool overflow_bottom;
  private bool overflow_top;
  private GameObject dragged;
  private GUIStyle item_label_style;

  private void Start()
  {
    Inventory.inv = this;
    this.item_label_style = new GUIStyle();
    this.item_label_style.normal.textColor = this.textColor;
    this.item_label_style.alignment = TextAnchor.MiddleCenter;
    this.item_label_style.fontSize = 12;
    this.item_label_style.wordWrap = true;
    this.item_label_style.font = this.label_font;
  }

  private void Update()
  {
    this.notifyActive();
  }

  public void SetDragged(GameObject item)
  {
    this.dragged = item;
  }

  public bool MouseOverInventory()
  {
    return new Rect((float) (Screen.width - this.inventory_width - 50), 50f, (float) this.inventory_width, (float) (Screen.height - 100)).Contains(Input.mousePosition);
  }

  private void OnGUI()
  {
    GUI.BeginGroup(new Rect((float) (Screen.width - this.inventory_width - 50), 50f, (float) this.inventory_width, (float) (Screen.height - 100)));
    GUI.DrawTexture(new Rect(0.0f, 0.0f, (float) this.inventory_width, (float) (Screen.height - 100)), (Texture) this.inventory_area_background);
    this.displayItems();
    this.displayButtons();
    GUI.EndGroup();
    this.displayDragged();
  }

  private void displayDragged()
  {
    if ((UnityEngine.Object) this.dragged == (UnityEngine.Object) null)
      return;
    float x = Input.mousePosition.x - (float) (this.item_width / 2);
    float y = (float) ((double) (Screen.width / 2) - (double) Input.mousePosition.y + 150.0) - (float) (this.item_height / 2);
    GUI.DrawTexture(new Rect(x, y, (float) this.item_width, (float) this.item_height), (Texture) (this.dragged.GetComponent(typeof (Item)) as Item).getTexture());
    GUI.Label(new Rect(x, y + (float) this.item_height, (float) this.item_width, (float) this.label_height), (this.dragged.GetComponent(typeof (Item)) as Item).getName(), this.item_label_style);
  }

  private void displayItems()
  {
    int num1 = 0;
    int num2 = 0;
    this.overflow_bottom = false;
    this.overflow_top = false;
    IEnumerator enumerator1 = this.to_remove.GetEnumerator();
    try
    {
      while (enumerator1.MoveNext())
        this.items.Remove((object) (GameObject) enumerator1.Current);
    }
    finally
    {
     
    }
    this.to_remove.Clear();
    IEnumerator enumerator2 = this.items.GetEnumerator();
    try
    {
      while (enumerator2.MoveNext())
      {
        GameObject current = (GameObject) enumerator2.Current;
        float num3 = (1f * (float) this.inventory_width - (float) this.margin_right_side - (float) this.margin_left_side - (float) (this.item_columns * this.item_width)) / (float) this.item_columns;
        float x1 = (float) (this.margin_right_side - 50) + (float) num1 * ((float) this.item_width + num3);
        float y1 = (float) (this.margin_top + num2 * (this.item_height + this.vertical_spacing + this.label_height) - this.starting_row * (this.item_height + this.vertical_spacing));
        if ((double) y1 + (double) this.item_height + (double) this.label_height < (double) (Screen.height - this.margin_bottom))
        {
          if ((double) y1 >= (double) this.margin_top)
          {
            GUIStyle style = new GUIStyle();
            style.normal.background = (current.GetComponent(typeof (Item)) as Item).getTexture();
            if (!(current.GetComponent(typeof (Item)) as Item).GetHidden())
            {
              bool flag1 = GUI.RepeatButton(new Rect(x1, y1, (float) this.item_width, (float) this.item_height), string.Empty, style);
              bool flag2 = false;
              float y2 = Input.mousePosition.y;
              float x2 = Input.mousePosition.x;
              if ((double) x2 > (double) (Screen.width - this.inventory_width) + (double) x1 - 50.0 && (double) x2 < (double) (Screen.width - this.inventory_width) + (double) x1 - 50.0 + (double) this.item_width && ((double) Screen.height - (double) y2 > (double) y1 + 50.0 && (double) Screen.height - (double) y2 < (double) y1 + (double) this.item_height + 50.0))
                flag2 = true;
              this.item_label_style.normal.textColor = current.GetComponent<Item>().textColor;
              GUI.Label(new Rect(x1, y1 + (float) this.item_height, (float) this.item_width, (float) this.label_height), (current.GetComponent(typeof (Item)) as Item).getName(), this.item_label_style);
              if (flag1)
                this.itemClicked(current);
              if (flag2)
                GUI.Box(new Rect(x1 + (float) this.item_width, y1 + (float) this.item_height, 200f, 50f), "Sword\nDmg +15\nA shortsword");
            }
          }
          else
            this.overflow_top = true;
        }
        else
          this.overflow_bottom = true;
        num1 = (num1 + 1) % this.item_columns;
        if (num1 == 0)
          ++num2;
      }
    }
    finally
    {
    
    }
  }

  private void displayButtons()
  {
    int num1 = 35;
    int num2 = 35;
    int num3 = 3;
    if (this.overflow_top)
    {
      if (GUI.Button(new Rect((float) (this.inventory_width / 2 - num1 / 2), (float) num3, (float) num1, (float) num2), string.Empty, new GUIStyle()
      {
        normal = {
          background = this.up_button_texture
        }
      }) && this.starting_row > 0)
        --this.starting_row;
    }
    if (!this.overflow_bottom)
      return;
    if (!GUI.Button(new Rect((float) (this.inventory_width / 2 - num1 / 2), (float) (Screen.height - num2 - num3), (float) num1, (float) num2), string.Empty, new GUIStyle()
    {
      normal = {
        background = this.down_button_texture
      }
    }))
      return;
    ++this.starting_row;
  }

  public void addItem(GameObject item)
  {
    this.items.Add((object) item);
  }

  public void removeItem(GameObject item)
  {
    this.to_remove.Add((object) item);
  }

  public void disactivate(GameObject item)
  {
    (item.GetComponent(typeof (Item)) as Item).SetActive(false);
  }

  public void itemClicked(GameObject item)
  {
    if ((item.GetComponent(typeof (Item)) as Item).GetActive())
      return;
    (item.GetComponent(typeof (Item)) as Item).ClickedInInventory(item);
    (item.GetComponent(typeof (Item)) as Item).SetActive(true);
  }

  private void notifyActive()
  {
    IEnumerator enumerator = this.items.GetEnumerator();
    try
    {
      while (enumerator.MoveNext())
      {
        GameObject current = (GameObject) enumerator.Current;
        if ((current.GetComponent(typeof (Item)) as Item).GetActive())
          (current.GetComponent(typeof (Item)) as Item).ActiveInInventory();
      }
    }
    finally
    {
      
    }
  }
}
