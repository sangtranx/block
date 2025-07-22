using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class SpinDataDB
{
    public int spinAmount;
    public long lastTickSaved2GetFree;
}
[Serializable]
public static class SpinData
{
    public static int SPIN_AMOUNT
    {
        get
        {
            return DBController.Instance.SPIN_DATA.spinAmount;
        }
        set
        {
            DBController.Instance.SPIN_DATA.spinAmount = value;
            DBController.Instance.SPIN_DATA = DBController.Instance.SPIN_DATA;
        }
    }
    public static long LAST_TICK_SAVED_2_GET_FREE
    {
        get
        {
            return DBController.Instance.SPIN_DATA.lastTickSaved2GetFree;
        }
        set
        {
            DBController.Instance.SPIN_DATA.lastTickSaved2GetFree = value;
            DBController.Instance.SPIN_DATA = DBController.Instance.SPIN_DATA;
        }
    }
}
public static class SpinConfig
{
    public static int MAX_STOCK = 1;
}