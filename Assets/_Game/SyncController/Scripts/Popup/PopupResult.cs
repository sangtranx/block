using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupResult : PopupBaseLogin
{
    [SerializeField] private Button btnClose;
    [SerializeField] private ItemTextResult itemTextResult;
    [SerializeField] private RectTransform rtfmParrent;
    private List<ItemTextResult> lstItemTextResult = new List<ItemTextResult>();
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

    public override void ShowPopup(PopupLoginModel popupLoginModel = null, UnityAction onShowComplete = null)
    {
        base.ShowPopup(popupLoginModel, onShowComplete);
        SpawnItemTextResult();
    }

    private void SpawnItemTextResult()
    {
        for (int i = 0; i < popupLoginModel.lstMessage.Count; i++)
        {
            var current = popupLoginModel.lstMessage[i];
            var item = Instantiate(itemTextResult, rtfmParrent);
            item.TxtResulft.text = current;
            lstItemTextResult.Add(item);
        }
    }

    public void DestroyAll()
    {
        for (int i = lstItemTextResult.Count - 1; i >= 0; i--)
        {
            var current = lstItemTextResult[i];
            lstItemTextResult.Remove(current);
            Destroy(current.gameObject);
        }
    }
}
