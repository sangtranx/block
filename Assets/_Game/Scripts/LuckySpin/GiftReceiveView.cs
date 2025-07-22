using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GiftReceiveView : MonoBehaviour
{
    [SerializeField] protected Image imgRewardItem;
    [SerializeField] protected float showDuration;
    [SerializeField] protected float hideDuration;
    public virtual void SetView(Gift gift)
    {
        imgRewardItem.sprite = gift.GiftInfo.sprGift;
        imgRewardItem.preserveAspect = true;
    }
    public void SetActiveView(bool state, UnityAction onCompleted = default)
    {
        if (gameObject.activeInHierarchy != state)
        {
            if (state)
            {
                transform.localScale = Vector3.zero;
                gameObject.SetActive(state);
                transform.DOScale(Vector3.one, showDuration).SetEase(Ease.InSine).SetLink(gameObject)
                    .onComplete += () => onCompleted?.Invoke();
            }
            else
            {
                transform.localScale = Vector3.one;
                transform.DOScale(Vector3.zero, hideDuration).SetEase(Ease.OutSine).SetLink(gameObject).onComplete += () =>
                {
                    gameObject.SetActive(state);
                    onCompleted?.Invoke();
                };
            }
        }
    }
}
