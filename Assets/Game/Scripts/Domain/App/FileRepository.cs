using System;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public class FileRepository : IRepository
    {
        private string _filePath;
        private int _version;

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public (bool, int) Save(JObject data)
        {
            if (data == null)
            {
                Debug.Log($"{this.GetType().Name}: Save failed!");
                return (false, 0);
            }
            
            _version++;
            data["Version"] = _version;
            
            string json = data.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            try
            {
                File.WriteAllBytes(_filePath, bytes);
                Debug.Log($"{this.GetType().Name}: Saved successfully!");
                return (true, _version);
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, 0);
            }
        }

        public (bool, int) TryLoad(int version, out JObject data)
        {
            data = null;

            if (!File.Exists(_filePath))
            {
                Debug.Log($"{this.GetType().Name}: Loading failed!");
                return (false, 0);
            }

            try
            {
                byte[] bytes = File.ReadAllBytes(_filePath);
                string json = Encoding.UTF8.GetString(bytes);
                data = JObject.Parse(json);
                Debug.Log($"{this.GetType().Name}: Loaded successfully!");
                return (true, _version);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, 0);
            }
        }
    }
}