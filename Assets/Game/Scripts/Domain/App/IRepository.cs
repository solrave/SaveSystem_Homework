using Newtonsoft.Json.Linq;

namespace Game.Scripts.Domain.App
{
    public interface IRepository
    {
        (bool, int) Save(JObject data);
        (bool, int) TryLoad(int version, out JObject data);
    }
}