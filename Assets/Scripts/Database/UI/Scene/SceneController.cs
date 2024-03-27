using Database.Firebase;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Scene
{
    public class SceneController : MonoBehaviour
    {
        [SerializeField] private AuthenticationManager _authenticationManager;

        private void OnEnable()
        {
            _authenticationManager.onSceneLoaded += TransitionToSceneByIndex;
        }

        private void OnDisable()
        {
            _authenticationManager.onSceneLoaded -= TransitionToSceneByIndex;
        }

        private void TransitionToSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}