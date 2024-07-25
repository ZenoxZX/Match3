using Data;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<ProjectSettings>().FromResources(nameof(ProjectSettings)).AsSingle();
        }
    }
}