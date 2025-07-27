using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
namespace Data
{
    public class DBController : Singleton<DBController>
    {
        [SerializeField] private List<ShopSO> lstShopSO;
        [SerializeField] private SupportItemShopSO supportItemShopSO;
        public SupportItemShopSO SupportItemSO
        {
            get => supportItemShopSO;
            set
            {
                supportItemShopSO = value;
            }
        }
        #region VARIABLE
        [SerializeField] private UserData userData;
        public UserData USER_DATA
        {
            get => userData;
            set
            {
                userData = value;
                Save(DBKey.USER_DATA, userData);
            }
        }

        [SerializeField] private UserSettings userSettings;
        public UserSettings USER_SETTINGS
        {
            get => userSettings;
            set
            {
                userSettings = value;
                Save(DBKey.USER_SETTINGS, userSettings);
            }
        }

        [SerializeField] private ShopDB shopDB;
        public ShopDB SHOP_DB
        {
            get => shopDB;
            set
            {
                shopDB = value;
                Save(DBKey.SHOP_DB, shopDB);
            }
        }
        [SerializeField] SpinDataDB spinDB;
        public SpinDataDB SPIN_DATA
        {
            get => spinDB;
            set
            {
                spinDB = value;
                Save(DBKey.SPIN_DB, spinDB);
            }
        }


        #endregion
        protected override void CustomAwake()
        {
            Initializing();
        }

        private void OnDestroy()
        {
        }


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
                    PlayerPrefsExtend.SetString(key, json);
                }
                catch (UnityException e)
                {
                    throw new UnityException(e.Message);
                }
            }
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

        void Initializing()
        {
            CheckDependency(DBKey.USER_DATA, key =>
            {
                var userData = new UserData();
                USER_DATA = userData;
            });
            CheckDependency(DBKey.USER_SETTINGS, key =>
            {
                var userSettings = new UserSettings();
                USER_SETTINGS = userSettings;
            });
            CheckDependency(DBKey.SHOP_DB, key =>
            {
                var shopDB = new ShopDB(lstShopSO);
                SHOP_DB = shopDB;
            });
            CheckDependency(DBKey.SPIN_DB, key =>
            {
                var spin = new SpinDataDB();
                SPIN_DATA = spin;
            });
            Load();
        }

        void Load()
        {
            userData = LoadDataByKey<UserData>(DBKey.USER_DATA);
            userSettings = LoadDataByKey<UserSettings>(DBKey.USER_SETTINGS);
            shopDB = LoadDataByKey<ShopDB>(DBKey.SHOP_DB);
            spinDB = LoadDataByKey<SpinDataDB>(DBKey.SPIN_DB);
        }

        public void ClearData()
        {
            Initializing();
        }

    }

    public class DBKey
    {
        public static readonly string USER_DATA = "USER_DATA";
        public static readonly string USER_SETTINGS = "USER_SETTINGS";
        public const string SHOP_DB = "SHOP_DB";
        public const string SPIN_DB = "SPIN_DB";
        public const string BOARD_DATA = "BOARD_DATA";
    }
}




