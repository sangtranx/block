using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Ultis;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Game.Helpers;

public class LuckySpinView : MonoBehaviour
{
    [Serializable]
    public class GiftReceiveViewSettings
    {
        public List<TypeGift> lstTypeGiftUseThisView;
        public GiftReceiveView giftReceiveView;
    }

    [SerializeField] Button btnSpin;
    [SerializeField] TimerButton btnSpinByAds;
    [SerializeField] TextMeshProUGUI txtStock;
    [SerializeField] List<GiftReceiveViewSettings> lstGiftReceiveViewSettings;
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] Image imgFade;
    [SerializeField] Button btnClaim;
    public event UnityAction onCompletedWaitForTimer;

    public TimerButton BtnSpinByAds
    {
        get => btnSpinByAds;
    }

    public void ResetView()
    {
        ReloadView();
        btnClaim.gameObject.SetActive(false);
        imgFade.gameObject.SetActive(false);
        for (int i = 0; i < lstGiftReceiveViewSettings.Count; i++)
        {
            lstGiftReceiveViewSettings[i].giftReceiveView.gameObject.SetActive(false);
        }
    }

    public void ReloadView()
    {
        txtStock.text = $"{SpinData.SPIN_AMOUNT}";
        btnSpin.interactable = SpinData.SPIN_AMOUNT > 0;
        btnSpinByAds.ReloadCurrentState();
    }

    public void SetTimer(TimeSpan remainTs)
    {
        StartCoroutine(TimerCoroutine(remainTs));
    }

    public void OnStartedSpin()
    {
        btnSpin.interactable = false;
        btnSpinByAds.interactable = false;
    }

    public void OnCompletedSpin()
    {

        //SpinData.LAST_TICK_SAVED_2_GET_FREE = DateTime.Now.Ticks;
        //SpinData.SPIN_AMOUNT--;
        btnSpin.interactable = false;
        //ReloadView();
    }

    public void SetGiftReceiveView(Gift gift)
    {
        if (lstGiftReceiveViewSettings.Count == 1)
        {
            lstGiftReceiveViewSettings[0].giftReceiveView.SetView(gift);
            cacheGift = gift;
            SetActiveReceiveView(0);
            return;
        }

        int indexView =
            lstGiftReceiveViewSettings.FindIndex((view) => view.lstTypeGiftUseThisView.Contains(gift.TypeGift));
        if (indexView != -1)
        {
            lstGiftReceiveViewSettings[indexView].giftReceiveView.SetView(gift);
            SetActiveReceiveView(indexView);
        }
    }

    void SetActiveReceiveView(int activeIndex)
    {
        btnClaim.gameObject.SetActive(false);
        imgFade.color = new Color(0, 0, 0, 0);
        imgFade.gameObject.SetActive(true);
        imgFade.DOFade(160f / 255, 0.25f).SetEase(Ease.InSine).SetLink(gameObject).onComplete += () =>
        {
            for (int i = 0; i < lstGiftReceiveViewSettings.Count; i++)
            {
                lstGiftReceiveViewSettings[i].giftReceiveView
                    .SetActiveView(activeIndex == i, () => { btnClaim.gameObject.SetActive(true); });
            }
        };
    }

    private Gift cacheGift;

    public void SetInactiveReceiveView()
    {
        AudioController.Instance.Play(AudioName.Sound_AddCoin);
        var screenPoint =
            Helper.MainCamera.ScreenToWorldPoint(new Vector3(Screen.height / 2, Screen.width / 2, 0));
        GameHelper.Instance.GamePlayUI.FlyAnimationByType(
            GetTypeResourceByTypeGift(cacheGift.TypeGift), cacheGift.Amount,
            screenPoint);
        btnClaim.gameObject.SetActive(false);
        for (int i = 0; i < lstGiftReceiveViewSettings.Count; i++)
        {
            if (lstGiftReceiveViewSettings[i].giftReceiveView.gameObject.activeInHierarchy)
            {
                lstGiftReceiveViewSettings[i].giftReceiveView.SetActiveView(false);
            }
        }

        imgFade.DOFade(0, 0.25f).SetEase(Ease.OutSine).SetLink(gameObject).onComplete +=
            () => imgFade.gameObject.SetActive(false);
    }


    private TypeResource GetTypeResourceByTypeGift(TypeGift typeGift)
    {
        var type = TypeResource.Coin;
        switch (typeGift)
        {
            case TypeGift.Coin:
                type = TypeResource.Coin;
                break;
            case TypeGift.Bom3x3:
                type = TypeResource.Bom3x3;
                break;
            case TypeGift.Rocket:
                type = TypeResource.Rocket;
                break;
            case TypeGift.Fire:
                type = TypeResource.Fire;
                break;
            case TypeGift.Thunder:
                type = TypeResource.Thunder;
                break;
        }

        return type;
    }

    IEnumerator TimerCoroutine(TimeSpan remainTs)
    {
        var oneSecTs = TimeSpan.FromSeconds(1);
        WaitForSecondsRealtime waitOneSec = new WaitForSecondsRealtime(1);
        while (remainTs.TotalSeconds > 0)
        {
            txtTimer.text = remainTs.ToString("mm\\:ss");
            yield return waitOneSec;
            remainTs = remainTs.Subtract(oneSecTs);
        }

        txtTimer.text = string.Empty;
        onCompletedWaitForTimer?.Invoke();
    }
}