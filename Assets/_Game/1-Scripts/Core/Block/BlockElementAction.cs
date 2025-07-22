using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Game.Core.Board;
using Game.Ultis;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;


namespace Game.Core.Block
{
    [RequireComponent(typeof(RectTransform))]
    public class BlockElementAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler,
        IEndDragHandler
    {
        [SerializeField] private Canvas canvas;
        public UnityAction<PointerEventData> onPointerDown;
        public UnityAction<PointerEventData> onPointerUp;
        public UnityAction<PointerEventData> onDrag;
        public bool isPointerUp;

        public void OnPointerDown(PointerEventData eventData)
        {
            // Debug.Log($"OnPointerDown - {isPointerUp}");
            if (isPointerUp) return;
            canvas.sortingOrder = 1;
            AudioController.Instance.Play(AudioName.Sound_Click_Shape);
            onPointerDown?.Invoke(eventData);
            Vector3 wp = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
                Mathf.Abs(Camera.main.transform.position.z)));
            transform.position = wp;
            // Debug.Log($"OnPointerDown Block - {isPointerUp}");
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            // Debug.Log("OnPointerUp Block ---");
            if (isPointerUp) return;
            canvas.sortingOrder = 0;
            isPointerUp = true;
            onPointerUp?.Invoke(eventData);
            // Debug.Log("OnPointerUp Block");
        }

        public void OnDrag(PointerEventData eventData)
        {
            // Debug.Log("OnDrag Block---------");
            if (isPointerUp) return;
            onDrag?.Invoke(eventData);
            Vector3 wp = Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y,
                Mathf.Abs(Camera.main.transform.position.z)));
            var clampX = Mathf.Clamp(wp.x, InGameData.HorizontalClamp.x, InGameData.HorizontalClamp.y);
            var clampY = Mathf.Clamp(wp.y, InGameData.VerticalClamp.x, InGameData.VerticalClamp.y);
            transform.position = new Vector3(clampX, clampY, wp.z);
            // Debug.Log("OnDrag Block");
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Debug.Log("OnPointerUp Block ---");
            // if(isPointerUp) return;
            // isPointerUp = true;
            // onPointerUp?.Invoke(eventData);
            // Debug.Log("OnPointerUp Block");
        }
    }
}