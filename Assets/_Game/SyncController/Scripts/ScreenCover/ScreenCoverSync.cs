using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenCoverSync : ScreenCover
{
    [SerializeField] private Image imgSync;

    public override void Show()
    {
        // imgSync.gameObject.SetActive(true);
        // imgSync.rectTransform.DOLocalRotate(Vector3.forward * 360f, 0.5f, RotateMode.FastBeyond360).SetLoops(-1).SetId(this).SetLink(gameObject);
    }
    public override void Hide()
    {
        // DOTween.Kill(this);
        // imgSync.gameObject.SetActive(false);
    }
}
