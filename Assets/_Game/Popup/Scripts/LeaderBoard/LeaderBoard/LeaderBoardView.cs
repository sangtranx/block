using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardView : MonoBehaviour
{
    [SerializeField] private ItemLeaderBoardSO itemLeaderBoardSO;
    [SerializeField] private RectTransform rtfmTop;
    [SerializeField] private RectTransform rtfmCurrent;
    [SerializeField] private RectTransform rtfmLeaderBoard;
    [SerializeField] private RectTransform rtfmDontLogin;
    [SerializeField] private ScrollRect scrollRect;
    public RectTransform RtfmTop { get => rtfmTop; }
    public RectTransform RtfmCurrent { get => rtfmCurrent; }
    public ScrollRect ScrollRect { get => scrollRect; }
    public ItemLeaderBoardSO ItemLeaderBoardSO { get => itemLeaderBoardSO; }

    public void CheckStatusLogin(bool isLogin)
    {
        rtfmLeaderBoard.gameObject.SetActive(isLogin);
        rtfmDontLogin.gameObject.SetActive(!isLogin);
    }
}
