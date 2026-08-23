using Newtonsoft.Json.Linq;

namespace Game.Gameplay
{
    public interface ISaveSerializer
    {
        string Key => this.GetType().Name;
        JToken Serialize();
        void Deserialize(JToken data);
    }

    public interface ISaveSerializer<T> : ISaveSerializer
    {
        JToken ISaveSerializer.Serialize() => JToken.FromObject(this.Serialize());
        void ISaveSerializer.Deserialize(JToken data) => this.Deserialize(data.ToObject<T>());
        new T Serialize();
        void Deserialize(T value);
    }
}