using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Daily_planner
{
    public class JsonSerializationDeserialization
    {
        public static void Serialize<T>(IEnumerable<T> data, string filePath)
        {
            string jsonData = JsonConvert.SerializeObject(data);
            File.WriteAllText(filePath, jsonData);
        }

        public static IEnumerable<T> Deserialize<T>(string filePath)
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonData);
            }
            return null;
        }
    }
}
