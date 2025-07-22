using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SubShop : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private RectTransform rtfmParrent;
    [SerializeField] private ScrollRect scrollRect;
    public TextMeshProUGUI TxtName { get => txtName; }
    public RectTransform RtfmParrent { get => rtfmParrent; }
    public ScrollRect ScrollRect { get => scrollRect; }

    public void DelayReset()
    {
        StartCoroutine(DelayResetPos());
    }

    private IEnumerator DelayResetPos()
    {
        yield return null;
        scrollRect.horizontalNormalizedPosition = 0f;
    }
}
