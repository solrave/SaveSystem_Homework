using Game.Scripts.Domain.App;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.Gameplay.SaveSystem
{
    public class DebugRepository : IRepository
    {
        private IRepository _origin;

        public DebugRepository(IRepository origin)
        {
            _origin = origin;
        }

        public (bool, int) Save(JObject data)
        {
            (bool success, int version) = _origin.Save(data);
            
            if (success)
            {
                Debug.Log($"{this.GetType().Name}: Saved successfully!");
            }
            else
            {
                Debug.Log($"{this.GetType().Name}: Saving failed!");
            }

            return (success, version);
        }

        public (bool, int) TryLoad(int version, out JObject data)
        {
            (bool success, int loadedVersion) = _origin.TryLoad(version, out data);
            
            if (success)
            {
                Debug.Log($"{this.GetType().Name}: Loaded successfully! Version: {loadedVersion}");
            }
            else
            {
                Debug.Log($"{this.GetType().Name}: Loading failed.Version: {loadedVersion}");
            }
            return (success, loadedVersion);
        }
    }
}