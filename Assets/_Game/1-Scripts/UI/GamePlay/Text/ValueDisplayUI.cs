using DG.Tweening;
using System;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class ValueDisplayUI : MonoBehaviour
{
    double updateUIByTypeOldValue = 0;
    long bonusGoldOldValue = 0;

    private void Start()
    {
        GameEvent.onResourceGonnaChange += SetStartValue;
        GameEvent.onBonusGoldGonnaChange += SetBonusGoldOldValue;
    }
    private void OnDestroy()
    {
        GameEvent.onResourceGonnaChange -= SetStartValue;
        GameEvent.onBonusGoldGonnaChange -= SetBonusGoldOldValue;
    }
    void SetStartValue(long value)
    {
        updateUIByTypeOldValue = value;
    }
    void SetBonusGoldOldValue(long value)
    {
        bonusGoldOldValue = value;
    }
    public void LerpValue(long value, TextMeshProUGUI txt, string title, string endTitle = "", float time = 0.4f, Action onComplete = null)
    {
        string _title = string.Empty;
        if (title == "+")
        {
            _title = "+";
        }
        else if (title == "x")
        {
            _title = "x";
        }
        else if (title.Length > 0)
        {
            _title = $"{title}: ";
        }
        //UnityEngine.Debug.Log($"{title} | updateUIByTypeOldValue = {updateUIByTypeOldValue}");
        Lerp(updateUIByTypeOldValue, value, txt, title);
        void Lerp(double curr, double value, TextMeshProUGUI text, string textTitle)
        {
            DOVirtual.Float((float)curr, (float)value, time, (v) => text.text =
            $"{_title}{v.ToString("N0")}{endTitle}").OnComplete(() =>
            {
                onComplete?.Invoke();
            }).SetLink(gameObject);
        }
    }
    public void LerpPoint(long value, TextMeshProUGUI txt)
    {
      
        float time = 0.4f;
        DOVirtual.Float((float)updateUIByTypeOldValue, value, time, (v) => txt.text =
            $"Point: {v.ToString("N0")}/{LevelCalculator.Instance.GetMaxPoint().ToString("N0")}").SetLink(gameObject); ; //hậu tố min/max
    }
    public void LerpPoint(long start, long value, TextMeshProUGUI txt)
    {
        UnityEngine.Debug.Log($"Point | updateUIByTypeOldValue = {updateUIByTypeOldValue}");
        float time = 0.4f;
        DOVirtual.Float((float)start, value, time, (v) => txt.text =
            $"Point: {v.ToString("N0")}/{LevelCalculator.Instance.GetMaxPoint().ToString("N0")}").SetLink(gameObject); ; //hậu tố min/max
    }
    public void LerpBonusGold(long value, TextMeshProUGUI txt, Action onComplete = null)
    {
        float time = 2f;
        DOVirtual.Float((float)updateUIByTypeOldValue, value, time, (v) => txt.text = $"+{v.ToString("N0")}")
            .OnComplete(() =>
            {
                onComplete?.Invoke();
            }).SetLink(gameObject);
    }
}