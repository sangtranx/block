using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ShopSO", menuName = "SO/ShopSO")]
public class ShopSO : ScriptableObject
{
    public ShopCategory shopCategory;
    public List<ItemShopSO> items;
    public ItemShopSO GetItem(SkinType skinType)
    {
        return items.Find(i => i.skinType == skinType);
    }
}
/// <summary>
/// Loại skin
/// </summary>
public enum ShopCategory
{
    NONE = 0,
    BLOCK,
    BACKGROUND,
    /// <summary>
    /// Board, Button, Exp
    /// </summary>
    GAMEPLAY,
    TOTAL
}
public enum SkinType
{
    CLASSIC = 0,
    CLOWN = 1,
    DRAGON, 
    SEA,
    KPOP,
    TOTAL
}
[Serializable]
public class ItemShopSO
{
    public string name;
    /// <summary>
    /// id của skin
    /// </summary>
    public SkinType skinType;
    public int price;
    public Sprite sprItem;
}