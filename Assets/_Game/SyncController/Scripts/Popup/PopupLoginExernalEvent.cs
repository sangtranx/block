using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PopupBaseLogin))]
public class PopupLoginExernalEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent externalShow;
    [SerializeField] private UnityEvent externalShowComplete;
    [SerializeField] private UnityEvent externalHideComplete;
    [SerializeField] private UnityEvent externalHide;
    private PopupBaseLogin popupLoginBase;
    public PopupBaseLogin PopupLoginBase
    {
        get
        {
            if (popupLoginBase == null)
            {
                popupLoginBase = GetComponent<PopupBaseLogin>();
            }
            return popupLoginBase;
        }
    }

    private void Awake()
    {
        PopupLoginBase.externalShow += () => externalShow?.Invoke();
        PopupLoginBase.externalShowComplete += () => externalShowComplete?.Invoke();
        PopupLoginBase.externalHide += () => externalHide?.Invoke();
        PopupLoginBase.externalHideComplete += () => externalHideComplete?.Invoke();
    }
}
