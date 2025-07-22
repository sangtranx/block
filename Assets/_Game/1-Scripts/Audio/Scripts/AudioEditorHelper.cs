#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEditorHelper : ScriptableObject
{
    public List<AudioPlayerSource> lstSFXSources = new List<AudioPlayerSource>();
    public List<AudioPlayerSource> lstMusicSources = new List<AudioPlayerSource>();
}
#endif