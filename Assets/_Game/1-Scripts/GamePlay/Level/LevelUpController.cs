using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PopLevelUp))]
public class LevelUpController : MonoBehaviour
{
    private PopLevelUp popLevelUp;
    public PopLevelUp PopLevelUp
    {
        get
        {
            if(popLevelUp == null)
            {
                popLevelUp = GetComponent<PopLevelUp>();
            }
            return popLevelUp;
        }
    }
    long bonusGold;

    private UserData userData => DBController.Instance.USER_DATA;
    private void Start()
    {
        GameEvent.onLevelUp += OnLevelUp;
    }
    private void OnDestroy()
    {
        GameEvent.onLevelUp -= OnLevelUp;
    }
    public void OnLevelUp(long bonusGold)
    {
        userData.SubResourceByType(TypeResource.Point, userData.GetResourceByType(TypeResource.Point).amount); //Reset point
        GameEvent.onUpdateUIByType(TypeResource.Point, userData.GetResourceByType(TypeResource.Point).amount);
        userData.AddResourceByType(TypeResource.Level, 1); //level ++
        GameEvent.onUpdateUIByType(TypeResource.Level, userData.GetResourceByType(TypeResource.Level).amount);
        this.bonusGold = bonusGold;
        var popupReplay = new PopupModel(PopupType.LevelUp);
        GameEvent.onShowPopup?.Invoke(popupReplay);
    }
    public void ShowResult()
    {
        PopLevelUp.SetBonusGold(bonusGold);
        userData.AddResourceByType(TypeResource.Coin, (int)bonusGold);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);
    }
}
