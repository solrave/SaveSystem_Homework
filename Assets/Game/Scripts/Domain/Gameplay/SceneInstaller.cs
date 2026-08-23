using System.IO;
using Game.Scripts.Domain.App;
using SampleGame.Gameplay;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField]
        private string _fileName;
        
        public override void InstallBindings()
        {
            this.Container.BindInterfacesAndSelfTo<ComponentSerializer>()
                .AsSingle();

            this.Container.Bind<ISerializableComponent>()
                .FromComponentsInHierarchy()
                .AsCached();
            
            this.Container.BindInterfacesAndSelfTo<SaveManager>()
                .AsSingle();

            this.Container.Bind<IRepository>()
                .To<FileRepository>()
                .AsSingle()
                .WithArguments(Path.Combine(Application.persistentDataPath, _fileName));

            this.Container.BindInterfacesAndSelfTo<ControlsPresenter>()
                .AsSingle();
        }
    }
}