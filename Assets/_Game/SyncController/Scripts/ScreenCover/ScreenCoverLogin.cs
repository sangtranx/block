using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScreenCoverLogin : ScreenCover
{
    [SerializeField] private RectTransform rtfm;
    [SerializeField] private TextMeshProUGUI txt;
    private string[] arrTxt = new string[]
    {
        ".","..","..."
    };

    private IEnumerator AnimationTxt()
    {
        int i = 0;
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            txt.text = arrTxt[i];
            if (i > arrTxt.Length) i = 0;
        }
    }

    public override void Show()
    {
        rtfm.gameObject.SetActive(true);
        // StartCoroutine(AnimationTxt());
    }

    public override void Hide()
    {
        // StopAllCoroutines();
        rtfm.gameObject.SetActive(false);
    }
}
