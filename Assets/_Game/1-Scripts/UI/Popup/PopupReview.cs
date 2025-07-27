using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class PopupReview : PopupBase
{
  [SerializeField] private Button btnClose;
  [SerializeField] private Image imgMap;
  [SerializeField] TMP_Text txItemName;
  private Sprite sprIcon;
  public Sprite SprIcon { get => sprIcon; set => sprIcon = value; }
  public TMP_Text TxItemName { get => txItemName; set => txItemName = value; }
  public override void InitPopup()
  {
    base.InitPopup();
    InitBtnClose();
  }

  private void InitBtnClose()
  {
    btnClose.onClick.RemoveAllListeners();
    btnClose.onClick.AddListener(() =>
    {
      HidePopup();
    });
  }

  public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
  {
    base.ShowPopup(popupModel, onShowComplete);
    //imgMap.sprite = typeSkin != TypeSkin.Background ? sprIcon : GetSpriteByTypeRegion(TypeRegion);
    imgMap.sprite = sprIcon;
  }
}
