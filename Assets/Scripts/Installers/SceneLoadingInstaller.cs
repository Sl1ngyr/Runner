using Scene;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneLoadingInstaller : MonoInstaller
    {
        [SerializeField] private SceneLoader _sceneLoader;
        public override void InstallBindings()
        {
            Container.Bind<SceneLoader>().FromInstance(_sceneLoader);
        }
    }
}