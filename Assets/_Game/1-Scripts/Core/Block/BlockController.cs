using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Data;
using Random = UnityEngine.Random;

namespace Game.Core.Block
{
    public class BlockController : MonoBehaviour
    {
        [SerializeField] private float deltaTimeSpawn;
        [SerializeField] private float timerMoveEndPos;
        [SerializeField] private float timerMoveBoard;
        private BlockView blockView;
        private List<BlockElement> lstBlockEmelent = new List<BlockElement>();

        public static Action<BlockElement> onRemoveBlockElement;
        private int boombCount;

        public static Action onAddBoom;

        private void Awake()
        {
            onRemoveBlockElement += OnRemoveBlockElement;
            onAddBoom += OnAddBoom;
        }

        private void OnDestroy()
        {
            onRemoveBlockElement -= OnRemoveBlockElement;
            onAddBoom -= OnAddBoom;
        }

        private void OnAddBoom()
        {
            boombCount++;
        }

        public void Init()
        {
            blockView = GetComponent<BlockView>();
            blockView.CalulatorPos();
            Spawn().Forget();
        }

        private void OnRemoveBlockElement(BlockElement blockElement)
        {
            lstBlockEmelent.Remove(blockElement);
        }

        private float GetTimer()
        {
            float timer = 0;
            var level = DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Level).amount;
            timer = Mathf.Clamp((timerMoveEndPos - (level / 3) * 0.2f * timerMoveEndPos), timerMoveEndPos / 2,
                timerMoveEndPos);
            return timer;
        }

        private async UniTaskVoid Spawn()
        {
            while (true)
            {
                if (GameManager.Instance.StateGame == StateGame.Pause)
                {
                    await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
                    continue;
                }

                await UniTask.WaitForSeconds(deltaTimeSpawn, cancellationToken: this.GetCancellationTokenOnDestroy());
                if (GameManager.Instance.StateGame == StateGame.Pause)
                {
                    await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
                    continue;
                }

                var randomPer = Random.Range(0, 101);
                if (randomPer < 95)
                {
                    blockView.SpawnBlockElement(GetTimer(), timerMoveBoard, out BlockElement blockElement);
                    lstBlockEmelent.Add(blockElement);
                    blockElement.gameObject.name = $"Block id: {blockElement.Id} - {lstBlockEmelent.Count}";
                }
                else
                {
                    blockView.SpawnBoomElement(GetTimer(), timerMoveBoard, out BoomElement boomElement);
                }

                // if (boombCount <= 0)
                // {
                //     blockView.SpawnBlockElement(timerMoveEndPos, timerMoveBoard, out BlockElement blockElement);
                //     lstBlockEmelent.Add(blockElement);
                //     blockElement.gameObject.name = $"Block id: {blockElement.Id} - {lstBlockEmelent.Count}";
                // }
                // else
                // {
                //     blockView.SpawnBoomElement(timerMoveEndPos, timerMoveBoard, out BoomElement boomElement);
                //     boombCount--;
                //     if (boombCount <= 0) boombCount = 0;
                // }
            }
        }
    }
}