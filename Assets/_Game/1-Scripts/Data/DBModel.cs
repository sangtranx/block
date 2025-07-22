
using Data;
using System;
using System.Collections.Generic;

[Serializable]
public class ShopDB
{
    public List<ShopModel> lstShopModel = new List<ShopModel>();
    public ShopDB(List<ShopSO> lstShopSO)
    {
        for (int i = 0; i < lstShopSO.Count; i++)
        {
            var shopSO = lstShopSO[i];
            var shopModel = new ShopModel(shopSO);
            lstShopModel.Add(shopModel);
        }
    }
    public ShopModel GetShopModelByType(ShopCategory shopCategory) => lstShopModel.Find(ex => ex.shopCategory == shopCategory);

    public bool CheckFull(SkinType skinType)
    {
        for (int i = 0; i < lstShopModel.Count; i++)
        {
            var curentList = lstShopModel[i];
            for (int j = 0; j < curentList.itemsShopDB.Count; j++)
            {
                var curentItem = curentList.itemsShopDB[j];
                if (curentItem.skinType == skinType && !curentItem.isUnlock)
                {
                    return false;
                }
            }
        }
        return true;
    }

    public void SaveDB()
    {
        DBController.Instance.SHOP_DB = this;
    }
}

[Serializable]
public class ShopModel
{
    public ShopCategory shopCategory;
    public SkinType currentRegion;
    public List<ItemShopDB> itemsShopDB = new List<ItemShopDB>();
    public ItemShopDB GetItemShopDB(SkinType skinType) => itemsShopDB.Find(ex => ex.skinType == skinType);

    public ShopModel(ShopSO shopSO)
    {
        this.shopCategory = shopSO.shopCategory;
        currentRegion = SkinType.CLASSIC;
        for (int i = 0; i < shopSO.items.Count; i++)
        {
            var item = shopSO.items[i];
            var itemShopDB = new ItemShopDB();
            itemShopDB.skinType = item.skinType;
            itemShopDB.isUnlock = i == 0 ? true : false;
            itemsShopDB.Add(itemShopDB);
        }
    }

    public void SetUnlockById(SkinType SkinType)
    {
        var item = GetItemShopDB(SkinType);
        item.isUnlock = true;
        SetCurrentId(SkinType);
    }

    public void SetCurrentId(SkinType SkinType)
    {
        currentRegion = SkinType;
    }
}
[Serializable]
public class ItemShopDB
{
    public SkinType skinType;
    public bool isUnlock;
}