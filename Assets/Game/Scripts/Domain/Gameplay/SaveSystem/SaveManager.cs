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
                componentData["Name"] = entity.Name;
                
                foreach (var component in entity.GetComponents<ISerializableComponent>())
                {
                    componentData[component.GetType().Name] = component.Serialize(_serializer);
                }
                
                componentData["Transform"] = new JObject
                {
                    ["position"] = JObject.FromObject((SerializedVector3)entity.transform.position),
                    ["rotation"] = JObject.FromObject((SerializedVector3)entity.transform.rotation)
                };

                saveData[entity.Id.ToString()] = componentData;
            }
            return _repository.Save(saveData);
        }

        public (bool, int) Load(int version)
        {
            _entityWorld.DestroyAll();
            (bool result, int loadVersion) = _repository.TryLoad(version, out var loadedData);
            
            if (result) //saveData[entity.Type] = componentData;
            {
                Dictionary<int, Entity> entities = new();
                foreach (var pair in loadedData)
                {
                    var id = int.Parse(pair.Key); //entity Id
                    var components = pair.Value;
                    var name = components["Name"].ToString();
                    var transform = (JObject)components["Transform"]; //concrete component Dictionary
                    Vector3 position = transform["position"].ToObject<SerializedVector3>(); //concrete value from component
                    Quaternion rotation = transform["rotation"].ToObject<SerializedVector3>(); //concrete value from component
                    var entity = _entityWorld.Spawn(name, position, rotation, id);
                    entities[id] = entity;
                }
                
                // foreach (var property in data.Properties())
                // {
                //     var id = int.Parse(property.Name); //entity Id
                //     var components = (JObject)property.Value; // components Dictionary
                //     var name = components["Name"].Value<string>();
                //     var transform = (JObject)components["Transform"]; //concrete component Dictionary
                //     Vector3 position = transform["position"].ToObject<SerializedVector3>(); //concrete value from component
                //     Quaternion rotation = transform["rotation"].ToObject<SerializedVector3>(); //concrete value from component
                //     var entity = _entityWorld.Spawn(name, position, rotation, id);
                //     entities[id] = entity;
                // }

                foreach (var entity in entities)
                {
                    foreach (var component in entity.Value.GetComponents<ISerializableComponent>())
                    {
                        //if (data.TryGetValue(component.GetType().Name, out JToken token))
                        if (loadedData[entity.Key.ToString()].Value<JObject>().TryGetValue(component.GetType().Name, out JToken token))
                        {
                            component.Deserialize(_serializer, token);
                        }
                    }
                }
            }
            
            return (result, loadVersion);
        }
    }
}
