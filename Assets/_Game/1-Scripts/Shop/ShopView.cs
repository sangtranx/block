using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] private SubShop subShopPrefab;
    [SerializeField] private ItemShop itemShopPrefab;
    [SerializeField] private ShopSO[] arrShopSO;
    [SerializeField] private RectTransform rtfmParrent;
    public SubShop SubShopPrefab { get => subShopPrefab; }
    public ItemShop ItemShopPrefab { get => itemShopPrefab; }
    public RectTransform RtfmParrent { get => rtfmParrent;}
    public ShopSO[] ArrShopSO { get => arrShopSO;}

    public ShopSO GetShopSOByType(ShopCategory shopCategory) => Array.Find(arrShopSO, item => item.shopCategory == shopCategory);
}
