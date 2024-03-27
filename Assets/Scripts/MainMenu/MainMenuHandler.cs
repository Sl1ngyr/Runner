using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MainMenuHandler : MonoBehaviour, IPointerDownHandler
    {
        [SerializeField] private Button _exitGame;
        [SerializeField] private Button _logOut;
        [SerializeField] private int _authSceneBuildIndex;
        
        public event Action onStateMenuChanged;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            onStateMenuChanged?.Invoke();
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
            SceneManager.LoadScene(_authSceneBuildIndex);
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
        
    }
}