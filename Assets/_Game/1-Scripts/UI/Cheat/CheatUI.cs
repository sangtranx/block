using Data;
using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CheatUI : MonoBehaviour
{
    private UserData userData => DBController.Instance.USER_DATA;
    [SerializeField] TMP_InputField inputAmount;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void CheatGold()
    {
        AudioController.Instance.Play(AudioName.Sound_AddCoin);
        Cheat(TypeResource.Coin);
    }
    public void CheatPoint()
    {
        Cheat(TypeResource.Point);
    }
    public void Defeat()
    {
        GameManager.Instance.Defeat();
    }
    public void Cheat(TypeResource type)
    {
        try
        {
            switch (type)
            {
                case TypeResource.Coin:
                    userData.AddResourceByType(TypeResource.Coin, long.Parse(inputAmount.text));
                    GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);

                    break;
                case TypeResource.Point:
                    userData.AddResourceByType(TypeResource.Point, long.Parse(inputAmount.text));
                    GameEvent.onUpdateUIByType?.Invoke(TypeResource.Point, userData.GetResourceByType(TypeResource.Point).amount);
                    GameEvent.onPointAdded?.Invoke();
                    break;
            }
        }
        catch { }
    }
    public void ResetAll()
    {
        foreach (TypeResource type in Enum.GetValues(typeof(TypeResource)))
        {
            if(type == TypeResource.Level)
            {
                userData.GetResourceByType(type).amount = 1;
            }
            else
            {
                userData.GetResourceByType(type).amount = 0;
            }
            userData.AddResourceByType(type, 0);
            GameEvent.onUpdateUIByType(type, userData.GetResourceByType(type).amount);
        }
    }
    public void Cancel()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.activeSelf);
    }
}
