using UnityEngine;

namespace Services
{
    public class SkyboxManager : MonoBehaviour

    {
        private const string SKYBOX_ROTATION = "_Rotation";
        
        [SerializeField] private float speed;


        private void Update()
        {
            RenderSettings.skybox.SetFloat(SKYBOX_ROTATION, Time.time * speed);
        }
    }
}