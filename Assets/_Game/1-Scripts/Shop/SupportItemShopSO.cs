using System;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SupportItemShopSO", menuName = "SO/SupportItemShopSO")]
public class SupportItemShopSO : ScriptableObject
{
    public List<SupportItemShopModel> items = new List<SupportItemShopModel>()
    {
        new SupportItemShopModel(TypeResource.Bom3x3, null, 200 ),
        new SupportItemShopModel(TypeResource.Rocket, null, 350 ),
        new SupportItemShopModel(TypeResource.Fire, null, 700 ),
        new SupportItemShopModel(TypeResource.Thunder, null, 1200 )
    };
}
[Serializable]
public class SupportItemShopModel
{
    public TypeResource itemType;
    public Sprite icon;
    public int price;

    public SupportItemShopModel(TypeResource itemType, Sprite icon, int price)
    {
        this.itemType = itemType;
        this.icon = icon;
        this.price = price;
    }
}