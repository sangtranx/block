using Cysharp.Threading.Tasks;
using Data;
using DataLogin;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LeaderBoardView))]
public class LeaderBoardController : MonoBehaviour
{
    private LeaderBoardView leaderBoardView;

    public LeaderBoardView LeaderBoardView
    {
        get
        {
            if (leaderBoardView == null)
            {
                leaderBoardView = GetComponent<LeaderBoardView>();
            }
            return leaderBoardView;
        }
    }
    [SerializeField] private float timerMinutes = 15;
    private bool isLogin;
    private List<ItemLeaderBoard> lstItemLeaderBoard = new List<ItemLeaderBoard>();
    private void Start()
    {
        /*isLogin = SyncController.Instance.GetIsLogin();
        LeaderBoardView.CheckStatusLogin(isLogin);
        if (isLogin)
        {
            AutoSyncLeaderBoard().Forget();
        }*/
    }

    private async UniTaskVoid AutoSyncLeaderBoard()
    {
        var cacheTime = TimeSpan.FromMinutes(timerMinutes);
        while (true)
        {
            DestroyAll();
            GetData();
            await UniTask.Delay(cacheTime,cancellationToken:this.GetCancellationTokenOnDestroy());
        }
    }

    public void DestroyAll()
    {
        for (int i = lstItemLeaderBoard.Count - 1; i >= 0; i--)
        {
            var current = lstItemLeaderBoard[i];
            lstItemLeaderBoard.Remove(current);
            Destroy(current.gameObject);
        }
    }


    private void GetData()
    {
        /*SyncController.Instance.GetDataLeaderBoard((value) =>
        {
            var leadboardDatas = JsonUtility.FromJson<LeaderBoardDatas>(value);
            Debug.Log($"Lead: {value}");
            List<ItemLeaderBoardModel> lstItemLeaderBoardModel = new List<ItemLeaderBoardModel>();
            for (int i = 0; i < leadboardDatas.items.Count; i++)
            {
                var item = leadboardDatas.items[i];
                var itemModel = new ItemLeaderBoardModel()
                {
                    name = item.user.username,
                    score = item.balance,
                    sprAvt = LeaderBoardView.ItemLeaderBoardSO.sprIconAvtDefault,
                    number = i + 1,
                };
                lstItemLeaderBoardModel.Add(itemModel);
            }
            var me = new ItemLeaderBoardModel()
            {
                name = "Your",
                score = leadboardDatas.me.balance,
                sprAvt = LeaderBoardView.ItemLeaderBoardSO.sprIconAvtDefault,
                number = leadboardDatas.your_position,
            };
            Spawn(lstItemLeaderBoardModel, me);
        });*/
    }

    private void Spawn(List<ItemLeaderBoardModel> lstItemLeadrBoardModel, ItemLeaderBoardModel me)
    {
        for (int i = 0; i < lstItemLeadrBoardModel.Count; i++)
        {
            var itemModel = lstItemLeadrBoardModel[i];
            var currentTop = Instantiate(LeaderBoardView.ItemLeaderBoardSO.itemLeaderBoard, LeaderBoardView.RtfmTop);
            currentTop.Init(itemModel);
            var top = itemModel.number < 3 ? itemModel.number : 4;
            var spr = leaderBoardView.ItemLeaderBoardSO.GeSpriteByTop(top);
            currentTop.SetIconAvt(spr.sprIcon);
            lstItemLeaderBoard.Add(currentTop);
        }
        var currentModel = me;
        var current = Instantiate(LeaderBoardView.ItemLeaderBoardSO.itemLeaderBoardYour, LeaderBoardView.RtfmCurrent);
        current.Init(currentModel);
        var currentMe = currentModel.number < 3 ? currentModel.number : 4;
        var sprMe = leaderBoardView.ItemLeaderBoardSO.GeSpriteByTop(currentMe);
        current.SetIconAvt(sprMe.sprIcon);
        lstItemLeaderBoard.Add(current);
    }

    public void DelayPos()
    {
        StartCoroutine(DelayResetPos());
    }

    private IEnumerator DelayResetPos()
    {
        yield return null;
        leaderBoardView.ScrollRect.verticalNormalizedPosition = 1f;
    }
}
