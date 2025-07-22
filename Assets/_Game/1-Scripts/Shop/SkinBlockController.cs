using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UI;

public class SkinBlockController : MonoBehaviour
{
    [SerializeField] private SkinBlockSO _skinBlockSo;
    private ShopDB _shopDB => DBController.Instance.SHOP_DB;
    public Sprite GetCurrentSpriteById(int id)
    {
        var currentSkinBlockType = _shopDB.GetShopModelByType((ShopCategory.BLOCK)).currentRegion;
        var skin = _skinBlockSo.GetSkinByType(currentSkinBlockType);
        if (skin == null) return null;
        var sprite = skin.GetSpriteById(id);
        if (sprite == null) return null;
        return sprite.sprBlock;
    }

    public Sprite[] GetCurrentCellBoard()
    {
        var currentSkinBlockType = _shopDB.GetShopModelByType((ShopCategory.BLOCK)).currentRegion;
        var skin = _skinBlockSo.GetSkinByType(currentSkinBlockType);
        if (skin == null) return null;
        var arrSprite = new Sprite[skin.lstCell.Count()];
        for (int i = 0; i < skin.lstCell.Count; i++)
        {
            arrSprite[i] = skin.lstCell[i].sprBlock;
        }
        return arrSprite;
    }
}