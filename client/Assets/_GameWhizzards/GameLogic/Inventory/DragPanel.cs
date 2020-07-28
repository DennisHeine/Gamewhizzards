// Decompiled with JetBrains decompiler
// Type: DragPanel
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 0EBE16EA-48A7-422E-A301-FC236929F611
// Assembly location: C:\Users\Dennis\Desktop\GW DevBuild\GameWhizzards_Data\Managed\Assembly-CSharp.dll

using UnityEngine;
using UnityEngine.EventSystems;

public class DragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
{
  private Vector2 originalLocalPointerPosition;
  private Vector3 originalPanelLocalPosition;
  private RectTransform panelRectTransform;
  private RectTransform parentRectTransform;

  private void Awake()
  {
    this.panelRectTransform = this.transform.parent as RectTransform;
    this.parentRectTransform = this.panelRectTransform.parent as RectTransform;
  }

  public void OnPointerDown(PointerEventData data)
  {
    this.originalPanelLocalPosition = this.panelRectTransform.localPosition;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, data.position, data.pressEventCamera, out this.originalLocalPointerPosition);
  }

  public void OnDrag(PointerEventData data)
  {
    if ((Object) this.panelRectTransform == (Object) null || (Object) this.parentRectTransform == (Object) null)
      return;
    Vector2 localPoint;
    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.parentRectTransform, data.position, data.pressEventCamera, out localPoint))
      this.panelRectTransform.localPosition = this.originalPanelLocalPosition + (Vector3) (localPoint - this.originalLocalPointerPosition);
    this.ClampToWindow();
  }

  private void ClampToWindow()
  {
    Vector3 localPosition = this.panelRectTransform.localPosition;
    Vector3 vector3_1 = (Vector3) (this.parentRectTransform.rect.min - this.panelRectTransform.rect.min);
    Vector3 vector3_2 = (Vector3) (this.parentRectTransform.rect.max - this.panelRectTransform.rect.max);
    localPosition.x = Mathf.Clamp(this.panelRectTransform.localPosition.x, vector3_1.x, vector3_2.x);
    localPosition.y = Mathf.Clamp(this.panelRectTransform.localPosition.y, vector3_1.y, vector3_2.y);
    this.panelRectTransform.localPosition = localPosition;
  }
}
