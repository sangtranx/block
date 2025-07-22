using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Helpers;
using UnityEngine;

public class LuckySpinController : MonoBehaviour
{
    [SerializeField] GiftRewardProbability giftRewardProbability;
    [SerializeField] Spinner spinner;
    [SerializeField] LuckySpinView luckySpinView;
    [SerializeField] float minutesGetFreeStock;
    GiftApplyActionFactory giftApplyActionFactory;

    private void Start()
    {
        giftApplyActionFactory = new GiftApplyActionFactory();
        giftRewardProbability.CalculateAngleGift();
        SpinData.SPIN_AMOUNT = Mathf.Clamp(SpinData.SPIN_AMOUNT + SpinAmountFree(), 0, 2);
        luckySpinView.ReloadView();
        luckySpinView.onCompletedWaitForTimer += OnCompletedWaitForFree;
        if (SpinData.SPIN_AMOUNT != SpinConfig.MAX_STOCK)
        {
            luckySpinView.SetTimer(TimeSpan.FromMinutes(minutesGetFreeStock) -
                                   (DateTime.Now - new DateTime(SpinData.LAST_TICK_SAVED_2_GET_FREE)));
        }
    }

    int SpinAmountFree()
    {
        DateTime lastSavedTime = new DateTime(SpinData.LAST_TICK_SAVED_2_GET_FREE);
        var deltaTime = DateTime.Now - lastSavedTime;
        //Debug.Log(deltaTime.TotalSeconds + " " + TimeSpan.FromMinutes(minutesGetFreeStock).TotalSeconds);
        return Mathf.FloorToInt(
            ((float)(deltaTime.TotalSeconds / TimeSpan.FromMinutes(minutesGetFreeStock).TotalSeconds)));
    }

    void OnCompletedWaitForFree()
    {
        SpinData.SPIN_AMOUNT = Mathf.Clamp(SpinData.SPIN_AMOUNT + 1, 0, 2);
        if (SpinData.SPIN_AMOUNT != SpinConfig.MAX_STOCK)
        {
            SpinData.LAST_TICK_SAVED_2_GET_FREE = DateTime.Now.Ticks;
            luckySpinView.SetTimer(TimeSpan.FromMinutes(minutesGetFreeStock));
        }

        luckySpinView.ReloadView();
    }

    public void Reset()
    {
        spinner.ResetAngle();
        luckySpinView.ResetView();
    }

    public void StartSpin()
    {
        AudioController.Instance.Play(AudioName.Sound_Spin);
        luckySpinView.OnStartedSpin();
        var giftReceiveData = giftRewardProbability.RandomGiftData();
        spinner.Spin2Angle(giftReceiveData.angle, () =>
        {
            //if (SpinData.SPIN_AMOUNT == SpinConfig.MAX_STOCK)
            //{
            //    luckySpinView.SetTimer(TimeSpan.FromMinutes(minutesGetFreeStock));
            //}

            int unit = giftReceiveData.gift.Amount;
            giftReceiveData.gift.SetAmount(unit);
            luckySpinView.SetGiftReceiveView(giftReceiveData.gift);
            luckySpinView.OnCompletedSpin();
            giftApplyActionFactory.Get(giftReceiveData.gift.TypeGift)?.Invoke(giftReceiveData.gift.Amount);
            AudioController.Instance.Stop(AudioName.Sound_Spin);
            AudioController.Instance.Play(AudioName.Sound_SubCoin);
        });
    }

    public void WatchAds2StartSpin()
    {
        // AdmobController.Instance.ShowRewardAd(() =>
        //     {
        //         AudioController.Instance.Play(AudioName.Sound_Spin);
        //         luckySpinView.OnStartedSpin();
        //         var giftReceiveData = giftRewardProbability.RandomGiftData();
        //         spinner.Spin2Angle(giftReceiveData.angle, () =>
        //         {
        //             int unit = giftReceiveData.gift.Amount;
        //             giftReceiveData.gift.SetAmount(unit);
        //             luckySpinView.SetGiftReceiveView(giftReceiveData.gift);
        //             luckySpinView.ReloadView();
        //             giftApplyActionFactory.Get(giftReceiveData.gift.TypeGift)?.Invoke(giftReceiveData.gift.Amount);
        //             AudioController.Instance.Stop(AudioName.Sound_Spin);
        //         });
        //         luckySpinView.BtnSpinByAds.SaveTicks();
        //     },
        //     () => { luckySpinView.BtnSpinByAds.ReloadCurrentState(); });
    }

    private TypeResource GetTypeResourceByTypeGift(TypeGift typeGift)
    {
        var type = TypeResource.Coin;
        switch (typeGift)
        {
            case TypeGift.Coin:
                type = TypeResource.Coin;
                break;
            case TypeGift.Bom3x3:
                type = TypeResource.Bom3x3;
                break;
            case TypeGift.Rocket:
                type = TypeResource.Rocket;
                break;
            case TypeGift.Fire:
                type = TypeResource.Fire;
                break;
            case TypeGift.Thunder:
                type = TypeResource.Thunder;
                break;
        }

        return type;
    }
}