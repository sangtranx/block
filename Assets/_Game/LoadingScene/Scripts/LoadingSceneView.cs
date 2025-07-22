using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class LoadingSceneView : MonoBehaviour
{
    [SerializeField] private Image imgFadeUI;
    [SerializeField] private GameObject gobjLoading;
    [SerializeField] private Image imgFill;
    [SerializeField] private TextMeshProUGUI txtLoadingValue;
    [SerializeField] private TextMeshProUGUI txtVersion;

    public void SetVersion()
    {
        txtVersion.text = $"v{Application.version}";
    }

    public void FadeOutBlackScreen()
    {
        imgFadeUI.DOFade(0, 0.75f).SetEase(Ease.Linear).OnComplete(() =>
        {
            imgFadeUI.gameObject.SetActive(false);
        });
    }

    public void FadeOutBlackScreen(UnityAction onComplete = null)
    {
        imgFadeUI.gameObject.SetActive(true);
        imgFadeUI.DOFade(1f, 0);
        imgFadeUI.DOFade(1f, 1f).OnComplete(() =>
        {
            imgFadeUI.DOFade(0, 2f).SetEase(Ease.Linear).OnComplete(() =>
            {
                onComplete?.Invoke();
                imgFadeUI.gameObject.SetActive(false);
            });
        });
    }

    public void AnimationFillBar(UnityAction onCompleteFade = null)
    {
        DOVirtual.Int(0, 100, 2f, (value) =>
        {
            imgFill.fillAmount = value / 100f;
            txtLoadingValue.text = $"Loading... {value}%";
        }).OnComplete(() =>
        {
            onCompleteFade?.Invoke();
        });
    }

    public void SetFade()
    {
        imgFadeUI.gameObject.SetActive(true);
        imgFadeUI.DOFade(1, 0f).SetEase(Ease.Linear);
    }

    public void ChangSceneAnimation(UnityAction onCompleteFade = null)
    {
        imgFadeUI.gameObject.SetActive(true);
        imgFadeUI.DOFade(1, 0.75f).SetEase(Ease.Linear).OnComplete(() =>
        {
            onCompleteFade?.Invoke();
        });
    }

    public void DisableLoadingScreen()
    {
        gobjLoading.SetActive(false);
    }
}
