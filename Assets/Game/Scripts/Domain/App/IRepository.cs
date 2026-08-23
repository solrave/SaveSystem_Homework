using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.App
{
    public interface IRepository
    {
        bool Save(JObject data);
        bool TryLoad(out JObject data);
    }
}