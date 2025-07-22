
using UnityEngine;

public class AudioReleasePool : ReleaseActionBase<AudioPlayer>
{
    private void OnEnable()
    {
        isReleased = false;
    }
    private void Update()
    {
        if (!isReleased &&
            gameObject != null &&
            !gameObject.IsPlaying &&
            !gameObject.IsPause &&
            !AudioListener.pause)
        {
#if UNITY_EDITOR
            if(!UnityEditorInternal.InternalEditorUtility.isApplicationActive)
            {
                return;
            }
#endif
            Release();
        }
    }

    private void OnDisable()
    {
        if (!isReleased)
        {
            Release();
        }
    }
}
