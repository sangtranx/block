using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;
public static class PlayerPrefsExtend
{
    public static void DeleteKey(string key)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        PlayerPrefs.DeleteKey(key);
#else
        RemoveFromLocalStorage(key);
#endif
    }

    public static bool HasKey(string key)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        return PlayerPrefs.HasKey(key);
#else
        return (HasKeyInLocalStorage(key) == 1);
#endif
    }

    public static string GetString(string key)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        return (PlayerPrefs.GetString(key));

#else
        return LoadFromLocalStorage(key);
#endif
    }

    public static void SetString(string key, string value)
    {
#if UNITY_EDITOR || !UNITY_WEBGL
        PlayerPrefs.SetString(key, value);
#else
        SaveToLocalStorage(key, value);
#endif

    }

    public static void Save()
    {
#if !UNITY_WEBGL
        PlayerPrefs.Save();
#endif
    }
    public static void DeleteAll()
    {
#if !UNITY_WEBGL
        PlayerPrefs.DeleteAll();
#endif
    }
#if UNITY_WEBGL
      [DllImport("__Internal")]
      private static extern void SaveToLocalStorage(string key, string value);

      [DllImport("__Internal")]
      private static extern string LoadFromLocalStorage(string key);

      [DllImport("__Internal")]
      private static extern void RemoveFromLocalStorage(string key);

      [DllImport("__Internal")]
      private static extern int HasKeyInLocalStorage(string key);
#endif
}
