using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupShop : PopupBase
{
    [SerializeField] private Button btnClose;

    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnClose();
    }

    private void InitBtnClose()    {
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }

    public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
    {
        base.ShowPopup(popupModel, onShowComplete);
    }

    public void AnimationCoinText()
    {

    }
}
