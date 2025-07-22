using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SpriteShopSO", menuName = "SO/SpriteShopSO")]
public class SpriteShopSO : ScriptableObject
{
    public List<SpriteShop> lstCharacterShop;

    public SpriteShop GetSpriteShopByTypeCharacter(ShopCategory shopCate) => lstCharacterShop.Find(ex => ex.shopCategory == shopCate);
}
[Serializable]
public class SpriteShop
{
    public ShopCategory shopCategory;
    public List<SpriteSkin> characterSkins;

    public SpriteSkin GetSpriteSkinByTypeRegion(SkinType region) => characterSkins.Find(ex => ex.skinType == region);
}
[Serializable]
public class SpriteSkin
{
    public SkinType skinType;
    public Sprite sprSkin;
    public RuntimeAnimatorController animator;
}
