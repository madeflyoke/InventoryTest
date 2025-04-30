using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Core.Utils.PlayerData
{
    public static class JsonSaver
    {
        public static void Save<T>(T data, string fileName)
        {
            string json = JsonConvert.SerializeObject(data);
            File.WriteAllText(GetPath(fileName), json);
        }

        public static T Load<T>(string fileName)
        {
            string path = GetPath(fileName);
            if (File.Exists(path))
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
            return default(T);
        }

        private static string GetPath(string fileName) 
            => Path.Combine(Application.persistentDataPath, fileName);
    }
}