namespace Game.Scripts.Domain.Gameplay.SaveSystem
{
    public class TestVault
    {
        // foreach (var entity in allEntities)
        // {
        //     allComponents.Add(entity.Id, entity.GetComponents<ISerializableComponent>());
        // }
        //         
        // foreach (var pair in allComponents)
        // {
        //     string Id = pair.Key.ToString();
        //     ISerializableComponent[] components = pair.Value;
        //     var componentData = new JObject();
        //         
        //     foreach (var component in components)
        //     {
        //         componentData[component.GetType().Name] = component.Serialize(_serializer);
        //     }
        //             
        //     if (_entityWorld.TryGet(pair.Key, out Entity entity))
        //     {
        //         componentData["Transform"] = new JObject
        //         {
        //             ["position"] = JObject.FromObject((SerializedVector3)entity.transform.position),
        //             ["rotation"] = JObject.FromObject((SerializedVector3)entity.transform.rotation)
        //         };
        //     }
        //             
        //     saveData.Add(Id, componentData);
        // }
    }
}