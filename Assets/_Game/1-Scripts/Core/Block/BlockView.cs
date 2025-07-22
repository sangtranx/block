using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core.Board;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Core.Block
{
    public class BlockView : MonoBehaviour, IObjectPoolUser<BlockElement>
    {
        [SerializeField] private BlockElement blockElementPrefab;
        [SerializeField] private BoomElement boomElementPrefab;
        [SerializeField] private RectTransform rtfmParrent;
        [SerializeField] private BlockData blockData;
        [SerializeField] private Transform rtfmLeft;
        [SerializeField] private Transform rtfmRight;
        [SerializeField] private Transform rtfmTop;
        [SerializeField] private Transform rtfmBot;
        Pooler<BlockElement> blockElementPooler;
        private Vector2 startPos;
        private Vector2 endPos;
        private Vector2 size => InGameData.sizeBlock;
        private void Start()
        {
            blockElementPooler = new Pooler<BlockElement>(blockElementPrefab, rtfmParrent, this);
        }
        public void CalulatorPos()
        {
            startPos = new Vector2(rtfmLeft.position.x - 0.15f, rtfmLeft.position.y + 0.15f);
            InGameData.HorizontalClamp = new Vector2(rtfmLeft.position.x - 0.15f, rtfmRight.position.x + 0.4f);
            InGameData.VerticalClamp = new Vector2(rtfmBot.position.y, rtfmTop.position.y);
            endPos = new Vector2(rtfmRight.position.x, rtfmRight.position.y + 0.15f);
            Debug.Log($"Pos New: {startPos} - End: {endPos}");
        }

        public void GetFromPool(BlockElement obj)
        {
            obj.ResetPool();
            obj.gameObject.SetActive(true);
        }

        public void ReturnToPool(BlockElement obj)
        {
            obj.gameObject.SetActive(false);
        }

        public void SpawnBlockElement(float timerMoveEndPos, float timerMoveBoard, out BlockElement blockElement)
        {
            var randomData = blockData.RandomData();
            if (randomData == null)
            {
                blockElement = null;
                return;
            }
            blockElement = blockElementPooler.Pool.Get();
            blockElement.transform.position = startPos;
            var blockElementBuilder = new BlockElement.Builder(blockElement);
            blockElementBuilder.SetSizeBlock(size)
                .SetId(randomData.id)
                .SetSprite(randomData.sprBlock)
                .SetTimerMoveEndPos(timerMoveEndPos)
                .SetTimerMoveBoard(timerMoveBoard)
                .Build();
            blockElement.MoveToEndPos(endPos).Forget();
            blockElement.name = $"Block-{blockElement.Id}";
        }

        public void SpawnBoomElement(float timerMoveEndPos, float timerMoveBoard, out BoomElement boomElement)
        {
            boomElement = Instantiate(boomElementPrefab, rtfmParrent);
            boomElement.transform.position = startPos;
            var boomElementBuilder = new BoomElement.Builder(boomElement);
            boomElementBuilder.SetTimerMoveEndPos(timerMoveEndPos)
                .Build();
            boomElement.MoveToEndPos(endPos).Forget();
            // Debug.Log($"Size: {size}");
        }
    }
}