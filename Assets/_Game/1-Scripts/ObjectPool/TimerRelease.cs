using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerRelease : ReleaseActionBase<GameObject>
{
    [SerializeField] float timer2Release;
    float timer;
    private void OnEnable()
    {
        isReleased = false;
        timer = timer2Release;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            Release();
        }
    }
}
