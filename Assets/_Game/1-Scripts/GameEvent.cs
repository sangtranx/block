using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvent
{
    public static Action<PopupModel> onShowPopup;
    public static Action onCompleteLoginInGame;
    public static Action<TypeVolume, float> onChangeVolume;
    public static Action<TypeResource, long> onUpdateUIByType;
    public static Action<TypeResource, long, long> onLerpUpdateUIByType;
    public static Action<TypeResource> onUpdateResourceUI;
    /// <summary>
    /// To save old value for lerp
    /// </summary>
    public static Action<long> onResourceGonnaChange;
    public static Action<long> onBonusGoldGonnaChange;

    public static Action<int> onAddCoinToBoxTips;
    public static Action onUpdateResource;
    public static Action<float, float> onWatchAdsSpeed;
    public static Action<ShopCategory, SkinType> onChangeSkin;


    //Game
    public static Action onPointAdded;
    public static Action<long /*point*/> onLevelUp;
    public static Action onDefeat;

    public static Action<int> onShapeAutoMoveBoard;
}
