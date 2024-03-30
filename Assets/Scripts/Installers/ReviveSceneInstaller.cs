using Ad;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ReviveSceneInstaller : MonoInstaller
    {
        [SerializeField] private ReviveManager _reviveManager;
    
        public override void InstallBindings()
        {
            Container.Bind<ReviveManager>().FromInstance(_reviveManager).AsSingle();
        }
    }
}