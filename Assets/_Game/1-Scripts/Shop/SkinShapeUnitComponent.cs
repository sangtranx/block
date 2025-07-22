using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Game.Core.Block;

public class SkinShapeUnitComponent : MonoBehaviour
{
    [SerializeField] private SkinBlockSO _skinBlockSo;
    private ShopDB _shopDB => DBController.Instance.SHOP_DB;
    private BlockElement shapeUnit;

    private BlockElement ShapeUnit
    {
        get
        {
            if (shapeUnit == null)
            {
                shapeUnit = GetComponent<BlockElement>();
            }

            return shapeUnit;
        }
    }
    private void Start()
    {
        GameEvent.onChangeSkin += OnChangeSkin;
    }
    private void OnChangeSkin(ShopCategory shopCategory, SkinType skinType)
    {
        if (shopCategory != ShopCategory.BLOCK) return;
        var sprite = GetCurrentSpriteById(ShapeUnit.Id);
        ShapeUnit.ImgBlock.sprite = sprite;
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
        var sprite = skin.GetSpriteById(id);
        if (sprite == null) return null;
        return sprite.sprBlock;
    }
}
