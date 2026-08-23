using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Health : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [field: SerializeField]
        public int Current { get; set; } = 50;

        ///Const
        [field: SerializeField]
        public int Max { get; private set; } = 100;
        
        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);
        
        public void Deserialize(IComponentSerializer serializer, JToken data)
            => serializer.Deserialize(this, data);
    }
}