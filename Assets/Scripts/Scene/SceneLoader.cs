using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        public void TransitionToSceneByIndex(int index)
        {
            SceneManager.LoadScene(index);
        }
    }
}