using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class GamePlayUI : MonoBehaviour
    {
        [SerializeField] private Button btnLoadScene;
        void Start()
        {
            btnLoadScene.onClick.RemoveAllListeners();
            btnLoadScene.onClick.AddListener(() =>
            {
                LoadingSceneController.Instance.ChangeScene(SceneType.GamePlay);
            });
        }
    }
}
