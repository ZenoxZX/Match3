using Data;
using SaveSystem;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProjectSettings>().FromResources(nameof(ProjectSettings)).AsSingle();
            InstallSaveSystem();
        }

        private void InstallSaveSystem()
        {
            Container.Bind<ISerializer>().To<JsonSerializer>().AsSingle();
            Container.Bind<IDataService>().To<FileDataService>().AsSingle();
            Container.BindInterfacesAndSelfTo<SaveManager>().AsSingle();
        }
    }
}