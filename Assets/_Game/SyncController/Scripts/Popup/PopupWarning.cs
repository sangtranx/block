using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupWarning : PopupBaseLogin
{
    [SerializeField] private Button btnOk;
    [SerializeField] private Button btnCancel;
    [SerializeField] private TextMeshProUGUI txtDescription;
    public override void InitPopup()
    {
        base.InitPopup();
        InitBtnOk();
        InitBtnCancel();
    }

    private void InitBtnOk()
    {
        btnOk.onClick.RemoveAllListeners();
        btnOk.onClick.AddListener(() =>
        {
            //Loading Scene To GamePlay
            
            LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
            //SyncController.Instance.SetDisableView();
            HidePopup();
        });
    }   
    
    private void InitBtnCancel()
    {
        btnCancel.onClick.RemoveAllListeners();
        btnCancel.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }

    public override void ShowPopup(PopupLoginModel popupLoginModel = null, UnityAction onShowComplete = null)
    {
        base.ShowPopup(popupLoginModel, onShowComplete);
    }
}
