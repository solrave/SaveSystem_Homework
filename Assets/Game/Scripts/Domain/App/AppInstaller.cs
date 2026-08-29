using System.IO;
using Game.Scripts.Domain.Gameplay.SaveSystem;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Domain.App
{
    public class AppInstaller : MonoInstaller
    {
        [SerializeField]
        private string _fileName = "GameSave.txt";
        
        public override void InstallBindings()
        {
            this.Container.Bind<IRepository>()
                .To<FileRepository>()
                .AsSingle()
                .WithArguments(Path.Combine(Application.persistentDataPath, _fileName));
                //.WithArguments(Application.persistentDataPath);

            this.Container.Decorate<IRepository>().With<DebugRepository>();
        }
    }
}