using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Spinner : MonoBehaviour
{
    [SerializeField] float time2RotateOneDegreeFinal;
    [SerializeField] List<SpinPhase> lstSpinPhase;
    [SerializeField] RectTransform rtfmSpinner;
    [SerializeField] bool overClockwise;
    public void Spin2Angle(float targetAngle, UnityAction onCompletedSpin)
    {
        StartCoroutine(Spin2AngleAction(targetAngle, onCompletedSpin));
    }
    IEnumerator Spin2AngleAction(float targetAngle, UnityAction onCompletedSpin)
    {
        //Play spin audio
        for (int i = 0; i < lstSpinPhase.Count; i++)
        {
            yield return rtfmSpinner.DOLocalRotate(lstSpinPhase[i].angle * Vector3.forward, lstSpinPhase[i].angle * lstSpinPhase[i].time2RotateOneDegree, RotateMode.FastBeyond360)
                .SetLink(gameObject).SetEase(lstSpinPhase[i].ease).SetId(rtfmSpinner).WaitForCompletion();
        }
        yield return rtfmSpinner.DOLocalRotate(targetAngle * Vector3.forward, targetAngle * time2RotateOneDegreeFinal, RotateMode.FastBeyond360)
                .SetLink(gameObject).SetEase(Ease.OutSine).SetId(rtfmSpinner).WaitForCompletion();
        onCompletedSpin?.Invoke();
    }
    public void ResetAngle()
    {
        DOTween.Kill(rtfmSpinner);
        rtfmSpinner.localEulerAngles = Vector3.zero;
    }
}
[Serializable]
public struct SpinPhase
{
    public float time2RotateOneDegree;
    public int angle;
    public Ease ease;
}