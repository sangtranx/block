using System.Collections;
using System.Collections.Generic;
using Game.Core.Block;
using Game.Core.Board;
using Game.Core.Line;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Core
{
    public enum StateGame
    {
        Play,
        Pause,
    }

    public class GameManager : Singleton<GameManager>
    {
        [SerializeField] private StateGame stateGame;
        [SerializeField] private BoardController boardController;
        [SerializeField] private BlockController blockController;
        [SerializeField] private LineBlockController lineBlockController;
        public static int countReload;
        public StateGame StateGame
        {
            get => stateGame;
            set => stateGame = value;
        }

        private void Start()
        {
            Application.targetFrameRate = 60;
            boardController.Init();
            blockController.Init();
            lineBlockController.InitLineSize(InGameData.sizeBlock.x);
        }
        public void LevelUp(long bonusGold)
        {
            GameEvent.onLevelUp?.Invoke(bonusGold);
        }
        public void Defeat()
        {
            AudioController.Instance.Play(AudioName.Sound_Lose);
            GameEvent.onDefeat?.Invoke();
        }
    }
}