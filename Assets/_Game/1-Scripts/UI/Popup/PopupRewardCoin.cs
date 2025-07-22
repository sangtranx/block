using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using Game.Helpers;

public class PopupRewardCoin : PopupBase
{
    [SerializeField] int coinAmount;
    [SerializeField] private Button btnClose;
    [SerializeField] private TimerButton btnYes;
    [SerializeField] private TextMeshProUGUI txtCoin;

    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnYes();
        InitBtnClose();
        txtCoin.text = $"Would you like to watch a video to get free {coinAmount} coins";
    }

    private void InitBtnClose()
    {
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }
    private void InitBtnYes()
    {
        // btnYes.onClick.RemoveAllListeners();
        // btnYes.onClick.AddListener(() =>
        // {
        //     AdmobController.Instance.ShowRewardAd(() =>
        //     {
        //         DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Coin, coinAmount);
        //         GameEvent.onResourceGonnaChange(DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Coin).amount);
        //         GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Coin).amount);
        //         GameHelper.Instance.GamePlayUI.FlyAnimationByType(TypeResource.Coin, coinAmount, transform.position).Forget();
        //         btnYes.SaveTicks();
        //     },
        //     () =>
        //     {
        //         btnYes.ReloadCurrentState();
        //         HidePopup();
        //     });
        // });
    }
}
