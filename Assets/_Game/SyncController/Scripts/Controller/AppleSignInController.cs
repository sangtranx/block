using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System.Text;
using Newtonsoft.Json.Linq;
public class AppleSignInController : MonoBehaviour
{
    IAppleAuthManager m_AppleAuthManager;
    public string Token { get; private set; }
    public string Error { get; private set; }

    private void Start()
    {
        if (m_AppleAuthManager == null)
        {
            Initialize();
        }
    }

    public void Initialize()
    {
        var deserializer = new PayloadDeserializer();
        m_AppleAuthManager = new AppleAuthManager(deserializer);
    }

    public void Update()
    {
        if (m_AppleAuthManager != null)
        {
            m_AppleAuthManager.Update();
        }
    }

    public void LoginToApple()
    {
        // Initialize the Apple Auth Manager
        if (m_AppleAuthManager == null)
        {
            Initialize();
        }

        // Set the login arguments
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        // Perform the login
        m_AppleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                var appleIDCredential = credential as IAppleIDCredential;
                if (appleIDCredential != null)
                {
                    var idToken = Encoding.UTF8.GetString(
                        appleIDCredential.IdentityToken,
                        0,
                        appleIDCredential.IdentityToken.Length);
                    Debug.Log("Sign-in with Apple successfully done. IDToken: " + idToken);
                    Debug.Log("Sign-in with Apple successfully done. Email: " + appleIDCredential.Email);
                    Token = idToken;
                    var loginAppleRequest = new LoginAppleRequest(idToken);
                    TadaplayAPI.LoginWithApple(loginAppleRequest, ((data, messenge) =>
                    {
                        JObject jsonObject = JObject.Parse(data);
                        JToken payloadToken = jsonObject["data"];
                        if (payloadToken != null)
                        {
                            var loginAppleResult = JsonUtility.FromJson<LoginMobileResult>(payloadToken.ToString());
                            var loginResult = new LoginResult()
                            {
                                data = new LoginData()
                                {
                                    access_token = loginAppleResult.access_token,
                                    refresh_token = loginAppleResult.refresh_token,
                                    user =  new HubData()
                                    {
                                        name =  loginAppleResult.user.name,
                                        username =  loginAppleResult.user.username,
                                        email =  loginAppleResult.user.email,
                                        id =  loginAppleResult.user.id,
                                        balance =  loginAppleResult.user.balance,
                                    }
                                }
                            };
                            //SyncController.Instance.SetLoginResult(loginResult);
                           // SyncController.Instance.CheckDataLogin();
                            GameEventLogin.onCompleteLoginInGame?.Invoke();
                            Debug.Log($"Apple: {data}");
                           // Debug.Log( $"Apple Token: {SyncController.Instance.DBLoginController.LOGIN_RESULT.data.access_token}");
                        }
                    }));
                }
                else
                {
                    Debug.Log("Sign-in with Apple error. Message: appleIDCredential is null");
                    Error = "Retrieving Apple Id Token failed.";
                }
            },
            error =>
            {
                Debug.Log("Sign-in with Apple error. Message: " + error);
                Error = "Retrieving Apple Id Token failed.";
            }
        );
    }
}

