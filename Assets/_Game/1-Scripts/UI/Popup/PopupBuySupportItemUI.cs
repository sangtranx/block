using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupBuySupportItemUI : PopupBase
{
    private UserData userData => DBController.Instance.USER_DATA;
    private SupportItemShopSO shop => DBController.Instance.SupportItemSO;
    SupportItemShopModel currentItem;
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnBuy;
    /// <summary>
    /// Thêm số lượng mua
    /// </summary>
    [SerializeField] private Button btnAddBuyQuantity;
    /// <summary>
    /// Giảm số lượng mua
    /// </summary>
    [SerializeField] private Button btnSubBuyQuantity;
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI txItemName;
    [SerializeField] TextMeshProUGUI txBuyQuantity;
    [SerializeField] TextMeshProUGUI txItemPrice;

    [SerializeField] TypeResource itemType;
    [SerializeField] byte buyQuantity; //max 255 per time
    public override void InitPopup()
    {
        base.InitPopup();
        InitBtn();
    }

    private void InitBtn()
    {
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() => HidePopup());

        btnBuy.onClick.RemoveAllListeners();
        btnBuy.onClick.AddListener(() => Buy());

        btnAddBuyQuantity.onClick.RemoveAllListeners();
        btnAddBuyQuantity.onClick.AddListener(() => ChangeBuyQuantity(true));

        btnSubBuyQuantity.onClick.RemoveAllListeners();
        btnSubBuyQuantity.onClick.AddListener(() => ChangeBuyQuantity(false));
    }
    public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
    {
        base.ShowPopup(popupModel, onShowComplete);
        itemType = popupModel.supportItemType.Value;
        SetItemType();
        buyQuantity = 1;
        UpdatePrice();
    }
    public void SetItemType()
    {
        switch (itemType)
        {
            case TypeResource.Bom3x3:
                txItemName.text = "Bom 3x3";
                break;
            case TypeResource.Rocket:
                txItemName.text = "Rocket";
                break;
            case TypeResource.Fire:
                txItemName.text = "Fire";
                break;
            case TypeResource.Thunder:
                txItemName.text = "Thunder";
                break;
            case TypeResource.ReSpawn:
                txItemName.text = "Respawn";
                break;
        }
        currentItem = shop.items.Find(i => i.itemType == itemType);
        itemIcon.sprite = currentItem.icon;
    }
    void ChangeBuyQuantity(bool isAdd)
    {
        if (isAdd && buyQuantity < 10)
        {
            buyQuantity++;
            UpdatePrice();
        }
        else if(!isAdd && buyQuantity > 1)
        {
            buyQuantity--;
            UpdatePrice();
        }
    }
    void UpdatePrice()
    {
        int totalPrice = currentItem.price * buyQuantity;
        txBuyQuantity.text = $"x{buyQuantity}";
        txItemPrice.text = $"{totalPrice.ToString("N0")} <sprite=0>";
        btnBuy.interactable = totalPrice < userData.GetResourceByType(TypeResource.Coin).amount; //không đủ tiền sẽ un button
        btnAddBuyQuantity.interactable = buyQuantity < 10;
        btnSubBuyQuantity.interactable = buyQuantity > 1;
    }
    void Buy()
    {
        int totalPrice = currentItem.price * buyQuantity;
        if (totalPrice > userData.GetResourceByType(TypeResource.Coin).amount) return;

        userData.SubResourceByType(TypeResource.Coin, totalPrice);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);

        userData.AddResourceByType(itemType, buyQuantity);
        GameEvent.onUpdateUIByType?.Invoke(itemType, userData.GetResourceByType(itemType).amount);
        HidePopup();
    }
}

