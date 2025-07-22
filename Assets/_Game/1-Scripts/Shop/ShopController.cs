using Data;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SubShopController
{
    public ShopCategory shopCategory;
    public List<ItemShop> shopItems = new List<ItemShop>();
    public ItemShop GetItemShopByTypeRegion(SkinType st) => shopItems.Find(ex => ex.SkinType == st);
    public ItemShop currentItem;
    private ShopDB shopDB => DBController.Instance.SHOP_DB;
    private UserData userData => DBController.Instance.USER_DATA;
    public void InitShop()
    {
        var shopModel = shopDB.GetShopModelByType(shopCategory);
        var current = shopModel.currentRegion;
        currentItem = GetItemShopByTypeRegion(current);
        currentItem.OnSelectedItem();
        CheckInterableAll();
        InitBtn();
    }
    public void InitBtn()
    {
        for (int i = 0; i < shopItems.Count; i++)
        {
            var current = shopItems[i];
            current.AddListenerBtn(() => OnClickBtn(current));
        }
    }
    public void OnClickBtn(ItemShop itemShop)
    {
        Debug.Log("ssss");
        switch (itemShop.State)
        {
            case TypeItemState.Lock:
                OnClickBuy(itemShop);
                break;
            case TypeItemState.Unlock:
                OnClickSelect(itemShop);
                break;
            case TypeItemState.Selected:
                break;
        }
        shopDB.SaveDB();
    }
    public void OnClickBuy(ItemShop itemShop)
    {
        var coin = userData.GetResourceByType(TypeResource.Coin).amount;
        if (coin >= itemShop.Price)
        {
            userData.SubResourceByType(TypeResource.Coin, itemShop.Price);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);
            var shopModel = shopDB.GetShopModelByType(shopCategory);
            shopModel.SetUnlockById(itemShop.SkinType);
            currentItem.OnUnSelect();
            currentItem = itemShop;
            currentItem.OnSelectedItem();
            ShopController.onCheckInterablAll?.Invoke();
            GameEvent.onChangeSkin?.Invoke(itemShop.ShopCategory, itemShop.SkinType);
        }
    }
    public void CheckInterableAll()
    {
        var coin = userData.GetResourceByType(TypeResource.Coin).amount;
        for (int i = 0; i < shopItems.Count; i++)
        {
            var itemShop = shopItems[i];
            if (itemShop.State == TypeItemState.Lock)
            {
                var interable = coin >= itemShop.Price;
                itemShop.SetInterable(interable);
            }
        }
    }

    public void OnClickSelect(ItemShop itemShop)
    {
        var shopModel = shopDB.GetShopModelByType(shopCategory);
        shopModel.SetUnlockById(itemShop.SkinType);
        currentItem.OnUnSelect();
        currentItem = itemShop;
        currentItem.OnSelectedItem();
        GameEvent.onChangeSkin?.Invoke(itemShop.ShopCategory, itemShop.SkinType);
    }

    public void DisableEvent()
    {
        ShopController.onCheckInterablAll -= CheckInterableAll;
    }
}
public class ShopController : MonoBehaviour
{
    private ShopView shopView;
    public ShopView ShopView
    {
        get
        {
            if (shopView == null)
            {
                shopView = GetComponent<ShopView>();
            }
            return shopView;
        }
    }
    private ShopDB shopDB => DBController.Instance.SHOP_DB;
    private List<SubShopController> lstTypeSkinController = new List<SubShopController>();
    public static Action onCheckInterablAll;
    private void Start()
    {
        SpawnItem();
        InitTypeSkinController();
        onCheckInterablAll += CheckAllInteracle;
        
    }
    private void OnDestroy()
    {
        onCheckInterablAll = null;
    }
    private void SpawnItem()
    {
        
        for (int i = 0; i < ShopView.ArrShopSO.Length; i++)
        {
            var shopCategory = ShopView.ArrShopSO[i].shopCategory;
            var subShop = Instantiate(ShopView.SubShopPrefab, ShopView.RtfmParrent);
            subShop.TxtName.text = EnumName.ShopCategoryName(shopCategory);
            var shopSO = ShopView.GetShopSOByType(shopCategory);
            var shopModel = shopDB.GetShopModelByType((shopCategory));
            var subShopController = new SubShopController();
            subShopController.shopCategory = shopCategory;
            lstTypeSkinController.Add(subShopController);
            for (int j = 0; j < shopSO.items.Count; j++)
            {
                var currentItemShopSO = shopSO.items[j];
                var itemShop = Instantiate(ShopView.ItemShopPrefab, subShop.RtfmParrent);
                var itemShopModel = shopModel.GetItemShopDB(currentItemShopSO.skinType);
                var itemShopSO = shopSO.GetItem(currentItemShopSO.skinType);
                itemShop.InitItemShop(shopCategory, itemShopSO, itemShopModel);
                if (subShopController.shopCategory == shopCategory)
                {
                    subShopController.shopItems.Add(itemShop);
                }
            }
        }
    }

    private void InitTypeSkinController()
    {
        for (int i = 0; i < lstTypeSkinController.Count; i++)
        {
            var current = lstTypeSkinController[i];
            current.InitShop();
        }
    }
    public void CheckAllInteracle()
    {
        for (int i = 0; i < lstTypeSkinController.Count; i++)
        {
            var current = lstTypeSkinController[i];
            current.CheckInterableAll();
        }
    }
}
