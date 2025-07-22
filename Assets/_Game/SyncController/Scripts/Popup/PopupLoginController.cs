using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupLoginController : MonoBehaviour
{
    [SerializeField] private PopupBaseLogin[] arrPopupBase;
    public static Action<PopupLoginModel> onShowPopup;
    private PopupBaseLogin GetPopupByType(PopupLoginType popupLoginType, bool isActice = true)
    {
        var popup = Array.Find(arrPopupBase, popup => popup.PopupType == popupLoginType);
        if (popup != null)
        {
            popup.gameObject.SetActive(isActice);
        }
        return popup;
    }

    public IEnumerator Init()
    {
        onShowPopup += OnShowPopup;
        yield return null;
        InitPosPopup();
    }

    private void OnDestroy()
    {
        onShowPopup -= OnShowPopup;
    }

    private void InitPosPopup()
    {
        for (int i = 0; i < arrPopupBase.Length; i++)
        {
            var popup = arrPopupBase[i];
            popup.InitPopup();
        }
    }

    private void ShowPopupLoginOption()
    {
        var popupLoginModel = new PopupLoginModel(PopupLoginType.Option);
        onShowPopup?.Invoke(popupLoginModel);
    }

    private void OnShowPopup(PopupLoginModel popupLoginModel)
    {
        var popup = GetPopupByType(popupLoginModel.popupLoginType);
        if (popup == null)
        {
            Debug.LogError($"Don't find popup by type: {popupLoginModel.popupLoginType}");
            return;
        }
        popup.ShowPopup(popupLoginModel);
    }
}

public class PopupLoginModel
{
    public PopupLoginType popupLoginType;
    public List<string> lstMessage;
    public PopupLoginModel(PopupLoginType popupLoginType)
    {
        this.popupLoginType = popupLoginType;
    }
}
