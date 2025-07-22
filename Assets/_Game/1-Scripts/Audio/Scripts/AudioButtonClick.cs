using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioButtonClick : MonoBehaviour
{
    private Button button;

    public Button Button
    {
        get
        {
            if (button == null)
            {
                button = GetComponent<Button>();
            }

            return button;
        }
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        if(Button == null) yield break;
        Button.onClick.AddListener(() =>
        {
            AudioController.Instance.Play(AudioName.Sound_Click);
        });
    }
}
