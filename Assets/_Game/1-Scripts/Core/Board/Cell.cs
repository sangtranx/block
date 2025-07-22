using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Game.Core.Block;
namespace Game.Core.Board
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private Vector2Int index;
        [SerializeField] private Image imgCellUse;
        [SerializeField] private Image imgCell;
        [SerializeField] private float ratio;
        [SerializeField] private bool isVisited;
        [SerializeField] private int id;
        [SerializeField] private BlockElement currentBlockElement;
        [SerializeField] private RectTransform rtfmVFX;
        public Image ImgCellUse => imgCellUse;
        public int Id => id;
        private RectTransform rtfmCell;
        public Vector2Int CellIndex => index;

        public RectTransform RtfmVFX => rtfmVFX;

        public RectTransform RtfmCell
        {
            get
            {
                if (rtfmCell == null)
                {
                    rtfmCell = GetComponent<RectTransform>();
                }

                return rtfmCell;
            }
        }

        public bool IsVisited => isVisited;
        public void SetStatusIsVisit(bool status) => isVisited = status;
        public BlockElement CurrentBlockElement => currentBlockElement;

        public void ApplyBlock(BlockElement blockElement)
        {
            currentBlockElement = blockElement;
            // rtfmVFX.gameObject.SetActive(false);
        }

        public void SetCellSprite(Sprite sprite)
        {
            imgCell.sprite = sprite;
        }
        public float Ratio => ratio;

        #region Builder
        public class Builder
        {
            private Cell cell;

            public Builder(Cell cell)
            {
                this.cell = cell;
            }

            public Builder SetIndexCell(Vector2Int index)
            {
                cell.index = index;
                return this;
            }

            public Builder SetSpriteCell(Sprite sprCell)
            {
                cell.imgCell.sprite = sprCell;
                return this;
            }

            public Builder SetId(int id)
            {
                cell.id = id;
                return this;
            }

            public Cell Build()
            {
                return cell;
            }
        }

        #endregion
    }
}