using Services;
using Services.Scene;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.MainMenu
{
    public class MainMenuHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Button _exitGame;
        [SerializeField] private Button _logOut;
        [SerializeField] private int _logOutSceneBuildIndex = 0;
        
        [Inject] private SceneLoader _sceneLoader;

        public void OnPointerDown(PointerEventData eventData)
        {
            EventBus.Instance.onGameStarted?.Invoke();
            gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            _exitGame.onClick.AddListener(QuitGame);
            _logOut.onClick.AddListener(LogOut);
        }

        private void OnDisable()
        {
            _exitGame.onClick.RemoveListener(QuitGame);
            _logOut.onClick.RemoveListener(LogOut);
        }

        private void LogOut()
        {
            _sceneLoader.TransitionToSceneByIndex(_logOutSceneBuildIndex);
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
        
    }
}