using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLeaderBoardSO", menuName = "SO/LeaderBoardSO")]
public class ItemLeaderBoardSO : ScriptableObject
{
    public ItemLeaderBoard itemLeaderBoard;
    public ItemLeaderBoard itemLeaderBoardYour;
    public List<SprieLeaderBoard> lstSprLeaderBoardIcon;
    public SprieLeaderBoard GeSpriteByTop(int top) => lstSprLeaderBoardIcon.Find(ex => ex.top == top);

    public Sprite sprIconAvtDefault;
}

[Serializable]
public class SprieLeaderBoard
{
    public int top;
    public Sprite sprIcon;
}
