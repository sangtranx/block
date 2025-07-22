using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCalculator : Singleton<LevelCalculator>
{
    private UserData userData => DBController.Instance.USER_DATA;
    private Dictionary<int /*Level*/, (int /*exp need*/, long /*gold reward*/)> levels;
    private void Start()
    {
        InitLevelData();
        GameEvent.onPointAdded += CheckLevelUp;
    }
    public void InitLevelData()
    {
        //Tính exp cần để lên cấp
        levels = new Dictionary<int, (int, long)>();
        levels.Add(1, (0, 0));
        levels.Add(2, (10000, 500));
        for (int i = 2; i < 20; i++)
        {
            int expNeed = levels[i].Item1 + (2000 / 4 * i);
            long goldReward = levels[i].Item2 + 1000;
            levels.Add(i + 1, (expNeed, goldReward));
        }
        //foreach(var x in levels)
        //{
        //    Debug.Log($"Level {x.Key}: {(x.Value.Item1).ToString("N0")} - {(x.Value.Item2).ToString("N0")}$");
        //}
        //===============================
    }
    private void OnDestroy()
    {
        GameEvent.onPointAdded -= CheckLevelUp;
    }
    void CheckLevelUp()
    {
        int crrLevel = (int)userData.GetResourceByType(TypeResource.Level).amount;
        if (crrLevel >= 20) Debug.LogError($"Max max max {crrLevel}");
        
        int crrExp = (int)userData.GetResourceByType(TypeResource.Point).amount;
        if (crrExp >= levels[crrLevel + 1].Item1 / 2 && !userData.isClaimInLevel)
        {
            var popupReplay = new PopupModel(PopupType.Spin);
            GameEvent.onShowPopup?.Invoke(popupReplay);
            userData.SetClaimInLevel(true);
        }
        if (crrExp >= levels[crrLevel+1].Item1) //Đủ point để level up, chỉ lên tối đa 1 cấp mỗi lần, không nhảy cấp khi exp đủ để đạt một lúc nhiều cấp
        {
            GameEvent.onLevelUp?.Invoke(levels[crrLevel + 1].Item2); //lấy vàng của cấp mới đạt được
            userData.SetClaimInLevel(false);
        }
    }

    public long GetMaxPoint()
    {
        //levels có thể bị null nếu hàm này được gọi trước khi Class chạy Start()
        if (levels == null) InitLevelData();
        int crrLevel = (int)userData.GetResourceByType(TypeResource.Level).amount;
        return levels[crrLevel+1].Item1;
    }
}
