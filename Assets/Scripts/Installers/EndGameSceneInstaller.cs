using Player;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EndGameSceneInstaller : MonoInstaller
    {
        [SerializeField] private CollisionDetector _collisionDetector;
        [SerializeField] private EndGameHandler _endGameHandler;

        public override void InstallBindings()
        {
            Container.Bind<CollisionDetector>().FromInstance(_collisionDetector).AsSingle();
            Container.Bind<EndGameHandler>().FromInstance(_endGameHandler).AsSingle();
        }
    }
}