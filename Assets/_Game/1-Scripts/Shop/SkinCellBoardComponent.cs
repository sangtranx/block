using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Game.Core.Board;
public class SkinCellBoardComponent : MonoBehaviour
{
    [SerializeField] private SkinBlockSO _skinBlockSo;
    [SerializeField] private Cell cell;
    private ShopDB _shopDB => DBController.Instance.SHOP_DB;

    private void Start()
    {
        GameEvent.onChangeSkin += OnChangeSkin;
    }
    private void OnChangeSkin(ShopCategory shopCategory, SkinType skinType)
    {
        if (shopCategory != ShopCategory.BLOCK) return;
        var sprite = GetCurrentSpriteById(cell.Id);
        cell.SetCellSprite(sprite);
    }

    private void OnDestroy()
    {
        GameEvent.onChangeSkin -= OnChangeSkin;
    }

    public Sprite GetCurrentSpriteById(int id)
    {
        var currentSkinBlockType = _shopDB.GetShopModelByType((ShopCategory.BLOCK)).currentRegion;
        var skin = _skinBlockSo.GetSkinByType(currentSkinBlockType);
        if (skin == null) return null;
        var sprite = skin.GetSpriteByIdCell(id);
        if (sprite == null) return null;
        return sprite.sprBlock;
    }
}