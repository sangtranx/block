using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftApplyActionFactory : BaseFactory<TypeGift, Action<int>>
{
    public override void Initialize()
    {
        stateByType.Add(TypeGift.Coin, (amount) =>
        { 
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Coin, amount);
            GameEvent.onResourceGonnaChange?.Invoke(amount);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Coin).amount); 
        });
        stateByType.Add(TypeGift.Bom3x3, (amount) =>
        {
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Bom3x3, amount);
            GameEvent.onResourceGonnaChange?.Invoke(amount);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Bom3x3, DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Bom3x3).amount);
        });
        stateByType.Add(TypeGift.Rocket, (amount) =>
        {
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Rocket, amount);
            GameEvent.onResourceGonnaChange?.Invoke(amount);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Rocket ,DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Rocket).amount);

        });
        stateByType.Add(TypeGift.Fire, (amount) =>
        {
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Fire, amount);
            GameEvent.onResourceGonnaChange?.Invoke(amount);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Fire, DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Fire).amount);
        });
        stateByType.Add(TypeGift.Thunder, (amount) =>
        {
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Thunder, amount);
            GameEvent.onResourceGonnaChange?.Invoke(amount);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Thunder, DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Thunder).amount);
        });
    }
}
