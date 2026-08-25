using SampleGame.Gameplay;
using Zenject;

namespace Game.Gameplay
{
    public class SceneInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ComponentSerializer>()
                .AsSingle();

            this.Container.Bind<ISerializableComponent>()
                .FromComponentsInHierarchy()
                .AsCached();
            
            this.Container.BindInterfacesAndSelfTo<SaveManager>()
                .AsSingle();

            this.Container.BindInterfacesAndSelfTo<ControlsPresenter>()
                .AsSingle();
        }
    }
}