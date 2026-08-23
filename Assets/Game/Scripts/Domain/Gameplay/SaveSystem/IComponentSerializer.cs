using System;
using System.Collections.Generic;
using System.Linq;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using SampleGame.Common;
using UnityEngine;

namespace SampleGame.Gameplay
{
    public interface IComponentSerializer
    {
        JToken Serialize(Countdown component);
        void Deserialize(Countdown component, JToken token);
        
        JToken Serialize(DestinationPoint component);
        void Deserialize(DestinationPoint component, JToken token);
        
        JToken Serialize(Health component);
        void Deserialize(Health component, JToken token);
        
        JToken Serialize(ProductionOrder component);
        void Deserialize(ProductionOrder component, JToken token);
        
        JToken Serialize(ResourceBag component);
        void Deserialize(ResourceBag component, JToken token);
        
        JToken Serialize(TargetObject component);
        void Deserialize(TargetObject component, JToken token);
        
        JToken Serialize(Team component);
        void Deserialize(Team component, JToken token);

    }

    public sealed class ComponentSerializer : IComponentSerializer
    {
        private EntityCatalog _entityCatalog;
        private EntityWorld _entityWorld;

        public ComponentSerializer(EntityCatalog entityCatalog, EntityWorld entityWorld)
        {
            _entityCatalog = entityCatalog;
            _entityWorld = entityWorld;
        }

        #region Countdown

        public JToken Serialize(Countdown component) => JToken.FromObject(component.Current);
        public void Deserialize(Countdown component, JToken token) => component.Current = token.ToObject<float>();

        #endregion

        #region DestinationPoint

        public JToken Serialize(DestinationPoint component) => JToken.FromObject(component.Value);
        public void Deserialize(DestinationPoint component, JToken token) => component.Value = token.ToObject<Vector3>();

        #endregion

        #region Health

        public JToken Serialize(Health component) => JToken.FromObject(component.Current);
        public void Deserialize(Health component, JToken token) => component.Current = token.ToObject<int>();

        #endregion

        #region Production_Order

        public JToken Serialize(ProductionOrder component)
        {
            var names = component.Queue
                .Select(config => config.name)
                .ToList();
            
            return JToken.FromObject(names); 
        }

        public void Deserialize(ProductionOrder component, JToken token)
        {
            var names = token.ToObject<List<string>>();
            var configs = new List<EntityConfig>(names.Count);
        
            foreach (var name in names)
            {
                if (_entityCatalog.FindConfig(name, out EntityConfig config))
                    configs.Add(config);
            }
        
            component.Queue = configs;
        }
        #endregion

        #region ResourceBag

        public JToken Serialize(ResourceBag component)
        {
            return new JObject
            {
                ["Type"] = new JValue(component.Type.ToString()),
                ["Current"] = new JValue(component.Current)
            };
        }

        public void Deserialize(ResourceBag component, JToken token)
        {
            component.Type = Enum.Parse<ResourceType>(token["Type"].Value<string>());
            component.Current = token["Current"].Value<int>();
        }

        #endregion

        #region TargetObject

        public JToken Serialize(TargetObject component) => JToken.FromObject(component.Value.Id);

        public void Deserialize(TargetObject component, JToken token)
        {
            var id = token.Value<int>();
            
            if (_entityWorld.TryGet(id, out var entity))
                component.Value = entity;
        }
        
        #endregion

        #region Team

        public JToken Serialize(Team component) => JToken.FromObject(component.Type);

        public void Deserialize(Team component, JToken token)
            => component.Type = Enum.Parse<TeamType>(token.ToString());

        #endregion
    }
    
    public interface ISerializableComponent
    {
        JToken Serialize(IComponentSerializer serializer);
        void Deserialize(IComponentSerializer serializer, JToken data);
    }
}