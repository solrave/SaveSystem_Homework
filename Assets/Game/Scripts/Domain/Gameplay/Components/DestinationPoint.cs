using Game.Gameplay;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class DestinationPoint : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [field: SerializeField]
        public Vector3 Value { get; set; }

        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);
        
        public void Deserialize(IComponentSerializer serializer, JToken data)
            => serializer.Deserialize(this, data);
    }
}