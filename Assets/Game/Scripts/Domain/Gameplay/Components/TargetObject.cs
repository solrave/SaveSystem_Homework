using Modules.Entities;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class TargetObject : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [field: SerializeField]
        public Entity Value { get; set; }
        
        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);

        public void Deserialize(IComponentSerializer serializer, JToken data)
            => serializer.Deserialize(this, data);
    }
}