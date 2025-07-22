using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using DataLogin;
using System;
using Newtonsoft.Json.Linq;
using Assets.SimpleGoogleSignIn.Scripts;

public class GoogleSignInController : MonoBehaviour
{
    private GoogleAuth GoogleAuth;
    public void Start()
    {
        GoogleAuth = new GoogleAuth();
    }

    public void OnGoogleClick()
    {
        GoogleAuth.GetAccessToken(OnGetAccessToken);
    }

    public void LogoutGoolge()
    {
        //GoogleSignIn.DefaultInstance.SignOut();
        GoogleAuth.SignOut(revokeAccessToken: true);
    }

    private void OnGetAccessToken(bool success, string error, TokenResponse tokenResponse)
    {
        string log = success ? $"Access token: {tokenResponse.AccessToken}" : error;
        Debug.Log($"OnGetAccessToke - {log}");
        if (!success) return;
        var jwt = new JWT(tokenResponse.IdToken);
        Debug.Log($"JSON Web Token (JWT) Payload: {jwt.Payload}");
        jwt.ValidateSignature(GoogleAuth.ClientId, OnValidateSignature);
        var loginGoogleRequest = new LoginGoogleRequest(tokenResponse.IdToken);
        TadaplayAPI.LoginWithGoogle(loginGoogleRequest, (data, messenge) =>
        {
            JObject jsonObject = JObject.Parse(data);
            JToken payloadToken = jsonObject["data"];
            if (payloadToken != null)
            {
                var loginGoogleResult = JsonUtility.FromJson<LoginMobileResult>(payloadToken.ToString());
                var loginResult = new LoginResult()
                {
                    data = new LoginData()
                    {
                        access_token = loginGoogleResult.access_token,
                        refresh_token = loginGoogleResult.refresh_token,
                        user =  new HubData()
                        {
                            name =  loginGoogleResult.user.name,
                            username =  loginGoogleResult.user.username,
                            email =  loginGoogleResult.user.email,
                            id =  loginGoogleResult.user.id,
                            balance =  loginGoogleResult.user.balance,
                        }
                    }
                };
                //SyncController.Instance.SetLoginResult(loginResult);
                //SyncController.Instance.CheckDataLogin();
                GameEventLogin.onCompleteLoginInGame?.Invoke();
                Debug.Log($"Google: {data}");
               // Debug.Log( $"Token: {SyncController.Instance.DBLoginController.LOGIN_RESULT.data.access_token}");
            }
        });
    }

    private void OnValidateSignature(bool success, string error)
    {
        string log = success ? "JWT signature validated" : error;
        Debug.Log($"OnValidateSignature - {log}");
    }
}
