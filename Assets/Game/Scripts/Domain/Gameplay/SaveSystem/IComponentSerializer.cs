using Newtonsoft.Json.Linq;

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
}