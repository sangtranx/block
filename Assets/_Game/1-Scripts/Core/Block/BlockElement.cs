using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Board;
using Game.Helpers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Game.Ultis;
using UnityEditor;

namespace Game.Core.Block
{
    public class BlockElement : MonoBehaviour
    {
        [SerializeField] private TypeEffect typeEffect;
        [SerializeField] protected RectTransform rtfmBlockElement;
        [SerializeField] private Image imgBlock;
        [SerializeField] private int id;
        protected BlockElementMove _blockElementMove;
        protected BlockElementAction _blockElementAction;
        public int Id => id;

        public Image ImgBlock
        {
            get => imgBlock;
        }

        public BlockElementMove BlockElementMove
        {
            get
            {
                if (_blockElementMove == null)
                {
                    _blockElementMove = GetComponent<BlockElementMove>();
                }

                return _blockElementMove;
            }
        }

        public BlockElementAction BlockElementAction
        {
            get
            {
                if (_blockElementAction == null)
                {
                    _blockElementAction = GetComponent<BlockElementAction>();
                }

                return _blockElementAction;
            }
        }

        protected float timerMoveEndPos;
        protected CancellationTokenSource moveCancellationTokenSource;

        protected virtual void GetComponent()
        {
            BlockElementAction.onPointerDown += OnPointerDown;
            BlockElementAction.onPointerUp += OnPointerUp;
            BlockElementAction.onDrag += OnDrag;
            GameEvent.onShapeAutoMoveBoard += OnShapeAutoMoveBoard;
        }

        public void ResetPool()
        {
            BlockElementAction.isPointerUp = false;
            BlockElementMove.isSelected = false;
            BlockElementMove.countPlay = 0;
            BlockElementAction.onPointerDown -= OnPointerDown;
            BlockElementAction.onPointerUp -= OnPointerUp;
            BlockElementAction.onDrag -= OnDrag;
            GameEvent.onShapeAutoMoveBoard -= OnShapeAutoMoveBoard;
        }


        private void OnDisable()
        {
            CancelMove();
        }

        private void OnDestroy()
        {
            GameEvent.onShapeAutoMoveBoard -= OnShapeAutoMoveBoard;
            CancelMove();
        }

        protected virtual void OnPointerDown(PointerEventData eventData)
        {
            BlockElementMove.isSelected = true;
            CancelMove();
            var boardController = GameHelper.Instance.BoardController;
            var lineBlockControlelr = GameHelper.Instance.LineBlockController;
            var pos = boardController.GetPosByColumn(transform.position.x);
            lineBlockControlelr.DisableStatusBlockLine();
            lineBlockControlelr.EnableLineBlockByIdAndPos(pos, id);

            var column = boardController.GetColumnByPos(transform.position.x);
            var cell = boardController.GetCellEmptyByColumn(column);
            cacheColumn = column;
            CalculateList(cell);
            EnableList();
        }

        private int cacheColumn = 0;

        protected virtual void OnPointerUp(PointerEventData eventData)
        {
            ClearList();
            var lineBlockControlelr = GameHelper.Instance.LineBlockController;
            lineBlockControlelr.DisableBlockLine();
            var boardController = GameHelper.Instance.BoardController;
            boardController.ApplyBlockToCell(transform.position.x, this, out Cell cell);
            Debug.Log($"Pointer Up: {gameObject.name}");
            if (cell == null) return;
            // Debug.Log($"Name: {gameObject.name} - {cell.CellIndex}");
            // MoveToBoard(cell,timerMoveBoard);
        }

        protected virtual void OnShapeAutoMoveBoard(int index)
        {
            var boardController = GameHelper.Instance.BoardController;
            if (BlockElementMove.isSelected)
            {
                var column = boardController.GetColumnByPos(transform.position.x);
                if (cacheColumn != column)
                {
                    cacheColumn = column;
                    var cell = boardController.GetCellEmptyByColumn(cacheColumn);
                    if (cell == null) return;
                    ClearList();
                    CalculateList(cell);
                    EnableList();
                }
            }
            // if (currentCells.CellIndex.y == index)
            // {
            //     var column = boardController.GetColumnByPos(transform.position.x);
            //     var cell = boardController.GetCellEmptyByColumn(column);
            //     if (cell == null) return;
            //     ClearList();
            //     CalculateList(cell);
            //     EnableList(true);
            // }
        }

        protected virtual void OnDrag(PointerEventData eventData)
        {
            // transform.DOKill(this);
            var boardController = GameHelper.Instance.BoardController;
            var lineBlockControlelr = GameHelper.Instance.LineBlockController;
            var pos = boardController.GetPosByColumn(transform.position.x);
            lineBlockControlelr.EnableLineBlockByIdAndPos(pos, id);
            
            var column = boardController.GetColumnByPos(transform.position.x);
            if (cacheColumn != column)
            {
                ClearList();
                cacheColumn = column;
                var cell = boardController.GetCellEmptyByColumn(cacheColumn);
                if (cell == null) return;
                CalculateList(cell);
                EnableList();
            }
        }

