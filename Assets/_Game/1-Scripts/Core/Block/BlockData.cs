using Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core.Block
{
  public class BlockData : MonoBehaviour
  {
    [SerializeField] private SkinBlockSO _skinBlockSo;
    private ShopDB _shopDB => DBController.Instance.SHOP_DB;

    public SpirteSkinBlock RandomData()
    {
      var currentSkinBlockType = _shopDB.GetShopModelByType((ShopCategory.BLOCK)).currentRegion;
      var skin = _skinBlockSo.GetSkinByType(currentSkinBlockType);
      if (skin == null)
      {
        return null;
      }

      var level = DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Level).amount;
      var id = UnityEngine.Random.Range(0, Mathf.Clamp(6 + (int)(level % 3), 6, 7));
      return skin.lstSprIcon[id];
    }
  }

  [Serializable]
  public class BlockSkin
  {
    public int id;
    public Sprite sprBlock;
  }
}