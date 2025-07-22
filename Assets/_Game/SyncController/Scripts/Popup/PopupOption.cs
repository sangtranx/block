using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupOption : PopupBaseLogin
{
    [SerializeField] private Button btnTadaplay;
    [SerializeField] private Button btnGoogle;
    [SerializeField] private Button btnApple;
    [SerializeField] private Button btnGuest;

    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnTadaplay();
        InitBtnGoogle();
        InitBtnApple();
        InitBtnGuest();
    }

    private void InitBtnTadaplay()
    {
        btnTadaplay.onClick.RemoveAllListeners();
        btnTadaplay.onClick.AddListener(() =>
        {
            var popupLoginModel = new PopupLoginModel(PopupLoginType.LoginTadaplay);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
    }

    private void InitBtnGoogle()
    {
        btnGoogle.onClick.RemoveAllListeners();
        btnGoogle.onClick.AddListener(() =>
        {
            Debug.Log("Login Google");
            //SyncController.Instance.GoogleSignInController.OnGoogleClick();
        });
    }

    private void InitBtnApple()
    {
        btnApple.onClick.RemoveAllListeners();
        btnApple.onClick.AddListener(() =>
        {
            Debug.Log("Login Apple");
           // SyncController.Instance.AppleSignInController.LoginToApple();

        });
    }

    private void InitBtnGuest()
    {
        btnGuest.onClick.RemoveAllListeners();
        btnGuest.onClick.AddListener(() =>
        {
            Debug.Log("Login Guest");
            var popupLoginModel = new PopupLoginModel(PopupLoginType.Warning);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
    }
}
