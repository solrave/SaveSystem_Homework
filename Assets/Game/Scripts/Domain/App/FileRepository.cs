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
            var savedData = new JObject()
            {
                ["Version"] = _version,
                ["Entities"] = data
            };
            
            string json = savedData.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            string path = DataPath;

            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    fs.Write(bytes, 0, bytes.Length);
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
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    byte[] bytes = new byte[fs.Length];
                    int totalRead = 0;
                    
                    while (totalRead < bytes.Length)
                    {
                        int bytesRead = fs.Read(bytes, totalRead, bytes.Length - totalRead);
                        if (bytesRead == 0) break;
                        totalRead = bytesRead;
                    }
                    
                    string json = Encoding.UTF8.GetString(bytes);
                    var loadedData = JObject.Parse(json);
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
    }
}