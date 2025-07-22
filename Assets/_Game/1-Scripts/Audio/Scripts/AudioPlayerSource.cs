using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TypeAudioInstance
{
    Single,
    Multi,
}
public enum TypeAudio
{
    SFX,
    Music,
}
[System.Serializable]
public class AudioPlayerSource
{
    public TypeAudioInstance typeAudioInstance;
    [HideInInspector] public TypeAudio typeAudio;
    public AudioName audioName;
    public AudioClip audioClip;
    public bool isSpeedConstant = true;
}