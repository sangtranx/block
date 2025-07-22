using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupLeaderBoard : PopupBase
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
}
