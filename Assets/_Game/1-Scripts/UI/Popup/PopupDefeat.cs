using Cysharp.Threading.Tasks;
using Data;
using DG.Tweening;
using Game.Ultis;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PopupDefeat : PopupBase
{
  private DefeatController crtl;

  public DefeatController Ctrl
  {
    get
    {
      if (crtl == null)
      {
        crtl = GetComponent<DefeatController>();
      }

      return crtl;
    }
  }

  [SerializeField] private Button btnReplay;


  //===============================================
  /// <summary>
  /// Khi full % chest, sẽ show số vàng nhận được
  /// </summary>


  //================================================
  private UserData userData => DBController.Instance.USER_DATA;

  public override void InitPopup()
  {
    base.InitPopup();
    InitBtn();
  }


  private void InitBtn()
  {

    btnReplay.onClick.RemoveAllListeners();
    btnReplay.onClick.AddListener(() => Replay());
  }

  public override void ShowPopup(PopupModel popupModel = null, UnityAction onShowComplete = null)
  {
    btnReplay.gameObject.SetActive(true);
    base.ShowPopup(popupModel, onShowComplete);
    btnFadeClose.interactable = false;
    //Vì mỗi lần thua +chestPercent% nên trừ chestPercent% để lerp từ trước đó đến hiện tại
  }

  /// <summary>
  /// Tiến độ rương 
  /// </summary>
  public async void ShowResult()
  {
    float targetFillAmount = userData.GetResourceByType(TypeResource.DefeatChest).amount / 100f;
    string textValue = $"{userData.GetResourceByType(TypeResource.DefeatChest).amount}%";
  }

  void Home()
  {
    // HidePopup();
    LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
  }

  void Replay()
  {
    // HidePopup();
    LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
  }

  private async UniTask Animation()
  {
  }

  protected override void InitCloseFade()
  {
    btnFadeClose.onClick.RemoveAllListeners();
    btnFadeClose.onClick.AddListener(() => { Debug.Log("Replay"); });
  }
}