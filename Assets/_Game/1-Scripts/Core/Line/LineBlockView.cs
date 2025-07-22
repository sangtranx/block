using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Core.Line
{
    public class LineBlockView : MonoBehaviour
    {
        [SerializeField] private Image imgLine;
        [SerializeField] private Animator _animatorLine;
        [SerializeField] private List<LineModel> lstLineModel;

        private LineModel GetLineModelById(int id) => lstLineModel.Find(ex => ex.id == id);

        public void InitLineSize(float x)
        {
            imgLine.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
        }
        
        public void SetStatusLine(bool isStatus)
        {
            imgLine.gameObject.SetActive(isStatus);
        }

        public void PlayAnimationShot()
        {
            _animatorLine.Play("line_move");
        }

        public void SetColorLineById(int id)
        {
            // var lineModel = GetLineModelById(id);
            // if (lineModel == null) return;
            // imgLine.color = new Color(lineModel.color.r, lineModel.color.g, lineModel.color.b, imgLine.color.a);
        }

        public void SetPosLine(Vector2 pos)
        {
            var newPos = new Vector2(pos.x, imgLine.transform.position.y);
            imgLine.transform.position = newPos;
        }
    }

    [Serializable]
    public class LineModel
    {
        public int id;
        public Color color;
    }
}