using Newtonsoft.Json.Linq;

namespace SampleGame.Gameplay
{
    public interface ISerializableComponent
    {
        JToken Serialize(IComponentSerializer serializer);
        void Deserialize(IComponentSerializer serializer, JToken data);
    }
}