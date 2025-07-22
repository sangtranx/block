using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using DataLogin;

public static class TadaplayAPI
{
    private static string apiUrl = "https://api.tadacloud.app/api/";

    public static async UniTask PostData(string access_token, DataToPos dataToPos, Action onComplete = null,
        Action onCatchIssueComplete = null)
    {
        var jsonData = JsonUtility.ToJson(dataToPos);
        await PostAPI("game/sync", jsonData, access_token, onCatchIssue: onCatchIssueComplete);
        onComplete?.Invoke();
    }

    public static async UniTask SyncDataFormSever(string token, Action<bool, string> onComplete,
        Action onCatchIssueComplete = null)
    {
        // Debug.Log("Awaitt");
        await GetAPI($"game/sync/{TypeNameGame.BlockMerge}", token, (state, data) =>
        {
            Debug.Log($"Awaitt Complete + {data}");
            onComplete.Invoke(state, data);
        }, onCatchIssueComplete);
    }

    public static async UniTask GetDataForLeaderBoard(string token, Action<bool, string> onComplete,
        Action onCatchIssue = null)
    {
        // Debug.Log("Awaitt Leaderboard");
        await GetAPI($"game/sync/leaderboard/{TypeNameGame.BlockMerge}", token, (state, data) =>
        {
            // Debug.Log($"Awaitt Complete Leaderboard + {data}");
            onComplete.Invoke(state, data);
        }, onCatchIssue);
    }

    public static void Login(LoginRequest loginRequest, Action<bool, string, List<string>> response = null)
    {
        var loginRequestData = JsonUtility.ToJson(loginRequest);
        PostAPI("auth/login", loginRequestData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            if (state)
            {
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(state, description, lstMessage);
        }).Forget();
    }

    public static void Register(RegisterRequest registerRequest, Action<List<string>> response = null)
    {
        var registerRequestData = JsonUtility.ToJson(registerRequest);
        PostAPI("auth/register", registerRequestData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            if (state)
            {
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(lstMessage);
        }).Forget();
    }

    public static void LoginWithGoogle(LoginGoogleRequest loginGoogleRequest,
        Action<string, List<string>> response = null)
    {
        var loginRequestData = JsonUtility.ToJson(loginGoogleRequest);
        PostAPI("auth/google", loginRequestData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            if (state)
            {
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(description, lstMessage);
        }).Forget();
    }

    public static void LoginWithApple(LoginAppleRequest loginAppleRequest,
        Action<string, List<string>> response = null)
    {
        var loginRequestData = JsonUtility.ToJson(loginAppleRequest);
        PostAPI("auth/apple/verify", loginRequestData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            if (state)
            {
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(description, lstMessage);
        }).Forget();
    }


    public static void ForgotPassword(ForgotPasswordRequest forgotPasswordRequest, Action<List<string>> response = null)
    {
        var forgotPasswordData = JsonUtility.ToJson(forgotPasswordRequest);
        PostAPI("auth/forgot-password", forgotPasswordData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            if (state)
            {
                //var loginResult = JsonUtility.FromJson<LoginResult>(description);
                //token = loginResult.data.access_token;
                //Debug.Log($"Token: {token}");
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(lstMessage);
        }).Forget();
    }

    public static void VertifyOTP(VerifyOTPRequest verifyOTPRequest,
        Action<bool, VerityOTPData, List<string>> response = null)
    {
        var verifyOTPData = JsonUtility.ToJson(verifyOTPRequest);
        PostAPI("auth/verify-otp", verifyOTPData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            bool stateVerify = false;
            VerityOTPData verityOTPData = null;
            if (state)
            {
                var verifyOTPRespone = JsonUtility.FromJson<VerityOTPRespone>(description);
                stateVerify = true;
                verityOTPData = verifyOTPRespone.data;
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(stateVerify, verityOTPData, lstMessage);
        }).Forget();
    }

    public static void UpdatePassword(UpdatePasswordRequest updatePasswordRequest, Action<List<string>> response = null)
    {
        var updatePasswordData = JsonUtility.ToJson(updatePasswordRequest);
        PostAPI("auth/reset-password", updatePasswordData, response: (state, description) =>
        {
            List<string> lstMessage = null;
            if (state)
            {
            }
            else
            {
                var jsonTrim = TrimJson(description);
                if (jsonTrim != string.Empty)
                {
                    var message = ParseJsonMessage(jsonTrim, "message");
                    lstMessage = message;
                }
            }

            response?.Invoke(lstMessage);
        }).Forget();
    }

    private static string TrimJson(string value)
    {
        int startIndex = value.IndexOf('{');
        int endIndex = value.LastIndexOf('}');
        string jsonSub = string.Empty;
        if (startIndex != -1 && endIndex != -1)
        {
            jsonSub = value.Substring(startIndex, endIndex - startIndex + 1);
        }
        else
        {
            Debug.LogError("JSON not found in the provided string.");
        }

        return jsonSub;
    }

    private static List<string> ParseJsonMessage(string jsonString, string nameSlit)
    {
        var jsonTrim = TrimJson(jsonString);
        if (jsonTrim != string.Empty)
        {
            var json = JObject.Parse(jsonTrim);
            JToken messageToken = json[nameSlit];
            if (messageToken != null)
            {
                if (messageToken.Type == JTokenType.Array)
                {
                    return messageToken.ToObject<List<string>>();
                }
                else if (messageToken.Type == JTokenType.String)
                {
                    List<string> messages = new List<string>();
                    messages.Add(messageToken.ToString());
                    return messages;
                }
            }
        }

        return null;
    }

    #region Post API

    private static async UniTask PostAPI(string subUrl, string data, string access_token = null,
        Action<bool, string> response = null, Action onCatchIssue = null)
    {
        var url = apiUrl + subUrl;
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            if (!string.IsNullOrEmpty(access_token))
                www.SetRequestHeader("Authorization", "Bearer " + access_token);
            www.SetRequestHeader("Content-Type", "application/json");
            try
            {
                await www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Debug.LogError("Error: " + www.error);
                    response?.Invoke(false, www.error);
                }
                else
                {
                    // Debug.Log("Success full: " + "Token: " + access_token + "   " + www.downloadHandler.text);
                    response?.Invoke(true, www.downloadHandler.text);
                }
            }
            catch (UnityWebRequestException unityWebRequestException)
            {
                response?.Invoke(false, unityWebRequestException.Message);
                onCatchIssue?.Invoke();
            }
        }
    }

    #endregion

    #region Get API

    private static async UniTask GetAPI(string subUrl, string token, Action<bool, string> response = null,
        Action onCatchIssue = null)
    {
        var text = string.Empty;
        var url = apiUrl + subUrl;
        using (UnityWebRequest www = new UnityWebRequest(url, "GET"))
        {
            www.SetRequestHeader("Authorization", "Bearer " + token);
            www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            try
            {
                await www.SendWebRequest();
                if (www.result != UnityWebRequest.Result.Success)
                {
                    // Debug.Log("sssss");
                    // Debug.LogError("Error: " + www.error);
                    response?.Invoke(false, text);
                }
                else
                {
                    text = www.downloadHandler.text;
                    response?.Invoke(true, text);
                }
            }
            catch (UnityWebRequestException unityWebRequestException)
            {
                response?.Invoke(false, unityWebRequestException.Message);
                onCatchIssue?.Invoke();
            }
        }
    }

    #endregion
}