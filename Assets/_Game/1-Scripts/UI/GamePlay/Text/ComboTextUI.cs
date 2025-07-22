using DG.Tweening;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboTextUI : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] Animator animator;
    public void Set(int value)
    {
        float time = 1f;
        if (value == 1)
        {
            animator.Play("good");
            time = 0.7f;
        }
        else if (value == 2)
        {
            animator.Play("great");
        }
        else if (value >= 3)
        {
            animator.Play("perfect");
        }
        //các text có kích thước (width + height) không giống nhau nên sẽ bị méo
        image.SetNativeSize();
        gameObject.transform.DOMoveY(transform.position.y + 0.3f, time).OnComplete(() =>
        {
            image.DOFade(0, 0.3f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        });
    }
}
