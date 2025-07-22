using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceneController : Singleton<LoadingSceneController>
{
    [SerializeField] private SceneType currentScene;
    [SerializeField] private LoadingSceneView loadingSceneView;
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        loadingSceneView.FadeOutBlackScreen();
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = false;
        loadingSceneView.SetVersion();
        loadingSceneView.AnimationFillBar(() =>
        {
            ChangeScene(SceneType.GamePlay, () => loadingSceneView.DisableLoadingScreen());
        });
    }

    public void ChangeScene(SceneType sceneType, UnityAction onCompleteFadeIn = null)
    {
        currentScene = sceneType;
        loadingSceneView.ChangSceneAnimation(() =>
        {
            SceneManager.LoadScene($"{sceneType}");
            onCompleteFadeIn?.Invoke();
        });
    }
}
public enum SceneType
{
    Loading = 0,
    Main = 1,
    GamePlay = 2,
}