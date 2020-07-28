// Decompiled with JetBrains decompiler
// Type: DropMe
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0EBE16EA-48A7-422E-A301-FC236929F611
// Assembly location: C:\Users\Dennis\Desktop\GW DevBuild\GameWhizzards_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropMe : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
  public Color highlightColor = Color.yellow;
  public Image containerImage;
  public Image receivingImage;
  private Color normalColor;

  public void OnEnable()
  {
    if (!((Object) this.containerImage != (Object) null))
      return;
    this.normalColor = this.containerImage.color;
  }

  public void OnDrop(PointerEventData data)
  {
    this.containerImage.color = this.normalColor;
    if ((Object) this.receivingImage == (Object) null)
      return;
    UnityEngine.Sprite dropSprite = this.GetDropSprite(data);
    if (!((Object) dropSprite != (Object) null))
      return;
    this.receivingImage.overrideSprite = dropSprite;
  }

  public void OnPointerEnter(PointerEventData data)
  {
    if ((Object) this.containerImage == (Object) null || !((Object) this.GetDropSprite(data) != (Object) null))
      return;
    this.containerImage.color = this.highlightColor;
  }

  public void OnPointerExit(PointerEventData data)
  {
    if ((Object) this.containerImage == (Object) null)
      return;
    this.containerImage.color = this.normalColor;
  }

  private UnityEngine.Sprite GetDropSprite(PointerEventData data)
  {
    GameObject pointerDrag = data.pointerDrag;
    if ((Object) pointerDrag == (Object) null)
      return (UnityEngine.Sprite) null;
    if ((Object) pointerDrag.GetComponent<DragMe>() == (Object) null)
      return (UnityEngine.Sprite) null;
    Image component = pointerDrag.GetComponent<Image>();
    if ((Object) component == (Object) null)
      return (UnityEngine.Sprite) null;
    return component.sprite;
  }
}
