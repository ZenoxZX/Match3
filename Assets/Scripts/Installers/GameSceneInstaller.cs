using Game;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameSceneInstaller : MonoInstaller
    {
        [SerializeField] private GameManager m_GameManager;
        
        public override void InstallBindings()
        {
            InstallManager();
            InstallEvents();
            InstallFactories();
        }

        private void InstallManager()
        {
            Container.Bind<GameManager>().FromInstance(m_GameManager).AsSingle();
            Container.BindInterfacesAndSelfTo<InputHandler>().AsSingle();
        }

        private void InstallEvents()
        {
            Container.Bind<GameEvents>().AsSingle();
        }

        private void InstallFactories()
        {
            Container.BindFactory<Shape, Shape.Factory>().FromNewComponentOnNewGameObject().AsSingle();
        }
    }
}