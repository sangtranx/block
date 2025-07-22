using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableRelease : ReleaseActionBase<GameObject>
{
    private void OnEnable()
    {
        isReleased = false;
    }
    private void OnDisable()
    {
        Release();
    }
}
