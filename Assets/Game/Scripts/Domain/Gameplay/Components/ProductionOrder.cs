using System.Collections.Generic;
using Game.Gameplay;
using Modules.Entities;
using Newtonsoft.Json.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class ProductionOrder : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [SerializeField]
        private List<EntityConfig> _queue;
        
        public IReadOnlyList<EntityConfig> Queue
        {
            get { return _queue; }
            set { _queue = new List<EntityConfig>(value); }
        }
        
        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);

        public void Deserialize(IComponentSerializer serializer, JToken data)
            => serializer.Deserialize(this, data);
    }
}