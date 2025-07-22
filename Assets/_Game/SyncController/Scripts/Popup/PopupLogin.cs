using DataLogin;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupLogin : PopupBaseLogin
{
    [Header("Button")]
    [SerializeField] private Button btnLogin;
    [SerializeField] private Button btnRegister;
    [SerializeField] private Button btnForgotPassword;
    [SerializeField] private Button btnCancel;
    [Header("InputField")]
    [SerializeField] private TMP_InputField inputFieldEmail;
    [SerializeField] private TMP_InputField inputFieldPassword;

    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnLogin();
        InitBtnCancel();
        InitBtnRegister();
        InitForgotPassword();
    }

    private void InitBtnLogin()
    {
        btnLogin.onClick.RemoveAllListeners();
        btnLogin.onClick.AddListener(() =>
        {
            //SyncController.Instance.ScreenCoverLog.Show();
            var loginRequest = new LoginRequest
            {
                email = inputFieldEmail.text,
                password = inputFieldPassword.text,
            };
            TadaplayAPI.Login(loginRequest, (state, data, message) =>
            {
                if (state)
                {
                    message = new List<string>();
                    message.Add("Login Success full");
                    var loginResult = JsonUtility.FromJson<LoginResult>(data);
                   // SyncController.Instance.SetLoginResult(loginResult);
                    //SyncController.Instance.CheckDataLogin();
                    HidePopup();
                    GameEventLogin.onCompleteLoginInGame?.Invoke();
                }
                else
                {
                    if (message == null)
                    {
                        message = new List<string>();
                        message.Add("Wrong password or account");
                    }
                    else
                    {
                        for (int i = 0; i < message.Count; i++)
                        {
                            Debug.Log(message[i]);
                        }
                    }
                    var popupLoginModel = new PopupLoginModel(PopupLoginType.Result);
                    popupLoginModel.lstMessage = message;
                    PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
                   // SyncController.Instance.ScreenCoverLog.Hide();
                }
            });
        });
    }

    private void InitBtnCancel()
    {
        btnCancel.onClick.RemoveAllListeners();
        btnCancel.onClick.AddListener(() =>
        {
            Debug.Log("Btn Cancel");
            HidePopup();
        });
    }

    private void InitForgotPassword()
    {
        btnForgotPassword.onClick.RemoveAllListeners();
        btnForgotPassword.onClick.AddListener(() =>
        {
            var popupLoginModel = new PopupLoginModel(PopupLoginType.ForgotPassword);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
    }

    private void InitBtnRegister()
    {
        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(() =>
        {
            var popupLoginModel = new PopupLoginModel(PopupLoginType.Resgister);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
    }
}
