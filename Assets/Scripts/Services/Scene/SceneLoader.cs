using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services.Scene
{
    public class SceneLoader : MonoBehaviour
    {
        public void TransitionToSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}