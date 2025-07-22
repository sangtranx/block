/*
using Cysharp.Threading.Tasks;
using Data;
using DataLogin;
using Newtonsoft.Json.Linq;
using System;
using UnityEngine;
public class SyncController : Singleton<SyncController>
{
    [SerializeField] private SyncView loginView;
    public DBLoginController DBLoginController => loginView.DBLoginController;
    public PopupLoginController PopupLoginController => loginView.PopupLoginController;
    /*public GoogleSignInController GoogleSignInController => loginView.GoogleSignInController;
    public AppleSignInController AppleSignInController => loginView.AppleSignInController;#1#

    public ScreenCover ScreenCoverLog => loginView.ScreenCoverLog;
    public ScreenCover ScreenCoverSync => loginView.ScreenCoverSync;

    protected override void CustomAwake()
    {
        DBLoginController.Initializing();
    }

    private void Start()
    {
        InitLogin(() =>
        {
            LogOut();
        });
        GameEventLogin.onPostData += PostData;
    }

    private void OnDestroy()
    {
        GameEventLogin.onPostData -= PostData;
    }

    public void SetDisableView()
    {
        loginView.SetDisableView();
    }

    public void InitLogin(Action onCatchIssue = null)
    {
      //  CheckDataLogin(onCatchIssue);
        StartCoroutine(PopupLoginController.Init());
       // InitView();
    }

    /*
    private void InitView()
    {
        loginView.InitBtnTadaplay(() =>
        {
            var popupLoginModel = new PopupLoginModel(PopupLoginType.LoginTadaplay);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
        loginView.InitBtnGoogle(() =>
        {
            GoogleSignInController.OnGoogleClick();
        });
        loginView.InitBtnApple(() =>
        {
            AppleSignInController.LoginToApple();
        });
        loginView.InitBtnGuest(() =>
        {
            // Call Popup Warning
            var popupLoginModel = new PopupLoginModel(PopupLoginType.Warning);
            PopupLoginController.onShowPopup?.Invoke(popupLoginModel);
        });
        loginView.InitBtnLogout(() =>
        {
            LogOut();
        });        
        loginView.InitBtnPlay(() =>
        {
            // Loading Scene GamePlay + Disable View
            LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
            SetDisableView();
        });
    }
    #1#

    /*public void CheckDataLogin(Action onCatch = null)
    {
        var token = DBLoginController.LOGIN_RESULT.data.access_token;
        var isLogin = token != string.Empty;
        // Debug.Log(GetIsLogin());
        if (isLogin)
        {
            ScreenCoverLog.Show();
            ScreenCoverSync.Show();
            TadaplayAPI.SyncDataFormSever(token, (state, userData) =>
            {
                if (state)
                {
                    // Debug.Log($"c: {userData}");
                    JObject jsonObject = JObject.Parse(userData);
                    JToken payloadToken = jsonObject["data"]["payload"];
                    if (payloadToken != null)
                    {
                        var jsonData = JsonUtility.FromJson<PayLoad>(payloadToken.ToString());
                        // Apply Data To DBController
                        
                        DBController.Instance.ApplyData(jsonData);

                    }
                    else
                    {
                        Debug.LogError("can't not find payload");
                        PostData();
                    }
                    ScreenCoverSync.Hide();
                    ScreenCoverLog.Hide();
                }
                else
                {
                    PostData();
                }
            }, () =>
            {
                onCatch?.Invoke();
            }).Forget();
        }
        else
        {
            //DBController.Instance.USER_DATA = new UserData();
        }
        loginView.SetStatusLoginOption(isLogin);
    }#1#

    /*
    [ContextMenu("Post")]
    public void PostData()
    {
        var token = DBLoginController.LOGIN_RESULT.data.access_token;
        var isLogin = token != string.Empty;
        if (isLogin)
        {
            ScreenCoverSync.Show();
            var DB = DBController.Instance;
            var dataToPos = new DataToPos()
            {
                // Post Data
                game = TypeNameGame.BlockMerge.ToString(),
                balance = DB.USER_DATA.GetResourceByType(TypeResource.Score).amount,
                payload = new PayLoad()
                {
                    userData = DB.USER_DATA,
                    shopDB = DB.SHOP_DB,
                    SpinDataDB =  DB.SPIN_DATA,
                    //BoardDataDB =  DB.BOARD_DATA,
                    // userSettings = DB.USER_SETTINGS,
                    // achivementDB = DB.ACHIVEMENT_DB,
                    // databaseManager = DB.DATABASE_MANAGER,
                    // houseDataDB = DB.HOUSE_DATA,
                    // characterDataDB = DB.CHARACTER_DATA,
                    // spinDataDB = DB.SPIN_DATA,
                    // zoneDataDB = DB.ZONE_DATA,
                }
            };
            TadaplayAPI.PostData(token, dataToPos, () =>
            {
                ScreenCoverSync.Hide();
                ScreenCoverLog.Hide();
            }).Forget();
        }
    }
    #1#

    [ContextMenu("GetLeaderBoard")]
    public void GetDataLeaderBoard(Action<string> onComplete)
    {
        var token = DBLoginController.LOGIN_RESULT.data.access_token;
        var isLogin = token != string.Empty;
        if (isLogin)
        {
            ScreenCoverSync.Show();
            string value = string.Empty;
            TadaplayAPI.GetDataForLeaderBoard(token, (state, userData) =>
            {
                if (state)
                {
                    JObject jsonObject = JObject.Parse(userData);
                    JToken payloadToken = jsonObject["data"];
                    if (payloadToken != null)
                    {
                        value = payloadToken.ToString();
                    }
                    else
                    {
                        Debug.LogError("can't not find LeaderboardData");
                    }
                    ScreenCoverSync.Hide();
                }
                onComplete.Invoke(value);
            }).Forget();
        }
    }

    /*public bool GetIsLogin()
    {
        var token = DBLoginController.LOGIN_RESULT.data.access_token;
        var isLogin = token != string.Empty;
        return isLogin;
    }#1#

    public void SetStatusLoginOption()
    {
        loginView.SetDisableLogout();
    }

    public void SetLoginResult(LoginResult loginResult)
    {
        DBLoginController.LOGIN_RESULT.SetLoginResult(loginResult.data);
        //Debug.Log($"AccessToken SetLogin: {DBLoginController.LOGIN_RESULT.data.access_token}");
        // string json = PlayerPrefsExtend.GetString("LOGIN_RESULT");
        // Debug.Log($"Check Data: {json}");
    }

    /*public void LogOut()
    {
        Debug.Log("LogOut");
#if !UNITY_EDITOR || UNITY_WEBGL
        GoogleSignInController.LogoutGoolge();
#endif
        PlayerPrefsExtend.DeleteAll();
        DBLoginController.Initializing();
        DBController.Instance.ClearData();
        CheckDataLogin();
    }#1#
}
*/
