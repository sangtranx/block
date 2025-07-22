#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioController))]
public class AudioControllerCustomEditor : Editor
{
    SerializedProperty isDontDestroyOnLoadSingleton;
    SerializedProperty defaultCapacityAudioPool;
    SerializedProperty audioPlayerPrefab;
    SerializedProperty lstSFXSourcesField;
    SerializedProperty lstMusicSourcesField;
    AudioController audioController;
    bool isValidMusicData = true;
    bool isValidSFXData = true;
    SerializedObject lstSoundHelper;
    static AudioEditorHelper audioEditorHelper;
    private void OnEnable()
    {
        #region LoadData
        #endregion
        audioController = target as AudioController;
        isDontDestroyOnLoadSingleton = serializedObject.FindProperty("dontDestroyOverload");
        defaultCapacityAudioPool = serializedObject.FindProperty("capacityAudioMultiPlaying");
        audioPlayerPrefab = serializedObject.FindProperty("audioPlayerPrefab");
        if (audioEditorHelper == null)
        {
            audioEditorHelper = ScriptableObject.CreateInstance<AudioEditorHelper>();
            foreach (AudioPlayerSource audioPlayerSource in audioController.LstAudioPlayerSource.FindAll((source) => source.typeAudio == TypeAudio.SFX))
            {
                audioEditorHelper.lstSFXSources.Add(audioPlayerSource);
            }
            foreach (AudioPlayerSource audioPlayerSource in audioController.LstAudioPlayerSource.FindAll((source) => source.typeAudio == TypeAudio.Music))
            {
                audioEditorHelper.lstMusicSources.Add(audioPlayerSource);
            }
        }
        lstSoundHelper = new SerializedObject(audioEditorHelper);
        lstSFXSourcesField = lstSoundHelper.FindProperty("lstSFXSources");
        lstMusicSourcesField = lstSoundHelper.FindProperty("lstMusicSources");
        isValidMusicData = audioEditorHelper.lstMusicSources.FindAll((source) => ((int)source.audioName)
        >= 0).Count == 0;
        isValidSFXData = audioEditorHelper.lstSFXSources.FindAll((source) => ((int)source.audioName)
        < 0).Count == 0;
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(isDontDestroyOnLoadSingleton);
        EditorGUILayout.PropertyField(audioPlayerPrefab);
        EditorGUILayout.PropertyField(defaultCapacityAudioPool);
        serializedObject.ApplyModifiedProperties();
        EditorGUILayout.Space(8);
        lstSoundHelper.Update();
        EditorGUILayout.PropertyField(lstMusicSourcesField);
        if (!isValidMusicData)
        {
            EditorGUILayout.HelpBox("Please Make Sure That All Music Name Are Valid!", MessageType.Error);
        }
        EditorGUILayout.Space(8);
        EditorGUILayout.PropertyField(lstSFXSourcesField);
        if (!isValidSFXData)
        {
            EditorGUILayout.HelpBox("Please Make Sure That All SFX Name Are Valid!", MessageType.Error);
        }
        lstSoundHelper.ApplyModifiedProperties();
        if (GUI.changed)
        {
            audioController.LstAudioPlayerSource.Clear();
            isValidMusicData = true;
            for (int i = 0; i < audioEditorHelper.lstMusicSources.Count; i++)
            {
                isValidMusicData = ((int)audioEditorHelper.lstMusicSources[i].audioName) < 0;
                if(isValidMusicData)
                {
                    var audioSource = audioEditorHelper.lstMusicSources[i];
                    audioSource.typeAudio = TypeAudio.Music;
                    audioController.LstAudioPlayerSource.Add(audioSource);
                }
            }
            isValidSFXData = true;
            for (int i = 0; i < audioEditorHelper.lstSFXSources.Count; i++)
            {
                isValidSFXData = ((int)audioEditorHelper.lstSFXSources[i].audioName) >= 0;
                if (isValidSFXData)
                {
                    var audioSource = audioEditorHelper.lstSFXSources[i];
                    audioSource.typeAudio = TypeAudio.SFX;
                    audioController.LstAudioPlayerSource.Add(audioSource);
                }
            }
            EditorUtility.SetDirty(audioController);
        }
    }
}
#endif