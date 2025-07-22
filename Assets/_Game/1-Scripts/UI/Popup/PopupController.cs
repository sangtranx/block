using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum PopupType
{
    Settings = 0,
    Shop = 1,
    Spin = 2,
    Upgrade = 3,
    Achivement = 4,
    BoxTips = 5,
    LeaderBoard = 6,
    X2Speed = 7,
    Review = 8,
    RewardCoin = 9,
    Tutorial = 10,
    /// <summary>
    /// Hiện khi thắng màn
    /// </summary>
    LevelUp,
    /// <summary>
    /// Hiện khi thua màn
    /// </summary>
    Defeat,
    /// <summary>
    /// Cửa hàng item support
    /// </summary>
    SupportItemShop,
    BuyRefillAction
}

public class PopupController : MonoBehaviour
{
    [SerializeField] private PopupBase[] arrPopupBase;
    public static PopupReview popupReview;
    private PopupBase GetPopupByType(PopupType popupType, bool isActice = true)
    {
        var popup = Array.Find(arrPopupBase, popup => popup.PopupType == popupType);
        if (popup != null)
        {
            popup.gameObject.SetActive(isActice);
        }
        return popup;
    }

    private IEnumerator Start()
    {
        GameEvent.onShowPopup += OnShowPopup;
        yield return null;
        InitPosPopup();
        popupReview = GetPopupByType(PopupType.Review) as PopupReview;
    }

    private void OnDestroy()
    {
        GameEvent.onShowPopup -= OnShowPopup;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            var popupReplay = new PopupModel(PopupType.Shop);
            GameEvent.onShowPopup?.Invoke(popupReplay);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            var popupReplay = new PopupModel(PopupType.Spin);
            GameEvent.onShowPopup?.Invoke(popupReplay);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            GameManager.Instance.Defeat();
        }
    }

    private void InitPosPopup()
    {
        for (int i = 0; i < arrPopupBase.Length; i++)
        {
            var popup = arrPopupBase[i];
            popup.InitPopup();
        }
    }

    private void OnShowPopup(PopupModel popupModel)
    {
        var popup = GetPopupByType(popupModel.popupType);
        if (popup == null)
        {
            Debug.LogError($"Don't find popup by type: {popupModel.popupType}");
            return;
        }
        popup.ShowPopup(popupModel);
    }
}

public class PopupModel
{
    public PopupType popupType;
    //public int coin;
    public TypeResource? supportItemType;
    public UnityAction onCompleteShow;


    public PopupModel(PopupType popupType, UnityAction onCompleteShow = null)
    {
        this.popupType = popupType;
        this.onCompleteShow = onCompleteShow;
    }
}