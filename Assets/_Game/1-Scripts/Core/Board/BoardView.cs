using System.Collections;
using System.Collections.Generic;
using Game.Ultis;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Board
{
    public class BoardView : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup gridLayout;
        [SerializeField] private Cell cellPrefabs;
        [SerializeField] private Sprite sprBoard;
        private SkinBlockController SkikBlockController => GameHelper.Instance.SkinBlockController;

        public void GenerateBoard(Vector2Int size, Vector2Int spacing, out Cell[,] cells)
        {
            var arrSpr = SkikBlockController.GetCurrentCellBoard();
            RectTransform rtfmParrent = gridLayout.GetComponent<RectTransform>();
            cells = new Cell[size.x, size.y];
            float cellSizeX = (rtfmParrent.rect.size.x - ((size.x + 1) * spacing.x)) / size.x;
            float cellSizeY = (rtfmParrent.rect.size.y - ((size.y + 1) * spacing.y)) / size.y;
            float ratio = cellPrefabs.Ratio;
            if (cellSizeX > cellSizeY)
            {
                cellSizeX = cellSizeY * ratio;
            }
            else
            {
                cellSizeY = cellSizeX / ratio;
            }

            var sizeCell = new Vector2(cellSizeX, cellSizeY);
            gridLayout.cellSize = sizeCell;
            gridLayout.spacing = spacing;
            InGameData.sizeBlock = sizeCell;
            Debug.Log($"Cell Size: {InGameData.sizeBlock}");
            for (int j = 0; j < cells.GetLength(1); j++)
            {
                for (int i = 0; i < cells.GetLength(0); i++)
                {
                    var cell = Instantiate(cellPrefabs, rtfmParrent);
                    cell.name = $"Cell: {i}-{j}";
                    var currenCell = new Cell.Builder(cell);
                    currenCell.SetIndexCell(new Vector2Int(i, j))
                        .SetSpriteCell(arrSpr[(i + j) % 2])
                        .SetId((i + j) % 2)
                        .Build();
                    cells[i, j] = cell;
                }
            }
        }
    }
}