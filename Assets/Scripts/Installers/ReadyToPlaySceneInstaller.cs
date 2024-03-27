using MainMenu;
using UnityEngine;
using Zenject;

public class ReadyToPlaySceneInstaller : MonoInstaller
{
    [SerializeField] private MainMenuHandler _mainMenuHandler;
    
    public override void InstallBindings()
    {
        Container.Bind<MainMenuHandler>().FromInstance(_mainMenuHandler);
    }
}