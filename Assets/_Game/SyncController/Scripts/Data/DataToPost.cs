using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Data;

public enum TypeNameGame
{
    BlockMerge = 0,
}

[Serializable]
public class DataToPos
{
    public string game;
    public double balance;
    public PayLoad payload;
}

[Serializable]
public class PayLoad
{
    public UserData userData;
    public ShopDB shopDB;
    public SpinDataDB SpinDataDB;
    //public BoardDataDB BoardDataDB;
}

[Serializable]
public class LeaderBoardDatas
{
    public List<LeaderBoardData> items;
    public LeaderBoardData me;
    public int your_position;
}

[Serializable]
public class LeaderBoardData
{
    public string _id;
    public string game;
    public double balance;
    public HubData user;
    public string id;
}
