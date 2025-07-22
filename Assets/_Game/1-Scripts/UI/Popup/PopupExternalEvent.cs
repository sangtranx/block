using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PopupExternalEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent externalShow;
    [SerializeField] private UnityEvent externalShowComplete;
    [SerializeField] private UnityEvent externalHide;
    [SerializeField] private UnityEvent externalHideComplete;    
    private PopupBase popupBase;
    public PopupBase PopupBase
    {
        get
        {
            if (popupBase == null)
            {
                popupBase = GetComponent<PopupBase>();
            }
            return popupBase;
        }
    }

    private void Awake()
    {
        PopupBase.externalShow += () => externalShow?.Invoke();        
        PopupBase.externalShowComplete += () => externalShowComplete?.Invoke();
        PopupBase.externalHide += () => externalHide?.Invoke();
        PopupBase.externalHideComplete += () => externalHideComplete?.Invoke();

    }
}
