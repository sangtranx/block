using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Game.Core.Board;
using UnityEngine.Events;

namespace Game.Core.Block
{
    public class BlockElementMove : MonoBehaviour
    {
        public bool isSelected;

        public async UniTask MoveEndPos(Vector2 endPos, float timer, CancellationToken cancellationToken,
            UnityAction onComplete = null)
        {
            // await transform.DOMove(endPos, timer).OnComplete(() => { onComplete?.Invoke(); }).SetId(this);

            var startPosition = transform.position;
            var endPosition = new Vector3(endPos.x, endPos.y, 0);
            var elapsedTime = 0f;
            try
            {
                while (elapsedTime < timer)
                {
                    if (GameManager.Instance.StateGame == StateGame.Pause)
                    {
                        await UniTask.Yield(cancellationToken);
                        continue;
                    }

                    elapsedTime += Time.deltaTime;
                    var t = Mathf.Clamp01(elapsedTime / timer);
                    transform.position = Vector3.Lerp(startPosition, endPosition, t);
                    await UniTask.Yield(cancellationToken);
                }

                onComplete?.Invoke();
            }
            catch
            {
            }
        }

        public int countPlay = 0;

        public async UniTask MoveToBoard(Cell cell, float timer, UnityAction onComplete = null)
        {
            isSelected = false;
            var startPosition = transform.position;
            var endPosition = new Vector3(cell.transform.position.x, cell.transform.position.y, 0);
            var elapsedTime = 0f;
            while (elapsedTime < timer)
            {
                if (GameManager.Instance.StateGame == StateGame.Pause)
                {
                    await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
                    continue;
                }

                elapsedTime += Time.deltaTime;
                var t = Mathf.Clamp01(elapsedTime / timer);
                transform.position = Vector3.Lerp(startPosition, endPosition, t);
                await UniTask.Yield(cancellationToken: this.GetCancellationTokenOnDestroy());
            }

            if (countPlay <= 0)
            {
                AudioController.Instance.Play(AudioName.Sound_OnPlace);
                countPlay++;
            Debug.Log("Onplace");
            }

            onComplete?.Invoke();
            // await transform.DOMove(cell.transform.position, timer).OnComplete(() => { onComplete?.Invoke(); })
            //     .SetId(this);
        }
    }
}