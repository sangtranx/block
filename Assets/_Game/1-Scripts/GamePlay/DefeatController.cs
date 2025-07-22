using Cysharp.Threading.Tasks;
using Data;
using Game.Ultis;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatController : MonoBehaviour
{
    private PopupDefeat popDefeat;
    public PopupDefeat PopDefeat
    {
        get
        {
            if (popDefeat == null)
            {
                popDefeat = GetComponent<PopupDefeat>();
            }
            return popDefeat;
        }
    }
    /// <summary>
    /// Tiến độ rương tăng lên mỗi lần thua
    /// </summary>
    public readonly int chestPercent = 20;
    public bool canRewardChest;
    public long goldRewardFromChest;
    private UserData userData => DBController.Instance.USER_DATA;
    private void Start()
    {
        CheckChestProgress();
        GameEvent.onDefeat += Defeat;
    }
    private void OnDestroy()
    {
        GameEvent.onDefeat -= Defeat;
    }
    public void Defeat()
    {
        userData.AddResourceByType(TypeResource.DefeatChest, chestPercent); //cộng chestPercent% tiến độ mỗi lần thua
        if (userData.GetResourceByType(TypeResource.DefeatChest).amount >= 100)
        {
            //Nhận quà
            canRewardChest = true;
            goldRewardFromChest = TheGoldCanReward();
            //CheckChestProgress();
        }
        var popupReplay = new PopupModel(PopupType.Defeat);
        GameEvent.onShowPopup?.Invoke(popupReplay);
    }
    public void GetDefeatReward()
    {
        AudioController.Instance.Play(AudioName.Sound_AddCoin);
        userData.AddResourceByType(TypeResource.Coin, goldRewardFromChest);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);
        GameHelper.Instance.GamePlayUI.FlyAnimationByType(TypeResource.Coin, (int)goldRewardFromChest, transform.position).Forget();
    }
    public void GetDefeatRewardX2Ads()
    {
        //ADS
        userData.AddResourceByType(TypeResource.Coin, goldRewardFromChest*2);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);
    }
    public void CheckChestProgress()
    {
        if (userData.GetResourceByType(TypeResource.DefeatChest).amount >= 100)
        {
            userData.SubResourceByType(TypeResource.DefeatChest, userData.GetResourceByType(TypeResource.DefeatChest).amount);
        }
    }
    //List<(long /*gold*/, short/*lucky*/)> goldRewards = new List<(long, short)>()
    //{
    //    {(200, 4000) /*40%*/ },
    //    {(500, 3000 )},
    //    {(1000, 1000) },
    //    {(1500, 800 )},
    //    {(2000, 500 )},
    //    {(3000, 350 )},
    //    {(4000, 250 )},
    //    {(5000, 100 )},
    //};
    long TheGoldCanReward()
    {
        long gold = UnityEngine.Random.Range(20, 51);
        return gold*100;
    }
}
