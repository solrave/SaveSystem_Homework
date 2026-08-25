using System;
using System.Collections.Generic;
using Game.Scripts.Domain.App;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using SampleGame.Gameplay;
using UnityEngine;

namespace Game.Gameplay
{
    public class SaveManager
    {
        private IRepository _repository;
        private IComponentSerializer _serializer;
        private EntityWorld _entityWorld;

        public SaveManager(IRepository repository,
                           IComponentSerializer serializer,
                           EntityWorld entityWorld)
        {
            _repository = repository;
            _serializer = serializer;
            _entityWorld = entityWorld;
        }

        public (bool, int) Save()
        {
            var saveData = new JObject();
            List <Entity> allEntities = new List<Entity>(_entityWorld.GetAll());
            
            foreach (var entity in allEntities)
            {
                var componentData = new JObject();
                
                foreach (var component in entity.GetComponents<ISerializableComponent>())
                {
                    componentData[component.GetType().Name] = component.Serialize(_serializer);
                }
                
                componentData["Transform"] = new JObject
                {
                    ["position"] = JObject.FromObject((SerializedVector3)entity.transform.position),
                    ["rotation"] = JObject.FromObject((SerializedVector3)entity.transform.rotation)
                };
                saveData[entity.Type] = componentData;
            }
            return _repository.Save(saveData);
        }

        public (bool, int) Load(int version)
        {
            (bool result, int loadVersion) = _repository.TryLoad(version, out var data);
            
            if (result) //saveData[entity.Type] = componentData;
            {
                foreach (var property in data.Properties())
                {
                    var type = property.Name; //entity Type
                    var components = (JObject)property.Value; // components Dictionary
                    var transform = (JObject)components["Transform"]; //concrete component Dictionary
                    Vector3 position = transform["position"].ToObject<SerializedVector3>(); //concrete value from component
                    Quaternion rotation = transform["rotation"].ToObject<SerializedVector3>(); //concrete value from component
                    _entityWorld.Spawn(type, position, rotation);
                }
            }
            
            return (result, loadVersion);
        }
    }
}
