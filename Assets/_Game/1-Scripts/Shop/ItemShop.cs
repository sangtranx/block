using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum TypeItemState
{
    Lock = 0,
    Unlock = 1,
    Selected = 2,
}

public class ItemShop : MonoBehaviour
{
    [SerializeField] private ShopCategory shopCategory;
    [SerializeField] private SkinType skinType;
    [SerializeField] private TypeItemState state;
    [SerializeField] private Image imgIcon;
    [SerializeField] private Button btn;
    [SerializeField] private TextMeshProUGUI txtBtn;
    [SerializeField] private Button btnReview;
    private int price;
    private UserData userData => DBController.Instance.USER_DATA;
    private ShopDB shopDB => DBController.Instance.SHOP_DB;
    public ShopCategory ShopCategory { get => shopCategory; }
    public SkinType SkinType { get => skinType; }
    public TypeItemState State { get => state; }
    public int Price { get => price; }
    private Sprite sprIcon;

    public void InitItemShop(ShopCategory shopCategory, ItemShopSO shop, ItemShopDB itemShopDB)
    {
        imgIcon.sprite = shop.sprItem;
        this.price = shop.price;
        this.shopCategory = shopCategory;
        this.skinType = shop.skinType;
        state = itemShopDB.isUnlock ? TypeItemState.Unlock : TypeItemState.Lock;
        sprIcon = shop.sprItem;
        SetStateBtn(state);
        InitBtnReview();
    }

    private void InitBtnReview()
    {
        btnReview.onClick.RemoveAllListeners();
        btnReview.onClick.AddListener(() =>
        {
            //PopupController.popupReview.TypeRegion = typeRegion;
            //PopupController.popupReview.TypeSkin = TypeSkin;
            PopupController.popupReview.SprIcon = sprIcon;
            PopupController.popupReview.TxItemName.text = SkinType.ToString();
            PopupController.popupReview.ShowPopup();
        });
    }

    public void AddListenerBtn(UnityAction item)
    {
        btn.onClick.RemoveAllListeners();
        btn.onClick.AddListener(item);
    }

    public void SetStateBtn(TypeItemState state)
    {
        this.state = state;
        switch (state)
        {
            case TypeItemState.Lock:
                txtBtn.text = $"{price} <sprite=0>";
                break;
            case TypeItemState.Unlock:
                txtBtn.text = "Select";
                SetInterable(true);
                break;
            case TypeItemState.Selected:
                txtBtn.text = "Selected";
                SetInterable(false);
                break;
        }
    }

    public void SetInterable(bool isInteracble)
    {
        btn.interactable = isInteracble;
    }

    public void OnSelectedItem()
    {
        SetStateBtn(TypeItemState.Selected);
    }

    public void OnUnSelect()
    {
        SetStateBtn(TypeItemState.Unlock);
    }
}
