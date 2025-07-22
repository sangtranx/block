using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class SkinGameplayComponent : MonoBehaviour
{
    [SerializeField] private SkinGameplaySO skinSO;  
    private ShopDB shopDB => DBController.Instance.SHOP_DB;

    private void Start()
    {
        GameEvent.onChangeSkin += OnChangeSkin;
        var currentSkinBlockType = shopDB.GetShopModelByType((ShopCategory.GAMEPLAY)).currentRegion;
        OnChangeSkin(ShopCategory.GAMEPLAY, currentSkinBlockType);
    }

    private void OnDestroy()
    {
        GameEvent.onChangeSkin -= OnChangeSkin;
    }

    private void OnChangeSkin(ShopCategory shopCategory, SkinType skinType)
    {
        if (shopCategory != ShopCategory.GAMEPLAY) return;
        var _skin = skinSO.GetSkinByType(skinType);
        if (_skin == null)
        {
            Debug.LogError($"Can't find skin block type: {skinType}");
        }
        GetComponent<Image>().sprite = _skin.sprItem;
    }
}
