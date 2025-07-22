using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using Game.Ultis;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupDefeat : PopupBase
{
    private DefeatController crtl;

    public DefeatController Ctrl
    {
        get
        {
            if (crtl == null)
            {
                crtl = GetComponent<DefeatController>();
            }

            return crtl;
        }
    }

    [SerializeField] private Button btnHome;
    [SerializeField] private Button btnReplay;
    [SerializeField] Image chestBackground;
    [SerializeField] Image chestFill;
    [SerializeField] Image chestOpen;

    [SerializeField] TextMeshProUGUI txChestProgress;

    //===============================================
    /// <summary>
    /// Khi full % chest, sẽ show số vàng nhận được
    /// </summary>
    [SerializeField] GameObject ChestRewardShow;

    [SerializeField] TextMeshProUGUI txGoldReward;
    [SerializeField] Button btnOkReward;

    [SerializeField] Button btnOkRewardAdsX2;

    //================================================
    private UserData userData => DBController.Instance.USER_DATA;

    public override void InitPopup()
    {
        base.InitPopup();
        InitBtn();
    }
    

    private void InitBtn()
    {
        btnHome.onClick.RemoveAllListeners();
        btnHome.onClick.AddListener(() => Home());

        btnReplay.onClick.RemoveAllListeners();
        btnReplay.onClick.AddListener(() => Replay());

        btnOkReward.onClick.RemoveAllListeners();
        btnOkReward.onClick.AddListener(() =>
        {
            Ctrl.GetDefeatReward();
            // ChestRewardShow.SetActive(false);
            LoadingSceneController.Instance.ChangeScene((SceneType.GamePlay));
        });

        btnOkRewardAdsX2.onClick.RemoveAllListeners();
        btnOkRewardAdsX2.onClick.AddListener(() =>
        {
            Ctrl.GetDefeatRewardX2Ads();
            // ChestRewardShow.SetActive(false);
            LoadingSceneController.Instance.ChangeScene((SceneType.GamePlay));
        });
    }

    public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
    {
        btnHome.gameObject.SetActive(false);
        btnReplay.gameObject.SetActive(false);
        chestBackground.gameObject.SetActive(true);
        chestFill.gameObject.SetActive(true);
        chestOpen.gameObject.SetActive(false);
        ChestRewardShow.SetActive(false);
        base.ShowPopup(popupModel, onShowComplete);
        btnFadeClose.interactable = false;
        //Vì mỗi lần thua +chestPercent% nên trừ chestPercent% để lerp từ trước đó đến hiện tại
        chestFill.fillAmount = (userData.GetResourceByType(TypeResource.DefeatChest).amount - Ctrl.chestPercent) / 100f;
    }

    /// <summary>
    /// Tiến độ rương 
    /// </summary>
    public async void ShowResult()
    {
        float targetFillAmount = userData.GetResourceByType(TypeResource.DefeatChest).amount / 100f;
        string textValue = $"{userData.GetResourceByType(TypeResource.DefeatChest).amount}%";
        await chestFill.DOFillAmount(targetFillAmount, 2f)
            .SetEase(Ease.Linear)
            .OnPlay(() =>
            {
                GameEvent.onResourceGonnaChange?.Invoke(userData.GetResourceByType(TypeResource.DefeatChest).amount -
                                                        Ctrl.chestPercent);
                GameHelper.Instance.ValueDisplayUI.LerpValue(
                    userData.GetResourceByType(TypeResource.DefeatChest).amount, txChestProgress, string.Empty, "%", 2);
            }).ToUniTask();
        await Animation();
    }

    void Home()
    {
        // HidePopup();
        LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
    }

    void Replay()
    {
        // HidePopup();
        LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
    }

    private async UniTask Animation()
    {
        var deltaTime = TimeSpan.FromSeconds(0.1f);
        if (Ctrl.canRewardChest)
        {
            Debug.Log("Animation Play");
            // GameEvent.onResourceGonnaChange?.Invoke(10000);
            GameHelper.Instance.ValueDisplayUI.LerpValue(Ctrl.goldRewardFromChest, txGoldReward, "+");
            chestBackground.gameObject.SetActive(false);
            chestFill.gameObject.SetActive(false);
            chestOpen.gameObject.SetActive(true);
            ChestRewardShow.SetActive(true);
            Ctrl.canRewardChest = false;
            txGoldReward.gameObject.SetActive(false);
            await UniTask.Delay(deltaTime);
        }

        Debug.Log("Enable Button");
        btnFadeClose.interactable = true;
        // btnHome.gameObject.SetActive(true);
        btnReplay.gameObject.SetActive(true);
        txGoldReward.gameObject.SetActive(true);
    }

    protected override void InitCloseFade()
    {
        btnFadeClose.onClick.RemoveAllListeners();
        btnFadeClose.onClick.AddListener(() => { Debug.Log("Replay"); });
    }
}