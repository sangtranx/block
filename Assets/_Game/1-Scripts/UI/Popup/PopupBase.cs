using DG.Tweening;
using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupBase : MonoBehaviour
{
    [SerializeField] protected PopupType popupType;
    [SerializeField] protected Image imgFade;
    [SerializeField] protected RectTransform rtfmPopup;
    [SerializeField] protected Button btnFadeClose;
    [SerializeField] float targetYAnchorPosition;
    public Action externalShowComplete;
    public Action externalShow;
    public Action externalHideComplete;
    public Action externalHide;
    protected PopupModel popupModel;
    private float timer = 0.5f;

    public PopupType PopupType { get => popupType; }

    public virtual void InitPopup()
    {
        imgFade.color = new Color(0, 0, 0, 0.8f);
        rtfmPopup.anchoredPosition = new Vector2(0, Screen.height * 2);
        SetStatusPopup(false);
        InitCloseFade();
        externalShow += () => GameManager.Instance.StateGame = StateGame.Pause;
        externalHideComplete += () => GameManager.Instance.StateGame = StateGame.Play;
    }

    protected virtual void InitCloseFade()
    {
        btnFadeClose.onClick.RemoveAllListeners();
        btnFadeClose.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }

    public virtual void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
    {
        this.popupModel = popupModel;
        SetStatusPopup(true);
        imgFade.DOFade(0.8f, timer).SetLink(gameObject);
        externalShow?.Invoke();
        rtfmPopup.DOAnchorPosY(targetYAnchorPosition, timer).OnComplete(() =>
        {
            onShowComplete?.Invoke();
            this.externalShowComplete?.Invoke();
        }).SetLink(gameObject);
    }

    public virtual void HidePopup(UnityAction onHideComplete = null)
    {
        externalHide?.Invoke();
        imgFade.DOFade(0, timer).SetLink(gameObject);
        rtfmPopup.DOAnchorPosY(Screen.height * 2, timer).OnComplete(() =>
        {
            SetStatusPopup(false);
            onHideComplete?.Invoke();
            this.externalHideComplete?.Invoke();
        }).SetLink(gameObject);
    }

    private void SetStatusPopup(bool status)
    {
        rtfmPopup.gameObject.SetActive(status);
        imgFade.gameObject.SetActive(status);
    }
}
