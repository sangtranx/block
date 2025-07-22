using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor.UI;
#region Editor
[CustomEditor(typeof(TimerButton))]
public class TimerButtonEditor : ButtonEditor
{
    SerializedProperty ticksSaveKey;
    SerializedProperty txtTimer;
    SerializedProperty isAutoSaveTicksWhenClick;
    SerializedProperty secondsTimer;
    SerializedProperty txtSetText;
    protected override void OnEnable()
    {
        base.OnEnable();
        ticksSaveKey = serializedObject.FindProperty(nameof(ticksSaveKey));
        txtTimer = serializedObject.FindProperty(nameof(txtTimer));
        isAutoSaveTicksWhenClick = serializedObject.FindProperty(nameof(isAutoSaveTicksWhenClick));
        secondsTimer = serializedObject.FindProperty(nameof(secondsTimer));
        txtSetText = serializedObject.FindProperty(nameof(txtSetText));
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(ticksSaveKey);
        EditorGUILayout.PropertyField(secondsTimer);
        EditorGUILayout.PropertyField(txtTimer);
        EditorGUILayout.PropertyField(isAutoSaveTicksWhenClick);
        EditorGUILayout.PropertyField(txtSetText);
        serializedObject.ApplyModifiedProperties();
        base.OnInspectorGUI();
    }
}
#endregion
#endif
public class TimerButton : Button
{
    [SerializeField] string ticksSaveKey;
    [SerializeField] float secondsTimer;
    [SerializeField] TextMeshProUGUI txtTimer;
    [SerializeField] string txtAtFirst;
    [SerializeField] bool isAutoSaveTicksWhenClick;
    public event UnityAction onCompletedWait;
    public float SecondsTimer { get => secondsTimer; set => secondsTimer = value; }

    protected override void OnEnable()
    {
#if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            return;
        }
#endif
        ReloadCurrentState();
    }
    protected override void Start()
    {
        base.Start();
        onClick.AddListener(() =>
        {
            interactable = false;
            if (isAutoSaveTicksWhenClick)
            {
                SaveTicks();
                ReloadCurrentState();
            }
        });
    }
    IEnumerator TimerCoroutine(TimeSpan remainTs)
    {
        var oneSecTs = TimeSpan.FromSeconds(1);
        WaitForSecondsRealtime waitOneSec = new WaitForSecondsRealtime(1);
        while (remainTs.TotalSeconds > 0)
        {
            txtTimer.text = remainTs.ToString("mm\\:ss");
            yield return waitOneSec;
            remainTs = remainTs.Subtract(oneSecTs);
        }
        txtTimer.text = txtAtFirst;
        interactable = true;
        onCompletedWait?.Invoke();
    }
    public void SaveTicks()
    {
        PlayerPrefs.SetString(ticksSaveKey, DateTime.Now.Ticks.ToString());
    }
    public void ReloadCurrentState()
    {
        interactable = true;
        if (PlayerPrefs.HasKey(ticksSaveKey))
        {
            var savedDateTime = new DateTime(long.Parse(PlayerPrefs.GetString(ticksSaveKey)));
            var deltaTicks = DateTime.Now - savedDateTime;
            var remainTick = TimeSpan.FromSeconds(SecondsTimer) - deltaTicks;
            if (remainTick.Ticks > 0)
            {
                interactable = false;
                if(gameObject.activeInHierarchy)
                {
                    StartCoroutine(TimerCoroutine(remainTick));
                }
            }
        }
    }
}
