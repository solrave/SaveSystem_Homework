using Newtonsoft.Json.Linq;
using UnityEngine;

namespace SampleGame.Gameplay
{
    //Can be extended
    public sealed class Countdown : MonoBehaviour, ISerializableComponent
    {
        ///Variable
        [field: SerializeField]
        public float Current { get; set; }

        ///Const
        [field: SerializeField]
        public float Duration { get; private set; }

        public JToken Serialize(IComponentSerializer serializer)
            => serializer.Serialize(this);

        public void Deserialize(IComponentSerializer serializer, JToken data)
            => serializer.Deserialize(this, data);
    }
}