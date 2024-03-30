using Services.Scene;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.EndGame
{
    public class EndGameMenuPanel : MonoBehaviour
    {
        [SerializeField] private Button _restartLevel;
        [SerializeField] private int _gameSceneBuildIndex = 1;

        [Inject] private SceneLoader _sceneLoader;

        
        private void RestartLevel()
        {
            _sceneLoader.TransitionToSceneByIndex(_gameSceneBuildIndex);
        }
        
        private void OnEnable()
        {
            _restartLevel.onClick.AddListener(RestartLevel);
        }

        private void OnDisable()
        {
            _restartLevel.onClick.RemoveListener(RestartLevel);
        }
    }
}