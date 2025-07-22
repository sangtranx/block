using Data;
using UnityEngine;
using UnityEngine.UI;

public class SkinChanger : MonoBehaviour
{
    [SerializeField] private ShopCategory shopCategory;
    [SerializeField] private SpriteShopSO spriteShopSO;
    private SpriteRenderer spriteRenderer;
    private Image imageSkin;
    private Animator animator;
    private ShopDB shopDB => DBController.Instance.SHOP_DB;
    public SpriteRenderer SpriteRenderer
    {
        get
        {
            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponent<SpriteRenderer>();
            }
            return spriteRenderer;
        }
    }
    public Image ImageSkin
    {
        get
        {
            if (imageSkin == null)
            {
                imageSkin = GetComponent<Image>();
            }
            return imageSkin;
        }
    }
    public Animator Animator
    {
        get
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
            return animator;
        }
    }

    private void Start()
    {
        GameEvent.onChangeSkin += ChangeSkin;
        var shopModel = shopDB.GetShopModelByType(shopCategory);
        GameEvent.onChangeSkin?.Invoke(shopModel.shopCategory, shopModel.currentRegion);
    }

    private void OnDestroy()
    {
        GameEvent.onChangeSkin -= ChangeSkin;
    }

    private void ChangeSkin(ShopCategory typeSkin, SkinType typeRegion)
    {
        //Debug.Log($"TypeSkin: {typeSkin} - Type Region: {typeRegion}");
        if (this.shopCategory != typeSkin) return;
        var spriteShop = spriteShopSO.GetSpriteShopByTypeCharacter(typeSkin);
        var sprSkin = spriteShop.GetSpriteSkinByTypeRegion(typeRegion);
        try
        {
            SpriteRenderer.sprite = sprSkin.sprSkin;
        }
        catch { }
        try
        {
            ImageSkin.sprite = sprSkin.sprSkin;
        }
        catch (System.Exception ew) 
        { 
            Debug.Log(ew); 
        }
        
        if (Animator == null) return;
        Animator.runtimeAnimatorController = sprSkin.animator;
        //GameManager.Instance.skinMapManager.ChangeSkin(typeRegion);
    }
}
