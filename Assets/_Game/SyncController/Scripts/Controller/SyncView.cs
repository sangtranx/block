using DataLogin;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
public class SyncView : MonoBehaviour
{
    [Header("Object")]
    [SerializeField] private PopupLoginController popupLoginController;
    [SerializeField] private DBLoginController dBLoginController;
    [SerializeField] private GoogleSignInController googleSignInController;
    [SerializeField] private AppleSignInController appleSignInController;
    [SerializeField] private ScreenCover screenCoverLog;
    [SerializeField] private ScreenCover screenCoverSync;
    [Header("RectTransform")]
    [SerializeField] private RectTransform rtfmOptionLogin;
    [SerializeField] private RectTransform rtfmLogout;
    [Header("Button")]
    [SerializeField] private Button btnTadaplay;
    [SerializeField] private Button btnGoogle;
    [SerializeField] private Button btnApple;
    [SerializeField] private Button btnGuest;
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnLogout;
    public DBLoginController DBLoginController { get => dBLoginController; }
    public PopupLoginController PopupLoginController { get => popupLoginController; }
    public GoogleSignInController GoogleSignInController { get => googleSignInController; }
    public AppleSignInController AppleSignInController { get => appleSignInController; }
    public ScreenCover ScreenCoverLog { get => screenCoverLog; }
    public ScreenCover ScreenCoverSync { get => screenCoverSync; }

    public void AnimationButton()
    {
        btnTadaplay.transform.DOScale(Vector3.one, 0.25f).OnComplete(() =>
        {
            btnTadaplay.transform.DOScale(Vector3.one * 0.8f, 0.25f);
        }).SetLoops(-1).SetLink(rtfmOptionLogin.gameObject).SetId(this);
    }

    public void SetStatusLoginOption(bool isLogin)
    {
        if (!isLogin)
        {
            DOTween.Kill(this);
            AnimationButton();
        }
        rtfmOptionLogin.gameObject.SetActive(!isLogin);
        rtfmLogout.gameObject.SetActive(isLogin);
    }

    public void SetDisableLogout()
    {
        rtfmLogout.gameObject.SetActive(false);
    }

    public void SetDisableView()
    {
        DOTween.Kill(this);
        rtfmOptionLogin.gameObject.SetActive(false);
        rtfmLogout.gameObject.SetActive(false);
    }

    public void InitBtnTadaplay(UnityAction action)
    {
        btnTadaplay.onClick.RemoveAllListeners();
        btnTadaplay.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }

    public void InitBtnGoogle(UnityAction action)
    {
        btnGoogle.onClick.RemoveAllListeners();
        btnGoogle.onClick.AddListener(() =>
        {
            Debug.Log("Login Google");
            action?.Invoke();
        });
    }

    public void InitBtnApple(UnityAction action)
    {
#if !UNITY_IOS
        btnApple.gameObject.SetActive(false);
#endif
        btnApple.onClick.RemoveAllListeners();
        btnApple.onClick.AddListener(() =>
        {
            Debug.Log("Login Apple");
            action?.Invoke();

        });
    }

    public void InitBtnGuest(UnityAction action)
    {
        btnGuest.onClick.RemoveAllListeners();
        btnGuest.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
    public void InitBtnLogout(UnityAction action)
    {
        btnLogout.onClick.RemoveAllListeners();
        btnLogout.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }    
    
    public void InitBtnPlay(UnityAction action)
    {
        btnPlay.onClick.RemoveAllListeners();
        btnPlay.onClick.AddListener(() =>
        {
            action?.Invoke();
        });
    }
}
