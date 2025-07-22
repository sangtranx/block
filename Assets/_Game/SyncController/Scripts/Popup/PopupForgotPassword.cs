using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupForgotPassword : PopupBaseLogin
{
    [SerializeField] private float timerOTP = 60;
    [Header("Button")]
    [SerializeField] private Button btnSend;
    [SerializeField] private Button btnOK;
    [SerializeField] private Button btnCancel;
    [Header("InputField")]
    [SerializeField] private TMP_InputField inputFieldEmail;
    [SerializeField] private TMP_InputField inputFieldOTP;
    [SerializeField] private TMP_InputField inputFieldNewPassword;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI txtCountdown;
    [Header("GameObject")]
    [SerializeField] private GameObject gobjOTP;
    [SerializeField] private GameObject gobjNewPass;

    private bool isVerifyDone = false;
    private VerityOTPData verityOTPData;
    private float timerOTPCountdown;
    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnSend();
        InitBtnCancel();
        InitBtnOk();
    }

    private void InitBtnSend()
    {
        btnSend.onClick.RemoveAllListeners();
        btnSend.onClick.AddListener(() =>
        {
           // SyncController.Instance.ScreenCoverLog.Show();
            var forgotPasswordRequest = new ForgotPasswordRequest
            {
                email = inputFieldEmail.text,
            };
            TadaplayAPI.ForgotPassword(forgotPasswordRequest, (message) =>
            {
                if (message == null)
                {
                    Debug.Log("Send Success full");
                    StartCoroutine(CountDownOTP());
                }
                else
                {
                    for (int i = 0; i < message.Count; i++)
                    {
                        Debug.Log(message[i]);
                    }
                    var popupLoginModel = new PopupLoginModel(PopupLoginType.Result);
                    popupLoginModel.lstMessage = message;
                    PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
                }
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

    private void InitBtnOk()
    {
        btnOK.onClick.RemoveAllListeners();
        btnOK.onClick.AddListener(() =>
        {
            if (!isVerifyDone)
            {
                VerifyOK();
            }
            else
            {
                NewPassOK();
            }
        });
    }

    private void VerifyOK()
    {
       // SyncController.Instance.ScreenCoverLog.Show();
        var vertifyOTP = new VerifyOTPRequest
        {
            email = inputFieldEmail.text,
            code = inputFieldOTP.text,
        };
        TadaplayAPI.VertifyOTP(vertifyOTP, (state, verifyOTPData, message) =>
        {
            if (state)
            {
                this.verityOTPData = verifyOTPData;
                gobjOTP.gameObject.SetActive(false);
                gobjNewPass.gameObject.SetActive(true);
                btnSend.interactable = false;
                btnOK.interactable = true;
            }
            else
            {
                if (message == null)
                {
                    message = new List<string>();
                    message.Add("OTP is wrong");
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
            }
            isVerifyDone = state;
            //SyncController.Instance.ScreenCoverLog.Hide();
        });
    }

    private void NewPassOK()
    {
       // SyncController.Instance.ScreenCoverLog.Show();
        var updatePasswordRequest = new UpdatePasswordRequest
        {
            email = verityOTPData.email,
            token = verityOTPData.token,
            new_password = inputFieldNewPassword.text,
        };

        TadaplayAPI.UpdatePassword(updatePasswordRequest, (message) =>
        {
            if (message == null)
            {
                var popupLoginModel = new PopupLoginModel(PopupLoginType.Result);
                popupLoginModel.lstMessage = new List<string>();
                popupLoginModel.lstMessage.Add("Change pass successfull");
                PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
                HidePopup();
            }
            else
            {
                for (int i = 0; i < message.Count; i++)
                {
                    Debug.Log(message[i]);
                }
                var popupLoginModel = new PopupLoginModel(PopupLoginType.Result);
                popupLoginModel.lstMessage = message;
                PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
            }
            //SyncController.Instance.ScreenCoverLog.Hide();
        });
    }

    public void ResetStatePopup()
    {
        btnSend.interactable = true;
        btnOK.interactable = false;
        gobjNewPass.SetActive(false);
        gobjOTP.SetActive(true);
    }

    private IEnumerator CountDownOTP()
    {
        btnSend.interactable = false;
        timerOTPCountdown = timerOTP;
        txtCountdown.text = $"{timerOTPCountdown}s";
        while (timerOTPCountdown > 0)
        {
            yield return new WaitForSeconds(1);
            timerOTPCountdown -= 1;
            txtCountdown.text = $"{timerOTPCountdown}s";
        }
        txtCountdown.text = $"Send";
        btnSend.interactable = true;
    }
}
