using UI.MainMenu;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class StartGameSceneInstaller : MonoInstaller
    {
        [SerializeField] private MainMenuHandler _mainMenuHandler;
    
        public override void InstallBindings()
        {
            Container.Bind<MainMenuHandler>().FromInstance(_mainMenuHandler).AsSingle();
        }
    }
}