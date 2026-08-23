using System;
using Game.Scripts.Domain.App;
using Newtonsoft.Json.Linq;
using SampleGame.Gameplay;

namespace Game.Gameplay
{
    public class SaveManager
    {
        private IRepository _repository;
        private IComponentSerializer _serializer;
        private ISerializableComponent[] _serializableComponents;

        public SaveManager(IRepository repository,
                           IComponentSerializer serializer,
                           ISerializableComponent[] serializableComponents)
        {
            _repository = repository;
            _serializer = serializer;
            _serializableComponents = serializableComponents;
        }

        public void Save()
        {
            var gameData = new JObject();
            
            foreach (var component in _serializableComponents)
            {
                gameData.Add(component.GetType().Name, component.Serialize(_serializer));    
            }

            _repository.Save(gameData);
        }
        
        public void Load(){}
    }
}