using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Data;
using DG.Tweening;
using Game.Ultis;
using Cysharp.Threading.Tasks;

public enum TypeUIUpdate
{
    Gold = 0,
    Score = 1,
    Level = 2,
    Point = 3,
}

public class GamePlayUI : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnShop;
    [SerializeField] private Button btnLeaderboard;
    [SerializeField] private Button btnPlayGame;
    [SerializeField] private Button btnSpin;
    [SerializeField] private Button btnRewardGoldADS;
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI txtCoin;
    [SerializeField] private TextMeshProUGUI txtLevel;
    /// <summary>
    /// Tổng điểm
    /// </summary>
    [SerializeField] private TextMeshProUGUI txtScore;
    /// <summary>
    /// Điểm level hiện tại
    /// </summary>
    [SerializeField] private TextMeshProUGUI txtPoint;
    [Header("Image")]
    [SerializeField] Image expFill;
    private UIPooler pooler => GameHelper.Instance.Poolers.UIPooler;
    private UserData userData => DBController.Instance.USER_DATA;

    //spin
    [Header("Imgage icon")]
    [SerializeField] private Image imgCoin;
    [SerializeField] private Image imgBoom;
    [SerializeField] private Image imgRocket;
    [SerializeField] private Image imgFire;
    [SerializeField] private Image imgThunder;
    private void Start()
    {
        InitBtnSettings();
        GameEvent.onUpdateUIByType += UpdateUIByType;
        GameEvent.onLerpUpdateUIByType += LerpUpdateUIByType;
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Coin, userData.GetResourceByType(TypeResource.Coin).amount);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Score, userData.GetResourceByType(TypeResource.Score).amount);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Level, userData.GetResourceByType(TypeResource.Level).amount);
        GameEvent.onUpdateUIByType?.Invoke(TypeResource.Point, userData.GetResourceByType(TypeResource.Point).amount);
    }

    private void OnDestroy()
    {
        GameEvent.onUpdateUIByType -= UpdateUIByType;
        GameEvent.onLerpUpdateUIByType -= LerpUpdateUIByType;
    }

    public void InitBtnSettings()
    {
        btnSettings.onClick.RemoveAllListeners();
        btnSettings.onClick.AddListener(() =>
        {
            var popupReplay = new PopupModel(PopupType.Settings);
            GameEvent.onShowPopup?.Invoke(popupReplay);
        });

        btnShop.gameObject.SetActive(false);
        btnShop.onClick.RemoveAllListeners();
        btnShop.onClick.AddListener(() =>
        {
            var popupReplay = new PopupModel(PopupType.Shop);
            //popupReplay.coin = 100;
            GameEvent.onShowPopup?.Invoke(popupReplay);
        });

        btnLeaderboard.onClick.RemoveAllListeners();
        btnLeaderboard.onClick.AddListener(() =>
        {
            var popupReplay = new PopupModel(PopupType.LeaderBoard);
            GameEvent.onShowPopup?.Invoke(popupReplay);
        });

        btnRewardGoldADS.onClick.RemoveAllListeners();
        btnRewardGoldADS.onClick.AddListener(() =>
        {
            var popupReplay = new PopupModel(PopupType.RewardCoin);
            GameEvent.onShowPopup?.Invoke(popupReplay);
        });
        btnSpin.onClick.RemoveAllListeners();
        btnSpin.onClick.AddListener(() =>
        {
            var popupReplay = new PopupModel(PopupType.Spin);
            GameEvent.onShowPopup?.Invoke(popupReplay);
        });
    }
    /// <summary>
    /// Set image fill in exp bar
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetExpBarFill()
    {
        long max = LevelCalculator.Instance.GetMaxPoint();
        long min = userData.GetResourceByType(TypeResource.Point).amount;
        expFill.DOFillAmount((float)min / max, 1f);
    }
    private void UpdateUIByType(TypeResource typeUIUpdate, long value)
    {
        switch (typeUIUpdate)
        {
            case TypeResource.Coin:
                GameHelper.Instance.ValueDisplayUI.LerpValue(value, txtCoin, "");
                break;
            case TypeResource.Score:
                GameHelper.Instance.ValueDisplayUI.LerpValue(value, txtScore, "Score");
                break;
            case TypeResource.Level:
                GameHelper.Instance.ValueDisplayUI.LerpValue(value, txtLevel, "Level");
                break;
            case TypeResource.Point:
                GameHelper.Instance.ValueDisplayUI.LerpPoint(value, txtPoint);
                SetExpBarFill();
                break;
        }
    }
    private void LerpUpdateUIByType(TypeResource typeUIUpdate, long start, long end)
    {
        switch (typeUIUpdate)
        {
            case TypeResource.Point:
                GameHelper.Instance.ValueDisplayUI.LerpPoint(start, end, txtPoint);
                SetExpBarFill();
                break;
        }
    }
    //=======
    public async UniTaskVoid FlyAnimationByType(TypeResource typeResource, int value, Vector3 tfmStart)
    {
        // var count = Mathf.RoundToInt(value * 1f / 2);
        var totalClam = Mathf.Clamp(value, 0, 10);
        for (int i = 0; i < totalClam; i++)
        {
            var obj = pooler.GetObjectPooled(typeResource);
            obj.transform.position = tfmStart;
            switch (typeResource)
            {
                case TypeResource.Coin:
                    Fly(typeResource, obj, tfmStart, imgCoin.transform.position);
                    break;
                case TypeResource.Bom3x3:
                    Fly(typeResource, obj, tfmStart, imgBoom.transform.position);
                    break;
                case TypeResource.Rocket:
                    Fly(typeResource, obj, tfmStart, imgRocket.transform.position);
                    break;
                case TypeResource.Fire:
                    Fly(typeResource, obj, tfmStart, imgFire.transform.position);
                    break;
                case TypeResource.Thunder:
                    Fly(typeResource, obj, tfmStart, imgThunder.transform.position);
                    break;
            }
            await UniTask.WaitForSeconds(0.1f, cancellationToken: this.GetCancellationTokenOnDestroy());
        }
    }

    private void Fly(TypeResource typeResource, GameObject gameObject, Vector3 posStart, Vector3 posEnd)
    {
        var endPos = posEnd;
        Vector3[] pos = new Vector3[3];
        pos[0] = posStart;
        pos[1] = randomMidPos(transform.position, endPos, 0.5f, new Vector2(-1f, 1f));
        pos[2] = endPos;
        gameObject.transform.DOScale(Vector3.one * 0.8f, 0.5f);
        gameObject.transform.DOPath(pos, 0.5f, PathType.CatmullRom).OnComplete(() =>
        {
            pooler.GetPooler(typeResource).Pool.Release(gameObject);
        }).SetLink(gameObject);
    }

    private Vector3 randomMidPos(Vector3 startPos, Vector3 endPos, float rangePos, Vector2 Range)
    {
        var Pos = Vector2.Lerp(startPos, endPos, rangePos);
        var random = Random.Range(Range.x, Range.y);
        return new Vector3(Pos.x, Pos.y + random, 0);
    }

}