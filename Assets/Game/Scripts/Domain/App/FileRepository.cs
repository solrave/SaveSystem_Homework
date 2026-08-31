using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Game.Scripts.Domain.App
{
    public class FileRepository : IRepository
    {
        private string _filePath;
        private int _version;

        private string DirectoryPath => Path.GetDirectoryName(_filePath);
        private string BaseName => Path.GetFileNameWithoutExtension(_filePath);
        private string Extension => Path.GetExtension(_filePath);
        private string DataPath => Path.Combine(DirectoryPath, $"{BaseName}_{_version}{Extension}");
        
        public FileRepository(string filePath)
        {
            _filePath = filePath;
            _version = FindLatestVersion();
        }
        
        public (bool, int) Save(JObject data)
        {
            if (data == null)
            {
                Debug.Log($"{this.GetType().Name}: Save failed!");
                return (false, 0);
            }

            _version = FindLatestVersion();
            _version++;
            
            var dataString = data.ToString(Formatting.None);
            string checksum = ComputeChecksum(dataString);
            
            var savedData = new JObject()
            {
                ["Version"] = _version,
                ["Checksum"] = checksum,
                ["Entities"] = data
            };

            try
            {
                using (FileStream fileStream = new FileStream(DataPath, FileMode.Create, FileAccess.Write))
                using (StreamWriter streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
                using (JsonTextWriter jsonTextWriter = new JsonTextWriter(streamWriter))
                {
                    savedData.WriteTo(jsonTextWriter);
                }
                
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
            _version = version;
            string path = DataPath;
            
            if (!File.Exists(path))
            {
                Debug.Log($"{this.GetType().Name}: Loading failed!");
                return (false, 0);
            }

            try
            {
                using (FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using (StreamReader streamReader = new StreamReader(fileStream, Encoding.UTF8))
                using (JsonTextReader jsonTextReader = new JsonTextReader(streamReader))
                {
                    var loadedData = JObject.Load(jsonTextReader);
                
                    string storedChecksum = loadedData["Checksum"]?.Value<string>();
                    string actualChecksum = ComputeChecksum(loadedData["Entities"].ToString(Formatting.None));

                    if (storedChecksum != actualChecksum)
                    {
                        Debug.LogError($"{this.GetType().Name}: Save file is corrupted!");
                        return (false, 0);
                    }
                    
                    data = loadedData["Entities"].Value<JObject>();
                }
                
                Debug.Log($"{this.GetType().Name}: Loaded successfully! Version:{_version}");
                return (true, _version);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return (false, 0);
            }
        }

        private int FindLatestVersion()
        {
            if (!Directory.Exists(DirectoryPath)) return 0;
            
            int latest = 0;
            foreach (string file in Directory.GetFiles(DirectoryPath, $"{BaseName}_*{Extension}"))
            {
                string versionPart = Path.GetFileNameWithoutExtension(file).Substring(BaseName.Length + 1);
                if (int.TryParse(versionPart, out int v) && v > latest)
                    latest = v;
            }
            return latest;
        }
        
        private static string ComputeChecksum(string content)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }
}