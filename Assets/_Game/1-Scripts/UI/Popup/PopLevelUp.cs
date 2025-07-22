using Data;
using Game.Ultis;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopLevelUp : PopupBase
{
    [SerializeField] private Button btnClaim;
    [SerializeField] private Button btnAdsX2;
    [SerializeField] private TextMeshProUGUI txGold;
    [SerializeField] private TextMeshProUGUI txGoldBonus;
    long goldBonus;
    private UserData userData => DBController.Instance.USER_DATA;
    private void Start()
    {
        GameEvent.onUpdateUIByType += UpdateUIByType;
    }

    private void OnDestroy()
    {
        GameEvent.onUpdateUIByType -= UpdateUIByType;
    }
    public override void InitPopup()
    {
        base.InitPopup();
        InitBtn();
    }

    private void InitBtn()
    {
        btnClaim.onClick.RemoveAllListeners();
        btnClaim.onClick.AddListener(() => Claim());

        btnAdsX2.onClick.RemoveAllListeners();
        btnAdsX2.onClick.AddListener(() => AdsX2());
    }
    public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
    {
        AudioController.Instance.Play(AudioName.Sound_SubCoin);
        base.ShowPopup(popupModel, onShowComplete);
        btnClaim.gameObject.SetActive(false);
        btnAdsX2.gameObject.SetActive(false);
    }
    public void SetBonusGold(long value)
    {
        txGoldBonus.text = $"+{value}";
        goldBonus = value;
    }
    void Claim()
    {
        AudioController.Instance.Play(AudioName.Sound_AddCoin);
        GameHelper.Instance.GamePlayUI.FlyAnimationByType(TypeResource.Coin, (int)goldBonus, transform.position);
        HidePopup();
    }
    void AdsX2()
    {
        GetComponent<LevelUpController>().OnLevelUp(goldBonus);
        HidePopup();
    }

    protected override void InitCloseFade()
    {
        // base.InitCloseFade();
    }

    private void UpdateUIByType(TypeResource typeUIUpdate, long value)
    {
        if(typeUIUpdate == TypeResource.Coin)
        {
            GameHelper.Instance.ValueDisplayUI.LerpValue(value, txGold, string.Empty, "", 2f);

            //Set cho điểm bonus
            GameEvent.onBonusGoldGonnaChange?.Invoke(goldBonus);
            //Lerp về 0
            GameHelper.Instance.ValueDisplayUI.LerpBonusGold(0, txGoldBonus, onComplete: () =>
            {
                btnClaim.gameObject.SetActive(true);
                // btnAdsX2.gameObject.SetActive(true);
            });
        }
    }
}
