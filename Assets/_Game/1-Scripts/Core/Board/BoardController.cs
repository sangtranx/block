using Cysharp.Threading.Tasks;
using Data;
using Game.Core.Block;
using Game.Helpers;
using Game.Ultis;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Core.Board
{
    [RequireComponent(typeof(BoardView))]
    public class BoardController : SerializedMonoBehaviour
    {
        [SerializeField] private Vector2Int size;
        [SerializeField] private Vector2Int spacing;
        private float[] arrPos;
        private BoardView boardView;
        private Cell[,] cells;

        [TableMatrix(HorizontalTitle = "X axis", VerticalTitle = "Y axis")] [SerializeField]
        private int[,] boardBlock;

        public void Init()
        {
            InGameData.spacing = spacing;
            boardView = GetComponent<BoardView>();
            boardView.GenerateBoard(size, spacing, out Cell[,] cells);
            this.cells = cells;
            boardBlock = new int[cells.GetLength(0), cells.GetLength(1)];
            StartCoroutine(SetColumnPos());
            GetBoardData();
        }

        [Button("Get Data Board")]
        public void GetBoardData()
        {
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    var id = cells[i, j].CurrentBlockElement == null ? -1 : cells[i, j].CurrentBlockElement.Id;
                    boardBlock[i, j] = id;
                }
            }
        }


        [Button("Get Data Board")]
        private IEnumerator SetColumnPos()
        {
            yield return null;
            arrPos = new float[cells.GetLength(0)];
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                var currentColumn = cells[i, 0];
                arrPos[i] = currentColumn.RtfmCell.transform.position.x;
            }
        }

        public int GetColumnByPos(float x)
        {
            int columnIndex = -1;
            var min = Mathf.Infinity;
            for (int i = 0; i < arrPos.Length; i++)
            {
                var currentColumn = arrPos[i];
                float delta = Mathf.Abs(x - currentColumn);
                if (delta < min)
                {
                    min = delta;
                    columnIndex = i;
                }
            }

            // Debug.Log($"Current: {x} - column: {columnIndex}");
            return columnIndex;
        }

        public int RandomColumn()
        {
            return Random.Range(0, cells.GetLength(0));
        }

        public Vector2 GetPosByColumn(float x)
        {
            var column = GetColumnByPos(x);
            if (column > cells.GetLength(1))
            {
                Debug.LogError($"Out Rangee - {column}");
                return Vector2.zero;
            }

            return cells[column, 0].transform.position;
        }

        public Cell GetCellEmptyByColumn(int column)
        {
            if (column > cells.GetLength(1))
            {
                Debug.LogError($"Out Rangee - {column}");
                return null;
            }

            for (int i = 0; i < cells.GetLength(1); i++)
            {
                var currentCell = cells[column, i];
                if (currentCell.CurrentBlockElement == null)
                {
                    return currentCell;
                }
            }

            Debug.Log("Loseeee");
            return null;
        }

        private void SortCellInColumn(int column, ref List<Func<UniTask>> lstFunc, ref List<int> lstBreak)
        {
            if (column > cells.GetLength(1))
            {
                Debug.LogError($"Out Rangee - {column}");
            }

            for (int i = 0; i < cells.GetLength(1); i++)
            {
                var current = cells[column, i];
                if (current.CurrentBlockElement != null) continue;
                for (int j = i + 1; j < cells.GetLength(1); j++)
                {
                    var next = cells[column, j];
                    if (next.CurrentBlockElement != null)
                    {
                        current.ApplyBlock(next.CurrentBlockElement);
                        next.ApplyBlock(null);
                        var block = current.CurrentBlockElement;
                        AddListRef(-1, async () => await block.MoveToBoard(current), ref lstFunc, ref lstBreak);
                        break;
                    }
                }
            }

            GetBoardData();
        }

        private void AddScore(int score, int combo, Vector2 pos)
        {
            if (score <= 0) return;
            GameEvent.onResourceGonnaChange?.Invoke(0);
            GameHelper.Instance.ComboTextController.Spawn(combo);
            GameHelper.Instance.PointFloatingController.Spawn(score, pos);
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Point, score);
            DBController.Instance.USER_DATA.AddResourceByType(TypeResource.Score, score);
            GameEvent.onLerpUpdateUIByType?.Invoke(TypeResource.Point,
                DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Point).amount - score,
                DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Point).amount);
            GameEvent.onUpdateUIByType?.Invoke(TypeResource.Score,
                DBController.Instance.USER_DATA.GetResourceByType(TypeResource.Score).amount);
            GameEvent.onPointAdded?.Invoke();
        }

        private void AddListRef(int id, Func<UniTask> func, ref List<Func<UniTask>> lstFunc, ref List<int> lstId)
        {
            if (!lstFunc.Contains(func))
            {
                lstId.Add(id);
                lstFunc.Add(func);
            }
        }

        private void InsertList(int id, int index, Func<UniTask> func, ref List<Func<UniTask>> lstFunc,
            ref List<int> lstId)
        {
            if (!lstFunc.Contains(func))
            {
                lstId.Insert(1, id);
                lstFunc.Insert(1, func);
            }
        }

        public void ApplyBlockToCell(float x, BlockElement block, out Cell cell)
        {
            var indexColumn = GetColumnByPos(x);
            if (indexColumn < 0)
            {
                Debug.LogError($"Pos: {x} - Buggg");
            }

            cell = GetCellEmptyByColumn(indexColumn);
            if (cell == null)
            {
                Debug.LogError("Losee");
                GameManager.Instance.Defeat();
                GameManager.Instance.StateGame = StateGame.Pause;
                return;
            }

            // Debug.Log("Apply");
            cell.ApplyBlock(block);
            ActionMerge(cell);
            GetBoardData();
        }


        public async void ActionBoom(BlockElement block, Cell cell, List<Cell> lstCell)
        {
            var lstAnimation = new List<Func<UniTask>>();
            var lstAwaitAnimation = new List<int>();
            var lstColumnCheck = new List<int>();
            var blockCountBoom = 0;
            var blockCount = 0;
            AddListRef(1, async () => await block.MoveToBoard(cell), ref lstAnimation, ref lstAwaitAnimation);
            AddListRef(-1, async () => await block.AnimationMerge(TypeEffect.Boom), ref lstAnimation,
                ref lstAwaitAnimation);
            for (int i = 0; i < lstCell.Count; i++)
            {
                var currentCell = lstCell[i];
                if (currentCell.CurrentBlockElement == null) continue;
                var blockInCell = currentCell.CurrentBlockElement;
                AddListRef(-1, () => blockInCell.AnimationMerge(TypeEffect.BoomMerge), ref lstAnimation,
                    ref lstAwaitAnimation);
                blockCountBoom++;
                if (!lstColumnCheck.Contains(currentCell.CellIndex.x))
                {
                    lstColumnCheck.Add(currentCell.CellIndex.x);
                }

                currentCell.ApplyBlock(null);
            }

            var scoreBoom = blockCountBoom * 50;
            Debug.Log($"Score: {scoreBoom}");
            AddListRef(-1, async () => await UpdateScore(scoreBoom, 0, cell.transform.position),
                ref lstAnimation,
                ref lstAwaitAnimation);
            for (int i = 0; i < lstColumnCheck.Count; i++)
            {
                SortCellInColumn(lstColumnCheck[i], ref lstAnimation,
                    ref lstAwaitAnimation);
            }

            for (int i = 0; i < lstColumnCheck.Count; i++)
            {
                for (int j = 0; i < cells.GetLength(1); j++)
                {
                    var currentCellInColumn = cells[lstColumnCheck[i], j];
                    if (currentCellInColumn.CurrentBlockElement == null) break;
                    CheckMergeAll(currentCellInColumn, ref lstAnimation,
                        ref lstAwaitAnimation, ref blockCount);
                }
            }

            var score = 0;
            var combo = 0;
            if (blockCount >= 4)
            {
                combo = blockCount / 4;
                score = (combo - 1) * 3 * 50 + 1 * 3 * 50 * (combo + 1) + blockCount % 4 * 50;
                InsertList(-1, 1, async () => await PlaySound(), ref lstAnimation, ref lstAwaitAnimation);
            }

            InsertList(-1, 2, async () => await UpdateScore(score, combo, cell.transform.position),
                ref lstAnimation,
                ref lstAwaitAnimation);
            for (int i = 0; i < lstAnimation.Count; i++)
            {
                if (lstAwaitAnimation[i] > 0)
                {
                    await lstAnimation[i].Invoke();
                    // await UniTask.WaitForSeconds(0.05f);
                }
                else
                {
                    lstAnimation[i].Invoke();
                }
            }
        }

        private async void ActionMerge(Cell cell)
        {
            var lstAnimation = new List<Func<UniTask>>();
            var lstAwaitAnimation = new List<int>();
            var block = cell.CurrentBlockElement;
            var blockCount = 0;
            var oneBlockScore = 25;
            AddListRef(1, async () => await block.MoveToBoard(cell), ref lstAnimation, ref lstAwaitAnimation);
            CheckMergeAll(cell, ref lstAnimation, ref lstAwaitAnimation, ref blockCount);
            var combo = 0;
            var score = 25;
            if (blockCount >= 4)
            {
                combo = blockCount / 4;
                score = (combo - 1) * 3 * 25 + 1 * 3 * 25 * (combo + 1) + blockCount % 4 * 25;
                AddListRef(1, async () => await PlaySound(), ref lstAnimation, ref lstAwaitAnimation);
                Debug.Log($"Combo: {combo} - BlockCount: {blockCount}");
            }

            InsertList(-1, 1, async () => await UpdateScore(score, combo, cell.transform.position),
                ref lstAnimation,
                ref lstAwaitAnimation);
            for (int i = 0; i < lstAnimation.Count; i++)
            {
                if (lstAwaitAnimation[i] > 0)
                {
                    await lstAnimation[i].Invoke();
                }
                else
                {
                    lstAnimation[i].Invoke();
                }
            }
            // await UniTask.WaitForSeconds(0.25f);
            // for (int i = 0; i < lstAnimationColumn.Count; i++)
            // {
            //     var currentColumn = lstAnimationColumn[i];
            //     var currentAwait = lstAwaitAnimationMergeColumn[i];
            //     for (int j = 0; j < currentColumn.Count; j++)
            //     {
            //         if (currentAwait[i] == 1)
            //         {
            //             await currentColumn[j].Invoke();
            //         }
            //         else if (currentAwait[i] == -1)
            //         {
            //             currentColumn[j].Invoke();
            //         }
            //     }
            // }
        }

        private async UniTask PlaySound()
        {
            // AudioController.Instance.Play(AudioName.Sound_Clear);
        }

        private async UniTask UpdateScore(int score, int combo, Vector2 pos)
        {
            AddScore(score, combo, pos);
            await UniTask.CompletedTask;
        }

        public void ApplyRandomBlockToCell(BlockElement block, out Cell cell)
        {
            var indexColumn = Random.Range(0, cells.GetLength(0));
            cell = GetCellEmptyByColumn(indexColumn);
            if (cell == null)
            {
                Debug.LogError("Losee");
                GameManager.Instance.Defeat();
                GameManager.Instance.StateGame = StateGame.Pause;
                return;
            }

            cell.ApplyBlock(block);
            ActionMerge(cell);
            GetBoardData();
        }

        public Cell GetCellByIndex(Vector2Int index)
        {
            try
            {
                return cells[index.x, index.y];
            }
            catch
            {
                // Debug.LogError($"OutRange ---- {index}");
                return null;
            }
        }

        public Vector2Int[] GetVector2Neighbor(Vector2Int index)
        {
            var arrIndex = new Vector2Int[4];
            var indexLeft = new Vector2Int(index.x - 1, index.y);
            var indexRight = new Vector2Int(index.x + 1, index.y);
            var indexTop = new Vector2Int(index.x, index.y + 1);
            var indexBottom = new Vector2Int(index.x, index.y - 1);
            arrIndex[0] = indexLeft;
            arrIndex[1] = indexRight;
            arrIndex[2] = indexTop;
            arrIndex[3] = indexBottom;
            return arrIndex;
        }

        private void CheckMergeAll(Cell cell, ref List<Func<UniTask>> lstFuncAnimation, ref List<int> lstBreak,
            ref int countBlockMerge)
        {
            Debug.Log("Check Merge");
            var lstCellCanMerge = new List<Cell>();
            var lstCellCheck = new List<Cell>();
            var lstColumnCheck = new List<int>();
            CheckCanMerge(cell, ref lstCellCanMerge, ref lstCellCheck);
            string debug = $"Count: {lstCellCanMerge.Count}";
            var isCanMerge = lstCellCanMerge.Count >= 4;
            for (int i = 0; i < lstCellCanMerge.Count; i++)
            {
                debug +=
                    $"\n{lstCellCanMerge[i].gameObject.name} - {lstCellCanMerge[i].CurrentBlockElement.Id}  - {lstCellCanMerge[i].CellIndex}";
                var current = lstCellCanMerge[i];
                var block = current.CurrentBlockElement;
                current.SetStatusIsVisit(false);
                if (i == 0)
                {
                    AddListRef(1, async () => await block.MoveToBoard(current), ref lstFuncAnimation, ref lstBreak);
                }

                if (isCanMerge)
                {
                    if (i != lstCellCanMerge.Count - 1)
                    {
                        AddListRef(-1, () => block.AnimationMerge(TypeEffect.Merge), ref lstFuncAnimation,
                            ref lstBreak);
                    }
                    else
                    {
                        AddListRef(1, () => block.AnimationMerge(TypeEffect.Merge), ref lstFuncAnimation, ref lstBreak);
                    }

                    current.ApplyBlock(null);
                    if (!lstColumnCheck.Contains(current.CellIndex.x))
                    {
                        lstColumnCheck.Add(current.CellIndex.x);
                    }

                    countBlockMerge++;
                }
            }

            for (int i = 0; i < lstCellCheck.Count; i++)
            {
                lstCellCheck[i].SetStatusIsVisit(false);
            }

            if (isCanMerge)
            {
                for (int i = 0; i < lstColumnCheck.Count; i++)
                {
                    SortCellInColumn(lstColumnCheck[i], ref lstFuncAnimation,
                        ref lstBreak);
                }

                for (int i = 0; i < lstColumnCheck.Count; i++)
                {
                    for (int j = 0; i < cells.GetLength(1); j++)
                    {
                        var currentCellInColumn = cells[lstColumnCheck[i], j];
                        if (currentCellInColumn.CurrentBlockElement == null) break;
                        CheckMergeAll(currentCellInColumn, ref lstFuncAnimation,
                            ref lstBreak, ref countBlockMerge);
                    }
                }
            }

            Debug.Log(debug);
            GetBoardData();
        }

        public void ShowCellCanMerge(Cell cell, BlockElement blockElement, out List<Cell> lstCellCanMerge)
        {
            lstCellCanMerge = new List<Cell>();
            lstCellCanMerge.Add(cell);
            var arrIndex = GetVector2Neighbor(cell.CellIndex);
            Debug.Log($"Count ArrCheck: {arrIndex.Length}");
            for (int i = 0; i < arrIndex.Length; i++)
            {
                // Debug.Log("ssss");
                var currentIndex = arrIndex[i];
                var currentCellByIndex = GetCellByIndex(currentIndex);
                if (currentCellByIndex == null || currentCellByIndex.CurrentBlockElement == null ||
                    currentCellByIndex.IsVisited) continue;
                currentCellByIndex.SetStatusIsVisit(true);
                if (blockElement.Id == currentCellByIndex.CurrentBlockElement.Id)
                {
                    lstCellCanMerge.Add(currentCellByIndex);
                    Debug.Log("ssssss");
                }
            }
        }


        public void ShowListCanMerge(Cell cell, BlockElement blockElement, ref List<Cell> lstCellCanMerge,
            ref List<Cell> lstCellCheck)
        {
            var arrIndex = GetVector2Neighbor(cell.CellIndex);
            for (int i = 0; i < arrIndex.Length; i++)
            {
                var currentIndex = arrIndex[i];
                var currentCellByIndex = GetCellByIndex(currentIndex);
                if (currentCellByIndex == null || currentCellByIndex.CurrentBlockElement == null ||
                    currentCellByIndex.IsVisited) continue;
                currentCellByIndex.SetStatusIsVisit(true);
                if (blockElement.Id == currentCellByIndex.CurrentBlockElement.Id)
                {
                    ShowListCanMerge(currentCellByIndex, blockElement, ref lstCellCanMerge, ref lstCellCheck);
                    AddList(currentCellByIndex, lstCellCanMerge);
                }

                AddList(currentCellByIndex, lstCellCheck);
            }

            void AddList(Cell cell, List<Cell> lstCellBoard)
            {
                if (!lstCellBoard.Contains(cell))
                {
                    lstCellBoard.Add(cell);
                }
            }
        }

        private void CheckCanMerge(Cell cell, ref List<Cell> lstCellCanMerge, ref List<Cell> lstCell)
        {
            AddList(cell, lstCellCanMerge);
            AddList(cell, lstCell);
            var arrIndex = GetVector2Neighbor(cell.CellIndex);
            for (int i = 0; i < arrIndex.Length; i++)
            {
                // Debug.Log("ssss");
                var currentIndex = arrIndex[i];
                var currentCellByIndex = GetCellByIndex(currentIndex);
                if (currentCellByIndex == null || currentCellByIndex.CurrentBlockElement == null ||
                    currentCellByIndex.IsVisited) continue;
                currentCellByIndex.SetStatusIsVisit(true);
                if (cell.CurrentBlockElement.Id == currentCellByIndex.CurrentBlockElement.Id)
                {
                    CheckCanMerge(currentCellByIndex, ref lstCellCanMerge, ref lstCell);
                }

                AddList(currentCellByIndex, lstCell);
            }

            void AddList(Cell cell, List<Cell> lstCellBoard)
            {
                if (!lstCellBoard.Contains(cell))
                {
                    lstCellBoard.Add(cell);
                }
            }
        }
    }
}