using Data;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Runtime.CompilerServices;

public class PopupSettings : PopupBase
{
    [Header("Txt")]
    [SerializeField] private TextMeshProUGUI txtVersion;
    [Header("Button")]
    [SerializeField] private Button btnClose;
    [SerializeField] private Button btnFb;
    [SerializeField] private Button btnYtb;
    [SerializeField] private Button btnX;
    [SerializeField] private Button btnSound;
    [SerializeField] private Button btnMusic;
    [Header("Slider")]
    [SerializeField] private Slider sliderMusic;
    [SerializeField] private Slider sliderSound;

    [Header("SO")]
    [SerializeField] private PopupSettingsSO popupSettingsSO;

    private Image imgBtnMusic;
    private Image imgBtnSound;
    [Header("Link Account")]
    [SerializeField] private RectTransform rtfmLogin;
    [SerializeField] private RectTransform rtfmLogout;
    [SerializeField] private Button btnTadaplay;
    [SerializeField] private Button btnGoogle;
    [SerializeField] private Button btnApple;
    [SerializeField] private Button btnLogOut;
    public override void InitPopup()
    {
        InitBtnClose();
        InitSliderMusic();
        InitSliderSound();
        txtVersion.text = $"v{Application.version}";
        base.InitPopup();
        /*InitBtnFb();
        InitBtnX();
        InitBtnYtb();*/
        imgBtnMusic = btnMusic.GetComponent<Image>();
        imgBtnSound = btnSound.GetComponent<Image>();
        InitSprite();
        InitBtnMusic();
        InitBtnSound();
        InitBtn();
       // GameEventLogin.onCompleteLoginInGame += OnCompleteLoginInGame;
    }

    private void OnDestroy()
    {
       // GameEventLogin.onCompleteLoginInGame -= OnCompleteLoginInGame;
    }

    private void OnCompleteLoginInGame()
    {
        /*Debug.Log("GamePlay");
        LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
       // SyncController.Instance.SetStatusLoginOption();*/
    }


    private void InitBtnClose()
    {
        btnClose.onClick.RemoveAllListeners();
        btnClose.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }

    private void InitBtnFb()
    {
        btnFb.onClick.RemoveAllListeners();
        btnFb.onClick.AddListener(() =>
        {
            var url = "https://www.facebook.com/TadaPlayStudio";
            Application.OpenURL(url);
        });
    }

    private void InitBtnYtb()
    {
        btnYtb.onClick.RemoveAllListeners();
        btnYtb.onClick.AddListener(() =>
        {
            var url = "https://www.youtube.com/@TadaPlayStudio";
            Application.OpenURL(url);
        });
    }

    private void InitBtnX()
    {
        btnX.onClick.RemoveAllListeners();
        btnX.onClick.AddListener(() =>
        {
            var url = "https://twitter.com/@TadaPlayStudio";
            Application.OpenURL(url);
        });
    }


    private void InitSliderMusic()
    {
        sliderMusic.value = DBController.Instance.USER_SETTINGS.music;
        sliderMusic.onValueChanged.RemoveAllListeners();
        sliderMusic.onValueChanged.AddListener(value =>
        {
            DBController.Instance.USER_SETTINGS.ChangeVolumeByType(TypeVolume.Music,value);
            GameEvent.onChangeVolume.Invoke(TypeVolume.Music,value);
            InitSprite(imgBtnMusic, TypeBtnSettings.Music, value);
        });

    }

    private void InitSliderSound()
    {
        sliderSound.value = DBController.Instance.USER_SETTINGS.sound;
        sliderSound.onValueChanged.RemoveAllListeners();
        sliderSound.onValueChanged.AddListener(value =>
        {
            DBController.Instance.USER_SETTINGS.ChangeVolumeByType(TypeVolume.Sound,value);
            GameEvent.onChangeVolume.Invoke(TypeVolume.Sound,value);
            InitSprite(imgBtnSound, TypeBtnSettings.Sound, value);
        });
    }

    private void InitBtnSound()
    {
        btnSound.onClick.RemoveAllListeners();
        btnSound.onClick.AddListener(() =>
        {
            var isSound = DBController.Instance.USER_SETTINGS.sound > 0;
            var value = isSound ? 0f : 1f;
            sliderSound.value = value;
            DBController.Instance.USER_SETTINGS.ChangeVolumeByType(TypeVolume.Sound,value);
            GameEvent.onChangeVolume.Invoke(TypeVolume.Sound,value);
            InitSprite(imgBtnSound, TypeBtnSettings.Sound, value);
        });
    }

    private void InitBtnMusic()
    {
        btnMusic.onClick.RemoveAllListeners();
        btnMusic.onClick.AddListener(() =>
        {
            var isMusic = DBController.Instance.USER_SETTINGS.music > 0;
            var value = isMusic ? 0f : 1f;
            sliderMusic.value = value;
            DBController.Instance.USER_SETTINGS.ChangeVolumeByType(TypeVolume.Music,value);
            GameEvent.onChangeVolume.Invoke(TypeVolume.Music,value);
            InitSprite(imgBtnMusic, TypeBtnSettings.Music, value);
        });
    }

    private void InitSprite()
    {
        var valueSound = DBController.Instance.USER_SETTINGS.sound;
        var valueMusic = DBController.Instance.USER_SETTINGS.music;
        InitSprite(imgBtnSound, TypeBtnSettings.Sound, valueSound);
        InitSprite(imgBtnMusic, TypeBtnSettings.Music, valueMusic);
    }

    private void InitSprite(Image image, TypeBtnSettings typeBtnSettings, float volume)
    {
        var isState = volume > 0f;
        var state = isState ? TypeButtonState.On : TypeButtonState.Off;
        var spr = popupSettingsSO.GetSpriteButtonSettingsByType(typeBtnSettings).GetSpriteButtonByType(state).sprBtn;
        image.sprite = spr;
    }

    public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
    {
        base.ShowPopup(popupModel, onShowComplete);
    }
    private bool isLogin;

    /*private void CheckStatusBtn()
    {
        isLogin = SyncController.Instance.GetIsLogin();
        rtfmLogin.gameObject.SetActive(!isLogin);
        rtfmLogout.gameObject.SetActive(isLogin);
    }*/
    private void InitBtn()
    {
       /*// CheckStatusBtn();
        btnTadaplay.onClick.RemoveAllListeners();
        btnTadaplay.onClick.AddListener(() =>
        {
            var popupLoginModel = new PopupLoginModel(PopupLoginType.LoginTadaplay);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
        btnGoogle.onClick.RemoveAllListeners();
        btnGoogle.onClick.AddListener(() =>
        {
            SyncController.Instance.GoogleSignInController.OnGoogleClick();
        });
        btnApple.onClick.RemoveAllListeners();
        btnApple.onClick.AddListener(() =>
        {
            SyncController.Instance.AppleSignInController.LoginToApple();
        });
        btnLogOut.onClick.RemoveAllListeners();
        btnLogOut.onClick.AddListener(() =>
        {
                Debug.Log(("sss"));
            LoadingSceneController.Instance.ChangeScene(SceneType.Main, () =>
            {
                SyncController.Instance.LogOut();
                SyncController.Instance.InitLogin();
                AudioController.Instance.PlayBG(AudioName.Music_BG_Loading);
            });
        });
#if !UNITY_IOS
        btnApple.gameObject.SetActive(false);
#endif*/
    }
}