        private List<Cell> lstCellOnVFX = new List<Cell>();
        private Cell currentCells;

        private void AddList(Cell cell)
        {
            if (!lstCellOnVFX.Contains(cell))
            {
                lstCellOnVFX.Add(cell);
            }
        }

        private void EnableList()
        {
            bool canMerge = lstCellOnVFX.Count >= 3 ? true : false;
            for (int i = 0; i < lstCellOnVFX.Count; i++)
            {
                if (lstCellOnVFX[i].RtfmVFX.gameObject.activeInHierarchy != canMerge)
                {
                    lstCellOnVFX[i].RtfmVFX.gameObject.SetActive(canMerge);
                }
            }
        }

        private void ClearList()
        {
            currentCells = null;
            for (int i = 0; i < lstCellOnVFX.Count; i++)
            {
                lstCellOnVFX[i].RtfmVFX.gameObject.SetActive(false);
            }

            lstCellOnVFX.Clear();
        }

        private void CalculateList(Cell cell)
        {
            var boardController = GameHelper.Instance.BoardController;
            var lstCell = new List<Cell>();
            var lstCellCheck = new List<Cell>();
            boardController.ShowListCanMerge(cell, this, ref lstCell, ref lstCellCheck);
            Debug.Log(($"Cell Count: {lstCell.Count}"));
            for (int i = 0; i < lstCellCheck.Count; i++)
            {
                lstCellCheck[i].SetStatusIsVisit(false);
            }

            currentCells = cell;
            for (int i = 0; i < lstCell.Count; i++)
            {
                AddList(lstCell[i]);
            }
        }

        public virtual async UniTask MoveToEndPos(Vector2 pos)
        {
            try
            {
                moveCancellationTokenSource = new CancellationTokenSource();
                BlockElementMove.MoveEndPos(pos, timerMoveEndPos, moveCancellationTokenSource.Token, () =>
                {
                    BlockElementAction.isPointerUp = true;
                    BlockElementMove.isSelected = false;
                    AudioController.Instance.Play(AudioName.Sound_Click_Shape);
                    var boardController = GameHelper.Instance.BoardController;
                    boardController.ApplyRandomBlockToCell(this, out Cell cell);
                    Debug.Log($"Random - Name: {gameObject.name} - {cell.CellIndex}");
                    if (cell == null) return;
                    // // MoveToBoard(cell,timerMoveBoard);
                });
            }
            catch
            {
            }
        }

        protected void CancelMove()
        {
            if (moveCancellationTokenSource != null)
            {
                moveCancellationTokenSource.Cancel();
                moveCancellationTokenSource.Dispose();
                moveCancellationTokenSource = null;
            }
        }

        public virtual async UniTask MoveToBoard(Cell cell)
        {
            // Debug.Log(($"Current: {cell.gameObject.name}"));
            await BlockElementMove.MoveToBoard(cell, GetTimerToMove(cell), () =>
            {
                GameEvent.onShapeAutoMoveBoard?.Invoke(cell.CellIndex.y);
                // Debug.Log("Move Complete");
            });
        }

        private float GetTimerToMove(Cell cell)
        {
            float timer = 0f;
            var dis = Vector2.Distance(cell.transform.position, transform.position);
            timer = Mathf.Clamp(dis / 10f, 0.1f, 0.5f);
            return timer;
        }

        public async UniTask AnimationMerge(TypeEffect typeEffect)
        {
            Debug.Log("Animation Merge");
            PlayAudioByType(this.typeEffect);
            var obj = GameHelper.Instance.EffectPooler.GetObjectPooled(typeEffect);
            obj.transform.position = this.transform.position;
            gameObject.SetActive(false);
            // await transform.DOScale(Vector3.zero, timer).OnComplete(() =>
            // {
            //     BlockController.onRemoveBlockElement?.Invoke(this);
            //     // Destroy(gameObject);
            // });
        }

        private void PlayAudioByType(TypeEffect typeEffect)
        {
            switch (typeEffect)
            {
                case TypeEffect.Boom:
                    AudioController.Instance.Play(AudioName.Sound_Boom);
                    break;
            }
        }


        #region Builder

        public class Builder
        {
            private BlockElement blockElement;

            public Builder(BlockElement blockElement)
            {
                this.blockElement = blockElement;
            }

            public Builder SetId(int id)
            {
                blockElement.id = id;
                return this;
            }

            public Builder SetSprite(Sprite spr)
            {
                blockElement.imgBlock.sprite = spr;
                return this;
            }

            public Builder SetSizeBlock(Vector2 size)
            {
                blockElement.rtfmBlockElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
                blockElement.rtfmBlockElement.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);
                return this;
            }

            public Builder SetTimerMoveBoard(float timerMoveBoard)
            {
                // blockElement.timerMoveBoard = timerMoveBoard;
                return this;
            }

            public Builder SetTimerMoveEndPos(float timerMoveEndPos)
            {
                blockElement.timerMoveEndPos = timerMoveEndPos;
                return this;
            }

            public BlockElement Build()
            {
                blockElement.GetComponent();
                return blockElement;
            }
        }

        #endregion
    }
}