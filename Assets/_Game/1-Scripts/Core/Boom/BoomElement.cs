using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Game.Core.Board;
using Game.Ultis;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

namespace Game.Core.Block
{
    public class BoomElement : BlockElement
    {
        [SerializeField] private List<BoomUnit> lstBoomUnit;

        private void OnDisable()
        {
            GameEvent.onShapeAutoMoveBoard -= OnShapeAutoMoveBoard;
        }

        protected override void OnPointerDown(PointerEventData eventData)
        {
            CancelMove();
            BlockElementMove.isSelected = true;
            var boardController = GameHelper.Instance.BoardController;
            var column = boardController.GetColumnByPos(transform.position.x);
            var cell = boardController.GetCellEmptyByColumn(column);
            var lineBlockControlelr = GameHelper.Instance.LineBlockController;
            var pos = boardController.GetPosByColumn(transform.position.x);
            lineBlockControlelr.DisableStatusBlockLine();
            lineBlockControlelr.EnableLineBlockByIdAndPos(pos, 0);
            if (cell == null) return;
            CalculateList(cell);
            EnableList(true);
        }

        protected override void OnDrag(PointerEventData eventData)
        {
            var boardController = GameHelper.Instance.BoardController;
            var column = boardController.GetColumnByPos(transform.position.x);
            var cell = boardController.GetCellEmptyByColumn(column);
            var lineBlockControlelr = GameHelper.Instance.LineBlockController;
            var pos = boardController.GetPosByColumn(transform.position.x);
            lineBlockControlelr.EnableLineBlockByIdAndPos(pos, 0);
            if (cell == null) return;
            ClearList();
            CalculateList(cell);
            EnableList(true);
        }


        protected override void OnPointerUp(PointerEventData eventData)
        {
            BlockElementMove.isSelected = false;
            var lineBlockControlelr = GameHelper.Instance.LineBlockController;
            lineBlockControlelr.DisableBlockLine();
            var boardController = GameHelper.Instance.BoardController;
            var column = boardController.GetColumnByPos(transform.position.x);
            var cell = boardController.GetCellEmptyByColumn(column);
            if (cell == null)
            {
                GameManager.Instance.Defeat();
                GameManager.Instance.StateGame = StateGame.Pause;
                return;
            }
            CalculateList(cell);
            boardController.ActionBoom(this, cell, lstCellOnDrag);
            ClearList();
        }

        public override async UniTask MoveToEndPos(Vector2 pos)
        {
            try
            {
                moveCancellationTokenSource = new CancellationTokenSource();
                await BlockElementMove.MoveEndPos(pos, timerMoveEndPos, moveCancellationTokenSource.Token, () =>
                {
                    BlockElementAction.isPointerUp = true;
                    BlockElementMove.isSelected = false;
                    AudioController.Instance.Play(AudioName.Sound_Click_Shape);
                    var boardController = GameHelper.Instance.BoardController;
                    var column = boardController.RandomColumn();
                    var cell = boardController.GetCellEmptyByColumn(column);
                    if (cell == null) return;
                    CalculateList(cell);
                    boardController.ActionBoom(this, cell, lstCellOnDrag);
                    GameEvent.onShapeAutoMoveBoard?.Invoke(cell.CellIndex.y);
                    ClearList();
                });
            }
            catch
            {
            }
        }

        protected override void OnShapeAutoMoveBoard(int index)
        {
            var boardController = GameHelper.Instance.BoardController;
            // if (currentCell.CellIndex.y == index)
            // {
            //     var column = boardController.GetColumnByPos(transform.position.x);
            //     var cell = boardController.GetCellEmptyByColumn(column);
            //     if (cell == null) return;
            //     ClearList();
            //     CalculateList(cell);
            //     EnableList(true);
            // }

            if (BlockElementMove.isSelected)
            {
                var column = boardController.GetColumnByPos(transform.position.x);
                var cell = boardController.GetCellEmptyByColumn(column);
                if (cell == null) return;
                ClearList();
                CalculateList(cell);
                EnableList(true);
            }
        }

        private List<Cell> lstCellOnDrag = new List<Cell>();
        private Cell currentCell;

        private void AddList(Cell cell)
        {
            if (!lstCellOnDrag.Contains(cell))
            {
                lstCellOnDrag.Add(cell);
            }
        }

        private void EnableList(bool status)
        {
            for (int i = 0; i < lstCellOnDrag.Count; i++)
            {
                lstCellOnDrag[i].ImgCellUse.gameObject.SetActive(status);
            }
        }


        private void ClearList()
        {
            EnableList(false);
            lstCellOnDrag.Clear();
        }


        private void CalculateList(Cell cell)
        {
            var boardController = GameHelper.Instance.BoardController;
            currentCell = cell;
            Debug.Log($"------------------");
            Debug.Log($"Cell Index: {cell.CellIndex}");
            for (int i = 0; i < lstBoomUnit.Count; i++)
            {
                var current = lstBoomUnit[i];
                var newIndex = new Vector2Int(cell.CellIndex.x + current.index.x, cell.CellIndex.y - current.index.y);
                var cellByIndex = boardController.GetCellByIndex(newIndex);
                if (cellByIndex != null)
                {
                    AddList(cellByIndex);
                }
            }
        }


        #region Builder

        public class Builder
        {
            private BoomElement boomElement;

            public Builder(BoomElement boomElement)
            {
                this.boomElement = boomElement;
            }


            public Builder SetTimerMoveBoard(float timerMoveBoard)
            {
                // boomElement.timerMoveEndPos = timerMoveBoard;
                return this;
            }

            public Builder SetTimerMoveEndPos(float timerMoveEndPos)
            {
                boomElement.timerMoveEndPos = timerMoveEndPos;
                return this;
            }

            public BoomElement Build()
            {
                boomElement.GetComponent();
                return boomElement;
            }
        }

        #endregion
    }
}