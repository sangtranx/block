using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupRegister : PopupBaseLogin
{
    [Header("Button")]
    [SerializeField] private Button btnRegister;
    [SerializeField] private Button btnCancel;
    [Header("InputField")]
    [SerializeField] private TMP_InputField inputFieldEmail;
    [SerializeField] private TMP_InputField inputFieldName;
    [SerializeField] private TMP_InputField inputFieldUserName;
    [SerializeField] private TMP_InputField inputFieldPassword;
    [SerializeField] private TMP_InputField inputFieldConfirmPassword;

    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnRegister();
        InitBtnCancel();
    }

    private void InitBtnRegister()
    {
        btnRegister.onClick.RemoveAllListeners();
        btnRegister.onClick.AddListener(() =>
        {
            //SyncController.Instance.ScreenCoverLog.Show();
            var registerRequest = new RegisterRequest
            {
                email = inputFieldEmail.text,
                name = inputFieldName.text,
                password = inputFieldPassword.text,
                username = inputFieldUserName.text,
                confirm_password = inputFieldConfirmPassword.text,
            };
            TadaplayAPI.Register(registerRequest, (message) =>
            {
                if (message == null)
                {
                    message = new List<string>();
                    message.Add("Created user successfully");
                    HidePopup();
                }
                else
                {
                    for (int i = 0; i < message.Count; i++)
                    {
                        Debug.Log(message[i]);
                    }
                };
                var popupLoginModel = new PopupLoginModel(PopupLoginType.Result);
                popupLoginModel.lstMessage = message;
                PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
                //SyncController.Instance.ScreenCoverLog.Hide();
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
}
