using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypeGift
{
    Coin,
    Bom3x3,
    Rocket,
    Fire,
    Thunder
}

[Serializable]
public class Gift
{
    [SerializeField] TypeGift typeGift;
    [SerializeField] int amount;
    [SerializeField] GiftInfo giftInfo;
    public TypeGift TypeGift { get => typeGift; }
    public int Amount { get => amount; }
    public GiftInfo GiftInfo { get => giftInfo; }

    public void SetAmount(int amount) => this.amount = amount;
}
