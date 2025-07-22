using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DataLoginTadaplay
{
    
}

#region  Login Tadaplay ID
[Serializable]
public class LoginRequest
{
    public string email;
    public string password;
}

[Serializable]
public class LoginData
{
    public HubData user;
    public string access_token;
    public string refresh_token;
}

[Serializable]
public class LoginResult
{
    public LoginData data;

    public void SetLoginResult(LoginData data)
    {
        this.data = data;
        //SyncController.Instance.DBLoginController.LOGIN_RESULT = this;
    }
}
#endregion
#region  Tadaplay Data User
[Serializable]
public class HubData
{
    public object invite_code;
    public int balance;
    public string titles;
    public List<object> wallets;
    public bool is_online;
    public string _id;
    public string name;
    public string email;
    public string username;
    public string telegram_id;
    public string avatar;
    public string banner;
    public bool is_active;
    public string password;
    public object email_verified_at;
    public DateTime created_at;
    public DateTime updated_at;
    public string about;
    public bool is_use_password;
    public string id;
}
#endregion
#region  Register Tadaplay ID
[Serializable]
public class RegisterRequest
{
    public string email;
    public string name;
    public string username;
    public string password;
    public string confirm_password;
}

[Serializable]
public class RegisterResult
{
    public string message;
    public string statusCode;
    public HubData data;

    public RegisterResult(string message, string statusCode, HubData data)
    {
        this.message = message;
        this.statusCode = statusCode;
        this.data = data;
    }
}
#endregion
#region  ForgotPassword
[Serializable]
public class ForgotPasswordRequest
{
    public string email;
}

[Serializable]
public class VerifyOTPRequest
{
    public string email;
    public string code;
}

[Serializable]
public class VerityOTPRespone
{
    public string statusCode;
    public VerityOTPData data;
}

[Serializable]
public class VerityOTPData
{
    public string email;
    public string token;
}

[Serializable]
public class UpdatePasswordRequest
{
    public string email;
    public string token;
    public string new_password;
}
#endregion