using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupSpin : PopupBase
{
    [SerializeField] private Button btnClose;

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

    protected override void InitCloseFade()
    {
        // base.InitCloseFade();
    }

    public void HidePopupSpin()
    {
        HidePopup();
    }

    public override void HidePopup(UnityAction onHideComplete = null)
    {
        AudioController.Instance.Stop(AudioName.Sound_Spin);
        base.HidePopup(onHideComplete);
    }
}
