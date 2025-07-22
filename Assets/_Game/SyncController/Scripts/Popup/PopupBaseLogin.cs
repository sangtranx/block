using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public enum PopupLoginType
{
    Option = 0,
    LoginTadaplay = 1,
    Resgister = 2,
    Warning = 3,
    Result = 4,
    ForgotPassword = 5,
    ChangePassword = 6,
    Wait = 7,
}

public class PopupBaseLogin : MonoBehaviour
{
    [SerializeField] private PopupLoginType popupLogionType;
    [SerializeField] private Image imgFade;
    [SerializeField] private RectTransform rtfmPopup;
    [SerializeField] private Button btnFadeClose;
    public Action externalShowComplete;
    public Action externalShow;
    public Action externalHideComplete;
    public Action externalHide;
    protected PopupLoginModel popupLoginModel;
    private float timer = 0.5f;
    public PopupLoginType PopupType { get => popupLogionType; }

    public virtual void InitPopup()
    {
        imgFade.color = new Color(0, 0, 0, 0.8f);
        rtfmPopup.anchoredPosition = new Vector2(0, Screen.height * 2);
        SetStatusPopup(false);
        InitBtnCloseFade();
    }

    private void InitBtnCloseFade()
    {
        btnFadeClose.onClick.RemoveAllListeners();
        btnFadeClose.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }

    public virtual void ShowPopup(PopupLoginModel popupLoginModel = null, UnityAction onShowComplete = null)
    {
        this.popupLoginModel = popupLoginModel;
        SetStatusPopup(true);
        imgFade.DOFade(0.8f, timer).SetLink(gameObject);
        externalShow?.Invoke();
        rtfmPopup.DOAnchorPosY(0, timer).OnComplete(() =>
        {
            onShowComplete?.Invoke();
            this.externalShowComplete.Invoke();
        }).SetLink(gameObject);
    }

    public virtual void HidePopup(UnityAction onHideComplete = null)
    {
        externalHide?.Invoke();
        imgFade.DOFade(0, timer).SetLink(gameObject);
        rtfmPopup.DOAnchorPosY(Screen.height * 2, timer).OnComplete(() =>
        {
            onHideComplete?.Invoke();
            this.externalHideComplete?.Invoke();
            SetStatusPopup(false);
        }).SetLink(gameObject);
    }

    private void SetStatusPopup(bool status)
    {
        rtfmPopup.gameObject.SetActive(status);
        imgFade.gameObject.SetActive(status);
    }
}
