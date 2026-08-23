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

        public FileRepository(string filePath)
        {
            _filePath = filePath;
        }

        public bool Save(JObject data)
        {
            string json = data.ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(json);

            try
            {
                File.WriteAllBytes(_filePath, bytes);
                return true;
            }
            
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool TryLoad(out JObject data)
        {
            throw new NotImplementedException();
        }
    }
}