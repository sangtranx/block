using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataLoginMobile
{
}

#region Login Request
[Serializable]
public class LoginGoogleRequest
{
    public string code;

    public LoginGoogleRequest(string code)
    {
        this.code = code;
    }
}

[Serializable]
public class LoginAppleRequest
{
    public string token;

    public LoginAppleRequest(string token)
    {
        this.token = token;
    }
}
#endregion
#region  Login Mobile Data
[Serializable]
public class LoginMobileResult
{
    public string access_token;
    public string refresh_token;
    public LoginMobileData user;
}

[Serializable]
public class LoginMobileData
{
    public string _id;
    public string name;
    public string email;
    public string username;
    public object telegram_id;
    public object avatar;
    public object banner;
    public bool is_active;
    public object password;
    public bool is_email_verified;
    public string invite_code;
    public string login_with_driver;
    public int balance;
    public string titles;
    public List<object> wallets;
    public bool is_online;
    public DateTime created_at;
    public DateTime updated_at;
    public bool is_use_password;
    public string id;
}
#endregion
