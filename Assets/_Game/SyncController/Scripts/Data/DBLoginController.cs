using System;
using UnityEngine;
using UnityEngine.Events;
namespace DataLogin
{
    public class DBLoginController : MonoBehaviour
    {
        #region VARIABLE

        private LoginResult loginResult;
        public LoginResult LOGIN_RESULT
        {
            get => loginResult;
            set
            {
                loginResult = value;
                Save(DBKeyLogin.LOGIN_RESULT, value);
            }
        }

        #endregion
        void CheckDependency(string key, UnityAction<string> onComplete)
        {
            if (!PlayerPrefsExtend.HasKey(key))
            {
                onComplete?.Invoke(key);
            }
        }

        public void Save<T>(string key, T values)
        {

            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(bool) ||
                typeof(T) == typeof(string) ||
                typeof(T) == typeof(float) ||
                typeof(T) == typeof(long) ||
                typeof(T) == typeof(Quaternion) ||
                typeof(T) == typeof(Vector2) ||
                typeof(T) == typeof(Vector3) ||
                typeof(T) == typeof(Vector2Int) ||
                typeof(T) == typeof(Vector3Int))
            {
                PlayerPrefsExtend.SetString(key, values.ToString());
            }
            else
            {
                try
                {
                    string json = JsonUtility.ToJson(values);
                    // Debug.Log($"Key: {key} - json: {json}");
                    PlayerPrefsExtend.SetString(key, json);
                }
                catch (UnityException e)
                {
                    throw new UnityException(e.Message);
                }
            }
            // Debug.Log($"Save");
        }

        public T LoadDataByKey<T>(string key)
        {
            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(bool) ||
                typeof(T) == typeof(string) ||
                typeof(T) == typeof(float) ||
                typeof(T) == typeof(long) ||
                typeof(T) == typeof(Quaternion) ||
                typeof(T) == typeof(Vector2) ||
                typeof(T) == typeof(Vector3) ||
                typeof(T) == typeof(Vector2Int) ||
                typeof(T) == typeof(Vector3Int))
            {
                string stringValue = PlayerPrefsExtend.GetString(key);
                return (T)Convert.ChangeType(stringValue, typeof(T));
            }
            else
            {
                string json = PlayerPrefsExtend.GetString(key);
                // Debug.Log($"Key: {key} - Load Data: {json}");
                return JsonUtility.FromJson<T>(json);
            }
        }

        public void Delete(string key)
        {
            PlayerPrefsExtend.DeleteKey(key);
        }

        public void DeleteAll()
        {
            PlayerPrefsExtend.DeleteAll();
        }

        public void Initializing()
        {
            // Debug.Log("Init DBLogin");
            CheckDependency(DBKeyLogin.LOGIN_RESULT, key =>
            {
                var loginResult = new LoginResult();
                loginResult.data = new LoginData();
                LOGIN_RESULT = loginResult;
                // Debug.Log("CheckDependency");
            });
            Load();
        }

        void Load()
        {
            loginResult = LoadDataByKey<LoginResult>(DBKeyLogin.LOGIN_RESULT);
            // Debug.Log($"AccessToken Load: {loginResult.data.access_token}");
        }
    }

    public class DBKeyLogin
    {
        public const string LOGIN_RESULT = "LOGIN_RESULT";
    }
}




